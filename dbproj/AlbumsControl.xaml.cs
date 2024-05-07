using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Npgsql;

namespace dbproj
{
    public partial class AlbumsControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Database=SongsDB;Username=postgres;Password=192837465";

        public AlbumsControl()
        {
            InitializeComponent();
            LoadAlbums();
        }

        private void LoadAlbums()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Albums\".\"Name\" AS AlbumName, \"Authors\".\"Nickname\" AS AuthorName, \"Albums\".\"Picture\" AS Picture, \"Albums\".\"Created_Date\" AS CreatedDate " +
                                   "FROM public.\"Albums\" " +
                                   "LEFT JOIN public.\"Authors\" ON \"Albums\".\"Author\" = \"Authors\".\"ID\""; ;

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ObservableCollection<Album> albums = new ObservableCollection<Album>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string albumName = row["AlbumName"].ToString();
                                string authorName = row["AuthorName"].ToString();
                                string picturePath = row["Picture"].ToString();
                                string createdDate = row["CreatedDate"].ToString();

                                albums.Add(new Album { AlbumName = albumName, AuthorName = authorName, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), CreatedDate = createdDate });
                            }

                            playlistItemsControl.ItemsSource = albums;
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

                    string query = "SELECT \"Albums\".\"Name\" AS AlbumName, \"Authors\".\"Nickname\" AS AuthorName, " +
                                   "\"Albums\".\"Picture\" AS Picture, \"Albums\".\"Created_Date\" AS CreatedDate " +
                                   "FROM public.\"Albums\" " +
                                   "LEFT JOIN public.\"Authors\" ON \"Albums\".\"Author\" = \"Authors\".\"ID\" " +
                                   "WHERE 1 = 1 ";

                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        query += "AND \"Albums\".\"Name\" ILIKE '%' || @SearchInput || '%' ";
                    }

                    if (selectedDate != null)
                    {
                        query += "AND \"Albums\".\"Created_Date\" <= @SelectedDate ";
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

                            ObservableCollection<Album> albums = new ObservableCollection<Album>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string albumName = row["AlbumName"].ToString();
                                string authorName = row["AuthorName"].ToString();
                                string picturePath = row["Picture"].ToString();
                                string createdDate = row["CreatedDate"].ToString();

                                albums.Add(new Album { AlbumName = albumName, AuthorName = authorName, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), CreatedDate = createdDate });
                            }

                            playlistItemsControl.ItemsSource = albums;
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

    public class Album
    {
        public string AlbumName { get; set; }
        public string AuthorName { get; set; }
        public string CreatedDate { get; set; }
        public BitmapImage Picture { get; set; }
    }
}
