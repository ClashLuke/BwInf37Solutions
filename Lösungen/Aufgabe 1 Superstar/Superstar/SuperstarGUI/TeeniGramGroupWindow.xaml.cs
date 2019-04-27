using MaterialDesign2;
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
using System.Windows.Shapes;
using Helper;
using Localization = Helper.Localization;
using MaterialDesign2.Controls;
using System.ComponentModel;
using SuperstarAPI;

namespace SuperstarGUI
{
    public partial class TeeniGramGroupWindow : MaterialWindow
    {
        public MainWindow mainWindow;

        public TeeniGramGroupWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public void Refresh()
        {
            if (mainWindow.Group != null)
            {
                followersCache = new Dictionary<User, IEnumerable<string>>();

                noGroup.Visibility = Visibility.Collapsed;

                names.ItemsSource = mainWindow.Group.users.Select(x => x.name);
                usersLabel.Text = string.Format(Localization.Get("Superstar.Users"), mainWindow.Group.users.Count());

                ClickUser(null, null);
            }
            else
            {
                noGroup.Visibility = Visibility.Visible;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ClickLoad(object sender, RoutedEventArgs e) => mainWindow.ClickLoadGroup(sender, e);
        private void ClickGenerate(object sender, RoutedEventArgs e) => mainWindow.ClickGenerate(sender, e);

        private Dictionary<User, IEnumerable<string>> followersCache;

        private void ClickUser(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                MaterialButton materialButton = (MaterialButton)sender;
                string name = (string)materialButton.Tag;
                User user = mainWindow.Group.users.Find(x => x.name == name);

                userName.Text = name;

                var follows = user.following.Select(x => x.name);
                followsNames.ItemsSource = follows;
                followsLabel.Text = string.Format(Localization.Get("Superstar.FollowsLabel"), follows.Count());

                IEnumerable<string> followers;
                if (!followersCache.ContainsKey(user)) followersCache[user] = mainWindow.Group.users.Where(x => x.following.Contains(user)).Select(x => x.name);
                followers = followersCache[user];
                followerNames.ItemsSource = followers;
                followersLabel.Text = string.Format(Localization.Get("Superstar.FollowersLabel"), followers.Count());
            }
            else
            {
                userName.Text = "";

                string[] values = new string[0];
                followsNames.ItemsSource = values;
                followsLabel.Text = string.Format(Localization.Get("Superstar.FollowsLabel"), 0);
                
                followerNames.ItemsSource = values;
                followersLabel.Text = string.Format(Localization.Get("Superstar.FollowersLabel"), 0);
            }
        }
    }
}
