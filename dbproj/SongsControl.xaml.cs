using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Npgsql;
using System.Collections.Generic;

namespace dbproj
{
    public partial class SongsControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string connectionString = "Host=localhost;Port=5432;Database=SongsDB;Username=postgres;Password=192837465";

        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set
            {
                _songs = value;
                OnPropertyChanged(nameof(Songs));
            }
        }

        public SongsControl()
        {
            LoadDataFromDatabase();
            DataContext = this;
            InitializeComponent();
            LoadAuthors();
            LoadGenres();
        }

        private void LoadDataFromDatabase()
        {
            string query = "SELECT \"Songs\".*, \"Authors\".\"Nickname\" AS \"AuthorNickname\" " +
                           "FROM public.\"Songs\" " +
                           "LEFT JOIN public.\"Authors\" ON \"Songs\".\"Author\" = \"Authors\".\"ID\" " +
                           "ORDER BY \"Songs\".\"ID\"";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            Songs = new ObservableCollection<Song>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                List<string> genreslist = GetGenresForSong(Convert.ToInt32(row["ID"]));
                                string genres = string.Join(", ", genreslist);

                                Songs.Add(new Song
                                {
                                    Name = row["Name"].ToString(),
                                    AuthorName = row["AuthorNickname"].ToString(),
                                    Duration = row["Duration"].ToString(),
                                    Listened_Num = Convert.ToInt32(row["Listened_Num"]),
                                    Picture = new BitmapImage(new Uri(row["Picture"].ToString(), UriKind.RelativeOrAbsolute)),
                                    Genres = genres
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private List<string> GetGenresForSong(int songID)
        {
            List<string> genres = new List<string>();

            string query = "SELECT \"Genres\".\"Name\" FROM public.\"Genres\" " +
                           "INNER JOIN public.\"Songs_Genres\" ON \"Genres\".\"ID\" = \"Songs_Genres\".\"Genre_ID\" " +
                           "WHERE \"Songs_Genres\".\"Song_ID\" = @SongID";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SongID", songID);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                genres.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return genres;
        }

        private void LoadAuthors()
        {
            string query = "SELECT DISTINCT \"Nickname\" AS \"Author\" FROM public.\"Authors\"";
            authorComboBox.Items.Clear();
            authorComboBox.Items.Add("");

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string author = reader["Author"].ToString();
                                authorComboBox.Items.Add(author);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void LoadGenres()
        {
            string query = "SELECT \"Name\" FROM public.\"Genres\"";
            genreComboBox.Items.Clear();
            genreComboBox.Items.Add("");
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string genre = reader["Name"].ToString();
                                genreComboBox.Items.Add(genre);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = searchTextBox.Text.Trim();
            string selectedAuthor = authorComboBox.SelectedValue?.ToString();
            string selectedGenre = genreComboBox.SelectedValue?.ToString();

            string query = "SELECT DISTINCT \"Songs\".*, \"Authors\".\"Nickname\" AS \"AuthorNickname\" " +
                           "FROM public.\"Songs\" " +
                           "LEFT JOIN public.\"Authors\" ON \"Songs\".\"Author\" = \"Authors\".\"ID\" " +
                           "LEFT JOIN public.\"Songs_Genres\" ON \"Songs\".\"ID\" = \"Songs_Genres\".\"Song_ID\" " +
                           "LEFT JOIN public.\"Genres\" ON \"Songs_Genres\".\"Genre_ID\" = \"Genres\".\"ID\" " +
                           "WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(searchInput))
            {
                query += "AND \"Songs\".\"Name\" ILIKE '%' || @SearchInput || '%' ";
            }

            if (!string.IsNullOrEmpty(selectedAuthor))
            {
                query += "AND \"Authors\".\"Nickname\" = @SelectedAuthor ";
            }

            if (!string.IsNullOrEmpty(selectedGenre))
            {
                query += "AND \"Genres\".\"Name\" = @SelectedGenre ";
            }

            query += "ORDER BY \"Songs\".\"ID\"";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchInput", searchInput);
                        if (!string.IsNullOrEmpty(selectedAuthor))
                        {
                            command.Parameters.AddWithValue("@SelectedAuthor", selectedAuthor);
                        }
                        if (!string.IsNullOrEmpty(selectedGenre))
                        {
                            command.Parameters.AddWithValue("@SelectedGenre", selectedGenre);
                        }

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            Songs = new ObservableCollection<Song>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                List<string> genreslist = GetGenresForSong(Convert.ToInt32(row["ID"]));
                                string genres = string.Join(", ", genreslist);

                                Songs.Add(new Song
                                {
                                    Name = row["Name"].ToString(),
                                    AuthorName = row["AuthorNickname"].ToString(),
                                    Duration = row["Duration"].ToString(),
                                    Listened_Num = Convert.ToInt32(row["Listened_Num"]),
                                    Picture = new BitmapImage(new Uri(row["Picture"].ToString(), UriKind.RelativeOrAbsolute)),
                                    Genres = genres
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    public class Song
    {
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public string Duration { get; set; }
        public int Listened_Num { get; set; }
        public BitmapImage Picture { get; set; }
        public string Genres { get; set; }
    }

    public class SongAuthor
    {
        public string Name { get; set; }
    }
}
