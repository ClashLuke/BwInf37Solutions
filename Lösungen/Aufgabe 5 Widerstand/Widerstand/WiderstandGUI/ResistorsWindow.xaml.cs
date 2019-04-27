using MaterialDesign2.Controls;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Localization = Helper.Localization;

namespace WiderstandGUI
{
    public partial class ResistorsWindow : MaterialWindow
    {
        public MainWindow mainWindow;
        public ObservableCollection<SettingsItem> resistors;

        public ResistorsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Resistors.ItemsSource = resistors = new ObservableCollection<SettingsItem>();

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            Title = Localization.Get("Widerstand.Resistors.Title");
            resistorsText.Text = Localization.Get("Widerstand.Resistors.Resistors");

            min.Label = Localization.Get("Widerstand.Resistors.Generator.Minimum");
            min.ToolTip = Localization.Get("Widerstand.Resistors.Generator.Minimum.Tooltip");
            max.Label = Localization.Get("Widerstand.Resistors.Generator.Maximum");
            max.ToolTip = Localization.Get("Widerstand.Resistors.Generator.Maximum.Tooltip");
            count.Label = Localization.Get("Widerstand.Resistors.Generator.Count");
            count.ToolTip = Localization.Get("Widerstand.Resistors.Generator.Count.Tooltip");

            allowFractions.Text = Localization.Get("Widerstand.Resistors.Generator.AllowFraction");
            allowFractions.ToolTip = Localization.Get("Widerstand.Resistors.Generator.AllowFraction.Tooltip");

            confirm.Text = Localization.Get("Widerstand.Resistors.Generator.Confirm");
            cancel.Text = Localization.Get("Widerstand.Resistors.Generator.Cancel");
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is MaterialTextBoxFilled materialTextBoxFilled)
            {
                if (materialTextBoxFilled.Text == "")
                {
                    //IEditableCollectionView items = Resistors.Items;
                    resistors.RemoveAt(resistors.ToList().FindIndex(x => x.Value == ""));
                }
                else
                {
                    if (double.TryParse(materialTextBoxFilled.Text, out double value))
                    {
                        materialTextBoxFilled.Text = value.ToString("0.0########", CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            resistors.Add(new SettingsItem { Value = "0.0" });
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
                Title = Localization.Get("Widerstand.Resistors.FileSelector")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName)) {
                    Regex regex = new Regex("[0-9.]+");
                    Resistors.ItemsSource = resistors = new ObservableCollection<SettingsItem>(
                        regex
                        .Matches(File.ReadAllText(openFileDialog.FileName))
                        .OfType<Match>()
                        .Select(x => new SettingsItem { Value = x.Value })
                        .ToList());
                }
            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            generatePopup.IsOpen = true;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            double minValue = double.Parse(min.Text, CultureInfo.InvariantCulture);
            double maxValue = double.Parse(max.Text, CultureInfo.InvariantCulture);
            int countValue = int.Parse(count.Text, CultureInfo.InvariantCulture);
            bool fractions = allowFractions.IsToggledOn;

            Task.Run(() =>
            {
                Random random = new Random();

                Dispatcher.Invoke(() =>
                {
                    resistors.Clear();
                    Resistors.ItemsSource = resistors;
                });

                ObservableCollection<SettingsItem> newResistors = new ObservableCollection<SettingsItem>();
                if (fractions)
                {
                    for (int i = 0; i < countValue; i++)
                    {
                        var item = new SettingsItem { Value = (random.NextDouble() * (maxValue - minValue) + minValue).ToString("0.000", CultureInfo.InvariantCulture) };

                        newResistors.Add(item);

                        AddResistors(i, newResistors);
                    }
                }
                else
                {
                    for (int i = 0; i < countValue; i++)
                    {
                        var item = new SettingsItem { Value = random.Next((int)minValue, (int)maxValue).ToString() };

                        newResistors.Add(item);

                        AddResistors(i, newResistors);
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    AddResistors(0, newResistors);

                    Resistors.ItemsSource = resistors;

                    generatePopup.IsOpen = false;
                });
            });
        }

        private void AddResistors(int i, ObservableCollection<SettingsItem> newResistors)
        {
            if (i % 5 == 0)
            {
                Dispatcher.Invoke(() =>
                {
                    foreach (var elem in newResistors) { resistors.Add(elem); }

                    newResistors.Clear();
                });
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            generatePopup.IsOpen = false;
        }
    }

    public class SettingsItem
    {
        public string Value { get; set; }
    }
}
