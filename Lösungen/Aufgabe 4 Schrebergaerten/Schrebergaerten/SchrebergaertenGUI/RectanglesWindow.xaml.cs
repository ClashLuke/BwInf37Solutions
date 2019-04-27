using MaterialDesign2.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Localization = Helper.Localization;

namespace SchrebergaertenGUI
{
    public partial class RectanglesWindow : MaterialWindow
    {
        public MainWindow mainWindow;
        public ObservableCollection<SettingsItem> rectAttempts;

        public RectanglesWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            RectAttempts.ItemsSource = rectAttempts = new ObservableCollection<SettingsItem>();

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            Title = Localization.Get("Schrebergaerten.Rectangles.Title");
            rectanglesText.Text = Localization.Get("Schrebergaerten.Rectangles.Rectangles");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MaterialTextBoxFilled materialTextBoxFilled)
            {
                if (materialTextBoxFilled.Text == "")
                {
                    //IEditableCollectionView items = Resistors.Items;
                    rectAttempts.RemoveAt(rectAttempts.ToList().FindIndex(x => x.X == "" || x.Y == ""));
                }
                else
                {
                    if (double.TryParse(materialTextBoxFilled.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        materialTextBoxFilled.Text = value.ToString("0.0########", CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            rectAttempts.Add(new SettingsItem { X = "0.0", Y = "0.0" });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();      
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = Localization.Get("Schrebergaerten.Rectangles.FileSelector")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName)) {
                    Regex regex = new Regex("([0-9]+)[^0-9]*([0-9]+)");
                    RectAttempts.ItemsSource = rectAttempts = new ObservableCollection<SettingsItem>(
                        regex
                        .Matches(File.ReadAllText(openFileDialog.FileName))
                        .OfType<Match>()
                        .Select(x => new SettingsItem { X = x.Groups[1].Value, Y = x.Groups[2].Value })
                        .ToList());
                }
            }
        }
    }

    public class SettingsItem
    {
        public string X { get; set; }
        public string Y { get; set; }
    }
}
