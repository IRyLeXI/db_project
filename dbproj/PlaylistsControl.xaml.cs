using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Npgsql;

namespace dbproj
{
    public partial class PlaylistsControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Database=SongsDB;Username=postgres;Password=192837465";
        private int userId = 1;

        public PlaylistsControl()
        {
            InitializeComponent();
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Playlists\".\"Name\" AS PlaylistName, \"Users\".\"Nickname\" AS CreatorName, \"Playlists\".\"Picture\" AS Picture, \"Playlists\".\"Updated_Date\" AS UpdatedDate " +
                                   "FROM public.\"Playlists\" " +
                                   "LEFT JOIN public.\"Users\" ON \"Playlists\".\"Owner\" = \"Users\".\"ID\" " +
                                   "WHERE \"Playlists\".\"Owner\" = @UserId";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string playlistName = row["PlaylistName"].ToString();
                                string creatorName = row["CreatorName"].ToString();
                                string picturePath = row["Picture"].ToString();
                                string updatedDate = row["UpdatedDate"].ToString(); // Assuming the column name is Updated_Date in the database

                                playlists.Add(new Playlist { PlaylistName = playlistName, CreatorName = creatorName, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), UpdatedDate = updatedDate });
                            }

                            playlistItemsControl.ItemsSource = playlists;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = searchTextBox.Text.Trim();
            DateTime? selectedDate = datePicker.SelectedDate;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Playlists\".\"Name\" AS PlaylistName, \"Users\".\"Nickname\" AS CreatorName, " +
                                   "\"Playlists\".\"Picture\" AS Picture, \"Playlists\".\"Updated_Date\" AS UpdatedDate " +
                                   "FROM public.\"Playlists\" " +
                                   "LEFT JOIN public.\"Users\" ON \"Playlists\".\"Owner\" = \"Users\".\"ID\" " +
                                   "WHERE 1 = 1 ";

                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        query += "AND \"Playlists\".\"Name\" ILIKE '%' || @SearchInput || '%' ";
                    }

                    if (selectedDate != null)
                    {
                        query += "AND \"Playlists\".\"Updated_Date\" >= @SelectedDate ";
                    }

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(searchInput))
                        {
                            command.Parameters.AddWithValue("@SearchInput", searchInput);
                        }

                        if (selectedDate != null)
                        {
                            command.Parameters.AddWithValue("@SelectedDate", selectedDate);
                        }

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string playlistName = row["PlaylistName"].ToString();
                                string creatorName = row["CreatorName"].ToString();
                                string picturePath = row["Picture"].ToString();
                                string updatedDate = row["UpdatedDate"].ToString();

                                playlists.Add(new Playlist { PlaylistName = playlistName, CreatorName = creatorName, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), UpdatedDate = updatedDate });
                            }

                            playlistItemsControl.ItemsSource = playlists;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }

    public class Playlist
    {
        public string PlaylistName { get; set; }
        public string CreatorName { get; set; }
        public string UpdatedDate { get; set; }
        public BitmapImage Picture { get; set; }
    }
}
