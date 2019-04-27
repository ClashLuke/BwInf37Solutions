using MaterialDesign2.Controls;
using Microsoft.Win32;
using SuperstarAPI;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Localization = Helper.Localization;

namespace SuperstarGUI
{
    public partial class MainWindow : MaterialWindow
    {
        private TeeniGramGroup _group;
        public TeeniGramGroup Group
        {
            get => _group;
            set
            {
                _group = value;
                logicLog.Clear();
                teeniGramGroupWindow.Refresh();

                superstarTitleLabel.Text = string.Format(Localization.Get("Superstar.SuperstarTitle"), Localization.Get("Superstar.Superstar.Undetermined"));
                billTitleLabel.Text = string.Format(Localization.Get("Superstar.BillTitle"), 0);
                superstarLabel.Text = string.Format(Localization.Get("Superstar.Superstar"), Localization.Get("Superstar.Superstar.Undetermined"));
                billLabel.Text = string.Format(Localization.Get("Superstar.Bill"), 0);
            }
        }
        public LogicLog logicLog;
        public TeeniGramGroupWindow teeniGramGroupWindow;

        public BackgroundWorker superstarFindingWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            Localization.Set(Properties.Resources.German);

            logicLog = new LogicLog(this);
            teeniGramGroupWindow = new TeeniGramGroupWindow(this);

            Group = new TeeniGramGroup(100, logicLog.superstarOutput);
            
            superstarFindingWorker.DoWork += (s, e) => {
                Dispatcher.Invoke(logicLog.Clear);
                User user = SuperSearch.FindSuperstar(Group, out int bill, logicLog.superstarOutput);
                string superstar = user == null ? Localization.Get("Superstar.Superstar.None") : user.name;
                Dispatcher.Invoke(() =>
                {
                    superstarTitleLabel.Text = string.Format(Localization.Get("Superstar.SuperstarTitle"), superstar);
                    billTitleLabel.Text = string.Format(Localization.Get("Superstar.BillTitle"), bill);
                    superstarLabel.Text = string.Format(Localization.Get("Superstar.Superstar"), superstar);
                    billLabel.Text = string.Format(Localization.Get("Superstar.Bill"), bill);
                });
            };

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            showGroup.Text = Localization.Get("Superstar.ShowGroup");
            loadGroup.Text = Localization.Get("Superstar.LoadGroup");
            generate.Text = Localization.Get("Superstar.Generate");

            showLog.Text = Localization.Get("Superstar.ShowLog");
            findSuperstar.Text = Localization.Get("Superstar.FindSuperstar");

            superstarTitleLabel.Text = string.Format(Localization.Get("Superstar.SuperstarTitle"), Localization.Get("Superstar.Superstar.Undetermined"));
            billTitleLabel.Text = string.Format(Localization.Get("Superstar.BillTitle"), 0);
            superstarLabel.Text = string.Format(Localization.Get("Superstar.Superstar"), Localization.Get("Superstar.Superstar.Undetermined"));
            billLabel.Text = string.Format(Localization.Get("Superstar.Bill"), 0);

            confirm.Text = Localization.Get("Superstar.Confirm");
            cancel.Text = Localization.Get("Superstar.Cancel");

            numberInput.Label = Localization.Get("Superstar.Generate.GroupSize");
            numberInput.ToolTip = Localization.Get("Superstar.Generate.GroupSize.Tooltip");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            Application.Current.Shutdown();
        }

        private void ClickViewGroup(object sender, RoutedEventArgs e)
        {
            teeniGramGroupWindow.Refresh();
            teeniGramGroupWindow.Show();
        }

        private void ClickFindSuperstar(object sender, RoutedEventArgs e)
        {
            if (!superstarFindingWorker.IsBusy)
            {
                superstarFindingWorker.RunWorkerAsync();
            }
        }

        private void ClickShowLog(object sender, RoutedEventArgs e)
        {
            logicLog.Show();
        }

        public void ClickGenerate(object sender, RoutedEventArgs e)
        {
            NumberInputPanePopup.IsOpen = true;
            mainGrid.Opacity = 0.5;
        }

        public void ClickLoadGroup(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = Localization.Get("Superstar.LoadGroup.Title"),
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    string[] file = File.ReadAllLines(openFileDialog.FileName);
                    Group = new TeeniGramGroup(file, logicLog.superstarOutput);
                }
                else
                {
                    MessageBox.Show(Localization.Get("Superstar.LoadGroup.InvalidFile"));
                }
            }
        }
        
        private void NumberInputPane_Click(object sender, RoutedEventArgs e) => ClickCancel(sender, e);
        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            NumberInputPanePopup.IsOpen = false;
            mainGrid.Opacity = 1;
        }

        private void ClickConfirm(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numberInput.Text, out int value))
            {
                if (value <= 0)
                {
                    MessageBox.Show(Localization.Get("Superstar.Generate.InvalidSize"));
                }
                else
                {
                    Group = new TeeniGramGroup(value, logicLog.superstarOutput);
                }
            }
            else
            {
                MessageBox.Show(Localization.Get("Superstar.Generate.InvalidValue"));
            }

            NumberInputPanePopup.IsOpen = false;
            mainGrid.Opacity = 1;
        }
    }
}
