using MaterialDesign2.Controls;
using SomeExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using Localization = Helper.Localization;

namespace VollDanebenGUI
{
    public partial class SettingsWindow : MaterialWindow
    {
        public string PassesText
        {
            get { return (string)GetValue(PassesTextProperty); }
            set { SetValue(PassesTextProperty, value); }
        }
        public static readonly DependencyProperty PassesTextProperty =
            DependencyProperty.Register("PassesText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int Passes
        {
            set => PassesText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(PassesText, out int value) ? value : 0;
        }
        public string StartText
        {
            get { return (string)GetValue(StartTextProperty); }
            set { SetValue(StartTextProperty, value); }
        }
        public static readonly DependencyProperty StartTextProperty =
            DependencyProperty.Register("StartText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int Start
        {
            set => StartText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(StartText, out int value) ? value : 0;
        }
        public string StrideText
        {
            get { return (string)GetValue(StrideTextProperty); }
            set { SetValue(StrideTextProperty, value); }
        }
        public static readonly DependencyProperty StrideTextProperty =
            DependencyProperty.Register("StrideText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int Stride
        {
            set => StrideText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(StrideText, out int value) ? value : 0;
        }
        public string AvailablePointsText
        {
            get { return (string)GetValue(AvailablePointsTextProperty); }
            set { SetValue(AvailablePointsTextProperty, value); }
        }
        public static readonly DependencyProperty AvailablePointsTextProperty =
            DependencyProperty.Register("AvailablePointsText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int AvailablePoints
        {
            set => AvailablePointsText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(AvailablePointsText, out int value) ? value : 0;
        }
        public string MinText
        {
            get { return (string)GetValue(MinTextProperty); }
            set { SetValue(MinTextProperty, value); }
        }
        public static readonly DependencyProperty MinTextProperty =
            DependencyProperty.Register("MinText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int Min
        {
            set => MinText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(MinText, out int value) ? value : 0;
        }
        public string MaxText
        {
            get { return (string)GetValue(MaxTextProperty); }
            set { SetValue(MaxTextProperty, value); }
        }
        public static readonly DependencyProperty MaxTextProperty =
            DependencyProperty.Register("MaxText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int Max
        {
            set => MaxText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(MaxText, out int value) ? value : 0;
        }
        public string ChoiceCountText
        {
            get { return (string)GetValue(ChoiceCountTextProperty); }
            set { SetValue(ChoiceCountTextProperty, value); }
        }
        public static readonly DependencyProperty ChoiceCountTextProperty =
            DependencyProperty.Register("ChoiceCountText", typeof(string), typeof(SettingsWindow), new PropertyMetadata("2"));

        public int ChoiceCount
        {
            set => ChoiceCountText = value.ToString("0", CultureInfo.InvariantCulture);
            get => int.TryParse(ChoiceCountText, out int value) ? value : 0;
        }

        public MainWindow mainWindow;
        
        public SettingsWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            PassesText = "50";
            StartText = "100";
            StrideText = "1";
            AvailablePointsText = "10";
            MinText = "0";
            MaxText = "1000";
            ChoiceCountText = "1000";

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void Localize()
        {
            Title = Localization.Get("VollDaneben.Settings.Title");

            solver.Text = Localization.Get("VollDaneben.Settings.Solver");

            start.Label = Localization.Get("VollDaneben.Settings.Solver.Start");
            start.ToolTip = Localization.Get("VollDaneben.Settings.Solver.Start.Tooltip");
            quality.Label = Localization.Get("VollDaneben.Settings.Solver.Passes");
            quality.ToolTip = Localization.Get("VollDaneben.Settings.Solver.Passes.Tooltip");
            stride.Label = Localization.Get("VollDaneben.Settings.Solver.Stride");
            stride.ToolTip = Localization.Get("VollDaneben.Settings.Solver.Stride.Tooltip");
            availablePoints.Label = Localization.Get("VollDaneben.Settings.Solver.LuckyNumbers");
            availablePoints.ToolTip = Localization.Get("VollDaneben.Settings.Solver.LuckyNumbers.Tooltip");


            generator.Text = Localization.Get("VollDaneben.Settings.Generator");

            min.Label = Localization.Get("VollDaneben.Settings.Generator.Minimum");
            min.ToolTip = Localization.Get("VollDaneben.Settings.Generator.Minimum.Tooltip");
            max.Label = Localization.Get("VollDaneben.Settings.Generator.Maximum");
            max.ToolTip = Localization.Get("VollDaneben.Settings.Generator.Maximum.Tooltip");
            choiceCount.Label = Localization.Get("VollDaneben.Settings.Generator.Participants");
            choiceCount.ToolTip = Localization.Get("VollDaneben.Settings.Generator.Participants.Tooltip");
        }
    }
}
