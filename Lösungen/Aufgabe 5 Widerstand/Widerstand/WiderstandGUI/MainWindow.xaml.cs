using MaterialDesign2;
using MaterialDesign2.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WiderstandAPI;
using WiderstandAPI.Resistors;
using Color = System.Drawing.Color;
using Localization = Helper.Localization;

namespace WiderstandGUI
{
    public partial class MainWindow : MaterialWindow
    {
        public ResistorsWindow settings;
        public Thread buildResistorThread;

        public int index;
        public IResistor CurrentResistor { get => resistors[index]; set => index = resistors.IndexOf(value); }
        public List<IResistor> resistors;
        public Dictionary<IResistor, ImageSource> imageCache;

        public Func<double, ResistorStyle> style;

        public double target;

        public MainWindow()
        {
            InitializeComponent();

            Localization.Set(Properties.Resources.German);

            settings = new ResistorsWindow(this);

            resistors = new List<IResistor>();
            imageCache = new Dictionary<IResistor, ImageSource>();
            index = 0;

            SolidColorBrush HICS = Application.Current.Resources["HighImportanceColorShade"] as SolidColorBrush;
            style = (highRes) => new ResistorStyle
            {
                BackColor = Color.FromArgb(Extensions.ARGB(255, 255, 255, 255)),
                TextColor = Color.FromArgb(Extensions.ARGB(255, 255, 255, 255)),
                WireColor = Color.FromArgb(Extensions.ARGB(255, 0, 0, 0)),
                ResistorColor = Color.FromArgb(Extensions.ARGB(HICS.Color.A, HICS.Color.R, HICS.Color.G, HICS.Color.B)),
                Font = "Roboto",
                ResistorHeight = (int)(30 * highRes),
                ResistorWidth = (int)(90 * highRes),
                WireLength = (int)(10 * highRes),
                WireThickness = (int)(1 * highRes),
            };

            UpdateCounter();
            UpdateImage();

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            resistanceText.Text = Localization.Get("Widerstand.Resistance");
            targetText.Text = Localization.Get("Widerstand.Target");
            differenceText.Text = Localization.Get("Widerstand.Difference");

            confirmButton.Text = Localization.Get("Widerstand.Run.Confirm");
            cancelButton.Text = Localization.Get("Widerstand.Run.Cancel");

            kNumberInput.Label = Localization.Get("Widerstand.Run.Count");
            kNumberInput.ToolTip = Localization.Get("Widerstand.Run.Count.Tooltip");
            targetNumberInput.Label = Localization.Get("Widerstand.Run.Target");
            targetNumberInput.ToolTip = Localization.Get("Widerstand.Run.Target.Tooltip");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void UpdateCounter()
        {
            counter.Text = $"{(resistors.Count == 0 ? 0 : index + 1)}/{resistors.Count}";
            indexScroller.Maximum = Math.Max(0, resistors.Count-1);
            indexScroller.Value = index;
        }

        private void UpdateImage()
        {
            if (resistors.Count == 0)
            {
                image.Visibility = Visibility.Hidden;

                ResistanceResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);
                TargetResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);
                DifferenceResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);

                ResistanceResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);
                TargetResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);
                DifferenceResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), 0);
            }
            else
            {
                image.Visibility = Visibility.Visible;

                var resistor = CurrentResistor;
                double resistance = resistor.Value;

                while (!imageCache.ContainsKey(resistor)) ;
                image.Source = imageCache[resistor];

                ResistanceResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), resistance.ToString("0.000", CultureInfo.InvariantCulture));
                TargetResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), target.ToString("0.000", CultureInfo.InvariantCulture));
                DifferenceResult.Text = string.Format(Localization.Get("Widerstand.ResistanceUnit"), Math.Abs(resistance - target).ToString("0.000", CultureInfo.InvariantCulture));

                ResistanceResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), resistance.ToString(CultureInfo.InvariantCulture));
                TargetResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), target.ToString(CultureInfo.InvariantCulture));
                DifferenceResult.ToolTip = string.Format(Localization.Get("Widerstand.ResistanceUnit"), Math.Abs(resistance - target).ToString(CultureInfo.InvariantCulture));
            }
        }

        private void Index_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (resistors.Count == 0 && (index = 0) == 0) return;
            index = (int)indexScroller.Value;
            if (index < 0) index = resistors.Count - 1;
            if (index >= resistors.Count) index = 0;

            UpdateImage();
            UpdateCounter();
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (resistors.Count == 0 && (index = 0) == 0) return;
            index--;
            if (index < 0) index = resistors.Count-1;

            UpdateImage();
            UpdateCounter();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (resistors.Count == 0 && (index = 0) == 0) return;
            index++;
            if (index >= resistors.Count) index = 0;

            UpdateImage();
            UpdateCounter();
        }
        
        private void Solve(double target, int k, List<double> allResistors)
        {
            resistors = new List<IResistor>();
            imageCache = new Dictionary<IResistor, ImageSource>();
            index = 0;

            this.target = target;

            UpdateImage();
            UpdateCounter();

            buildResistorThread?.Abort();
            Task.Run(() =>
            {
                List<double> clone = allResistors.ToList();
                buildResistorThread = Thread.CurrentThread;
                return ResistorBuilder.BuildResistor(target, k, clone, ResistorBuilderCallback(target));
            });
        }

        private Action<List<IResistor>, IResistor> ResistorBuilderCallback(double targetResistance)
        {
            return (all, resistorOriginal) =>
            {
                if (resistors.FindIndex(x => x == resistorOriginal) != -1) return;

                IResistor resistor = (IResistor)resistorOriginal.Clone();
                Dispatcher.Invoke(() =>
                {
                    double diff = Math.Abs(resistor.Value - targetResistance);
                    int insertionIndex = resistors.FindIndex(x => Math.Abs(x.Value - targetResistance) > diff) - 0;
                    if (insertionIndex < 0) insertionIndex = 0;
                    resistors.Insert(insertionIndex, resistor);

                    Task.Run(() => imageCache[resistor] = resistor.Draw(style(1)).ToWPF());

                    if (index >= insertionIndex)
                    {
                        if (resistors.Count > 1) index++;
                        UpdateImage();
                    }

                    UpdateCounter();
                });
            };
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Opacity = 0.5;
            NumberInputPanePopup.IsOpen = true;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(kNumberInput.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out int k)
             && double.TryParse(targetNumberInput.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double target)) {
                Solve(target, k, settings.resistors.Select(x => double.Parse(x.Value, CultureInfo.InvariantCulture)).ToList());
            }

            Hide_Pane();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Hide_Pane();
        }

        private void Hide_Pane()
        {
            mainGrid.Opacity = 1;
            NumberInputPanePopup.IsOpen = false;
        }
    }
}
