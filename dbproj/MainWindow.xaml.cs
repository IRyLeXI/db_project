using System;
using System.Collections.Generic;
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

namespace dbproj
{
    public partial class MainWindow : Window
    {
        public enum View
        {
            Home,
            Songs,
            Playlists,
            Authors,
            Albums
        }

        public MainWindow()
        {
            InitializeComponent();
            NavigateTo(View.Home);
        }

        public void NavigateTo(View view)
        {
            UserControl content = null;
            switch (view)
            {
                case View.Home:
                    content = new HomeControl();
                    break;
                case View.Songs:
                    content = new SongsControl();
                    break;
                case View.Playlists:
                    content = new PlaylistsControl();
                    break;
                case View.Authors:
                    content = new AuthorsControl();
                    break;
                case View.Albums:
                    content = new AlbumsControl();
                    break;
            }
            Content = content;
        }
    }
}