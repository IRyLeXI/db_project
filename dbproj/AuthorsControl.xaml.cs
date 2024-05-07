using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Npgsql;

namespace dbproj
{
    public partial class AuthorsControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Database=SongsDB;Username=postgres;Password=192837465";

        public AuthorsControl()
        {
            InitializeComponent();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Authors\".\"Nickname\", \"Authors\".\"Firstname\", \"Authors\".\"Lastname\", \"Authors\".\"Picture\", \"Authors\".\"Description\", \"Authors\".\"Birthdate\", " +
                                   "COALESCE(SUM(\"Listened_Num\"), 0) AS TotalListens " +
                                   "FROM public.\"Authors\" " +
                                   "LEFT JOIN public.\"Songs\" ON \"Authors\".\"ID\" = \"Songs\".\"Author\" " +
                                   "GROUP BY \"Authors\".\"ID\"";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ObservableCollection<Author> authors = new ObservableCollection<Author>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string nickname = row["Nickname"].ToString();
                                string firstName = row["FirstName"].ToString();
                                string lastName = row["LastName"].ToString();
                                int totalListens = Convert.ToInt32(row["TotalListens"]);
                                string picturePath = row["Picture"].ToString();
                                string description = row["Description"].ToString();
                                string birthdate = Convert.ToDateTime(row["Birthdate"]).ToString("dd.MM.yyyy");

                                authors.Add(new Author { Nickname = nickname, FirstName = firstName, LastName = lastName, TotalListens = totalListens, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), Description = description, Birthdate = birthdate });
                            }

                            authorsItemsControl.ItemsSource = authors;
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
            int minListens = 0;

            if (!string.IsNullOrEmpty(minListensTextBox.Text.Trim()))
            {
                if (!int.TryParse(minListensTextBox.Text.Trim(), out minListens))
                {
                    MessageBox.Show("Please enter a valid number for minimum listens.");
                    return;
                }
            }

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Authors\".\"Nickname\", \"Authors\".\"Firstname\", \"Authors\".\"Lastname\", \"Authors\".\"Picture\", \"Authors\".\"Description\", \"Authors\".\"Birthdate\", " +
                                   "COALESCE(SUM(\"Listened_Num\"), 0) AS TotalListens " +
                                   "FROM public.\"Authors\" " +
                                   "LEFT JOIN public.\"Songs\" ON \"Authors\".\"ID\" = \"Songs\".\"Author\" " +
                                   "WHERE 1 = 1 ";

                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        query += "AND (\"Nickname\" ILIKE '%' || @SearchInput || '%') ";
                    }

                    query += "GROUP BY \"Authors\".\"ID\" " +
                             "HAVING COALESCE(SUM(\"Listened_Num\"), 0) >= @MinListens";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(searchInput))
                        {
                            command.Parameters.AddWithValue("@SearchInput", searchInput);
                        }

                        command.Parameters.AddWithValue("@MinListens", minListens);

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            ObservableCollection<Author> authors = new ObservableCollection<Author>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string nickname = row["Nickname"].ToString();
                                string firstName = row["FirstName"].ToString();
                                string lastName = row["LastName"].ToString();
                                int totalListens = Convert.ToInt32(row["TotalListens"]);
                                string picturePath = row["Picture"].ToString();
                                string description = row["Description"].ToString();
                                string birthdate = Convert.ToDateTime(row["Birthdate"]).ToString("dd.MM.yyyy");

                                authors.Add(new Author { Nickname = nickname, FirstName = firstName, LastName = lastName, TotalListens = totalListens, Picture = new BitmapImage(new Uri(picturePath, UriKind.RelativeOrAbsolute)), Description = description, Birthdate = birthdate });
                            }

                            authorsItemsControl.ItemsSource = authors;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void AuthorItem_Click(object sender, RoutedEventArgs e)
        {
            Author clickedAuthor = (sender as FrameworkElement).DataContext as Author;

            AuthorDetailsControl authorDetailsControl = new AuthorDetailsControl(clickedAuthor);

            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Content = authorDetailsControl;
            }
        }

    }

    public class Author
    {
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalListens { get; set; }
        public BitmapImage Picture { get; set; }
        public string Description { get; set; }
        public string Birthdate { get; set; }
    }
}
