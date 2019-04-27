using MaterialDesign2.Controls;
using SomeExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Localization = Helper.Localization;

namespace VollDanebenGUI
{
    public partial class MainWindow : MaterialWindow
    {
        public double cost;
        public Thread luckyNumbersThread;

        public ChoicesWindow choicesWindow;
        public SettingsWindow settings;
        
        public MainWindow()
        {
            InitializeComponent();

            Localization.Set(Properties.Resources.German);

            choicesWindow = new ChoicesWindow(this);
            settings = new SettingsWindow(this);

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            Revenue.Text = string.Format("?", Localization.Get("VollDaneben.Currency"));
            Expenditure.Text = string.Format("?", Localization.Get("VollDaneben.Currency"));
            Total.Text = string.Format("?", Localization.Get("VollDaneben.Currency"));

            luckyNumbersText.Text = Localization.Get("VollDaneben.LuckyNumbers").Replace("\\n", Environment.NewLine);
            choicesText.Text = Localization.Get("VollDaneben.Choices");

            labelRevenue.Text = Localization.Get("VollDaneben.Revenue");
            labelExpenditure.Text = Localization.Get("VollDaneben.Expenditure");
            labelTotal.Text = Localization.Get("VollDaneben.Total");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            
            Application.Current.Shutdown();
        }

        private void ClickChoices(object sender, RoutedEventArgs e)
        {
            choicesWindow.Show();
        }

        private void ClickSettings(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private int[] choices, luckyNumbers;

        private void ClickStart(object sender, RoutedEventArgs e)
        {
            luckyNumbersThread?.Abort();

            int availablePoints = settings.AvailablePoints;
            int passes = settings.Passes;
            int start = settings.Start;
            int stride = settings.Stride;

            int choiceCount = settings.ChoiceCount;
            int min = settings.Min;
            int max = settings.Max;

            Task.Run(() =>
            {
                luckyNumbersThread = Thread.CurrentThread;

                Random random = new Random();
                choices = choicesWindow.choices.Count == 0 ? Enumerable.Repeat(0, choiceCount).Select(x => random.Next(min, max)).ToArray() : choicesWindow.choices.Select(x => (int)double.Parse(x.Value)).ToArray();
                luckyNumbers = new int[0];
                Dispatcher.Invoke(Render);

                luckyNumbers = VollDaneben.GeneralSolver(choices, availablePoints, passes, start, stride);
                cost = VollDaneben.Cost(choices, luckyNumbers);
                Dispatcher.Invoke(Render);
            });
        }
        
        private void Render()
        {
            int width = settings.Max - settings.Min;
            
            choicesTable.ItemsSource = choices.OrderBy(x => x);
            luckyNumbersTable.ItemsSource = luckyNumbers.OrderBy(x => x);
            
            Revenue.Text = string.Format((choices.Length * 25).ToString(), Localization.Get("VollDaneben.Currency"));
            Expenditure.Text = string.Format((luckyNumbers.Length == 0 ? "?" : (-cost).ToString()), Localization.Get("VollDaneben.Currency"));
            Total.Text = string.Format((luckyNumbers.Length == 0 ? "?" : (choices.Length * 25 - cost).ToString()), Localization.Get("VollDaneben.Currency"));
        }
    }
}
