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

namespace VollDanebenGUI
{
    public partial class ChoicesWindow : MaterialWindow
    {
        public MainWindow mainWindow;
        public ObservableCollection<SettingsItem> choices;

        public ChoicesWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Choices.ItemsSource = choices = new ObservableCollection<SettingsItem>();

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        private void Localize()
        {
            Title = Localization.Get("VollDaneben.Choices.Title");
            choicesText.Text = Localization.Get("VollDaneben.Choices.Choices");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MaterialTextBoxFilled materialTextBoxFilled)
            {
                if (materialTextBoxFilled.Text == "")
                {
                    choices.RemoveAt(choices.ToList().FindIndex(x => x.Value == ""));
                }
                else
                {
                    if (int.TryParse(materialTextBoxFilled.Text, out int value))
                    {
                        materialTextBoxFilled.Text = value.ToString("0", CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            choices.Add(new SettingsItem { Value = "0" });
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
                Title = Localization.Get("VollDaneben.Choices.SelectFile")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName)) {
                    Regex regex = new Regex("[0-9.]+");
                    Choices.ItemsSource = choices = new ObservableCollection<SettingsItem>(
                        regex
                        .Matches(File.ReadAllText(openFileDialog.FileName))
                        .OfType<Match>()
                        .Select(x => new SettingsItem { Value = x.Value })
                        .ToList());
                }
            }
        }
    }

    public class SettingsItem
    {
        public string Value { get; set; }
    }
}
