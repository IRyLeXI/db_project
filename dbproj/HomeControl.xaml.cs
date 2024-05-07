using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace dbproj
{
    public partial class HomeControl : UserControl
    {
        private string connectionString = "Host=localhost;Port=5432;Database=SongsDB;Username=postgres;Password=192837465";
        private int userId = 1;

        public HomeControl()
        {
            InitializeComponent();
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT \"Nickname\", \"Firstname\", \"Lastname\", \"Birthdate\", \"Photo\" " +
                                   "FROM public.\"Users\" " +
                                   "WHERE \"ID\" = @UserId";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow row = dataTable.Rows[0];

                                UserProfile userProfile = new UserProfile();
                                userProfile.Nickname = row["Nickname"].ToString();
                                userProfile.FirstName = row["Firstname"].ToString();
                                userProfile.LastName = row["Lastname"].ToString();
                                userProfile.Birthday = Convert.ToDateTime(row["Birthdate"]).ToString("dd.MM.yyyy");
                                userProfile.ProfilePicture = row["Photo"].ToString();

                                this.DataContext = userProfile;
                            }
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


    public class UserProfile
    {
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public string ProfilePicture { get; set; }
    }
}