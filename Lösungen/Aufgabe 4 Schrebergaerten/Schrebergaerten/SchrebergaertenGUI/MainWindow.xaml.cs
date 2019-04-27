using MaterialDesign2;
using MaterialDesign2.Controls;
using SchrebergaertenAPI;
using SchrebergaertenGUI.Drawing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Localization = Helper.Localization;

namespace SchrebergaertenGUI
{
    public partial class MainWindow : MaterialWindow
    {
        public RectanglesWindow settings;
        public EvolverSettingsWindow evolverSettings;
        public Thread evolverThread;

        public int index, room = -1;
        public SpaceFiller spaceFiller;
        public List<RectAttempt> CurrentRectAttempts { get => room == -1 ? spaceFiller?.evolver.Batch : spaceFiller?.evolver.evolvers[room].batch; }
        public RectAttempt CurrentRectAttempt { get => room == -1 ? spaceFiller?.evolver.Batch[index] : spaceFiller?.evolver.evolvers[room].batch[index]; }
        public int? RectAttemptCount { get => room == -1 ? spaceFiller?.evolver.Batch.Count : spaceFiller?.evolver.evolvers[room].batch.Count; }

        public List<RectAttempt> CurrentRectAttemptsCached;
        public RectAttempt CurrentRectAttemptCached { get => CurrentRectAttemptsCached[index]; }
        public int? RectAttemptCountCached { get => CurrentRectAttemptsCached.Count; }
        
        public RectAttemptStyle style;

        public MainWindow()
        {
            InitializeComponent();

            Localization.Set(Properties.Resources.German);

            settings = new RectanglesWindow(this);
            evolverSettings = new EvolverSettingsWindow(this);
            
            CurrentRectAttemptsCached = new List<RectAttempt>();
            index = 0;

            SolidColorBrush HICS = Application.Current.Resources["HighImportanceColorShade"] as SolidColorBrush;
            style = new RectAttemptStyle
            {
                FillColor = Color.FromArgb(Extensions.ARGB(HICS.Color.A, HICS.Color.R, HICS.Color.G, HICS.Color.B)),
                StrokeColor = Color.FromArgb(Extensions.ARGB(255, 0, 0, 0)),
                StrokeThickness = 1,
            };

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

            UpdateCounter();
            UpdateImage();

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            generationText.Text = Localization.Get("Schrebergaerten.Generation");
            fitnessText.Text = Localization.Get("Schrebergaerten.Fitness");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            evolverThread?.Abort();
            Application.Current.Shutdown();
        }

        private void UpdateCounter()
        {
            counter.Text = $"{((RectAttemptCountCached ?? 0) == 0 ? 0 : index + 1)}/{(RectAttemptCountCached ?? 0)}";
            indexScroller.Maximum = Math.Max(0, (RectAttemptCountCached ?? 0) - 1);
            indexScroller.Value = index;
        }

        private void UpdateImage()
        {
            if ((RectAttemptCountCached ?? 0) == 0)
            {
                image.Visibility = Visibility.Hidden;

                GenerationResult.Text = "0";
                FitnessResult.Text = "0";

                GenerationResult.ToolTip = "0";
                FitnessResult.ToolTip = "0";
            }
            else
            {
                image.Visibility = Visibility.Visible;

                var rectAttempt = CurrentRectAttemptCached;
                
                image.Source = rectAttempt.DrawRectAttempt(style).ToWPF();

                GenerationResult.Text = spaceFiller.generation.ToString(CultureInfo.InvariantCulture);
                FitnessResult.Text = rectAttempt.Fitness.ToString("0.0", CultureInfo.InvariantCulture);
                
                GenerationResult.ToolTip = spaceFiller.generation.ToString(CultureInfo.InvariantCulture);
                FitnessResult.ToolTip = rectAttempt.Fitness.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Index_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (RectAttemptCountCached == 0 && (index = 0) == 0) return;
            index = (int)indexScroller.Value;
            if (index < 0) index = (RectAttemptCountCached ?? 0) - 1;
            if (index >= RectAttemptCountCached) index = 0;

            UpdateImage();
            UpdateCounter();
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (RectAttemptCountCached == 0 && (index = 0) == 0) return;
            index--;
            if (index < 0) index = (RectAttemptCountCached ?? 0) - 1;

            UpdateImage();
            UpdateCounter();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (RectAttemptCountCached == 0 && (index = 0) == 0) return;
            index++;
            if (index >= RectAttemptCountCached) index = 0;

            UpdateImage();
            UpdateCounter();
        }


        private void Solve()
        {
            if (settings.rectAttempts.Count == 0) return;
            
            index = 0;

            UpdateImage();
            UpdateCounter();
            
            spaceFiller = new SpaceFiller(evolverSettings.BatchSize, evolverSettings.EvolverBatchSize,
                settings.rectAttempts.Select(x => new SchrebergaertenAPI.Rect(0, 0, double.Parse(x.X), double.Parse(x.Y))).ToList(),
                RectFitnesses.Exponents(evolverSettings.DensityExponent, evolverSettings.AreaExponent, evolverSettings.CenterDistanceExponent, evolverSettings.WeightedCenterDistanceExponent),
                evolverSettings.AllowRotation)
            {
                genders = evolverSettings.Genders, //Default = 2
                crossovers = evolverSettings.Crossovers, //Default = 5
                mutatability = evolverSettings.Mutatability, //Default = 0.005f
                survivalRate = evolverSettings.SurvivalRate, //Default = 0.5f
                failureImmunity = evolverSettings.FailureImmunity, //Default = 0.5f
                suddenDeath = evolverSettings.SuddenDeath, //Default = 0.1f

                evolverGenders = evolverSettings.EvolverGenders, //Default = 2
                evolverCrossovers = evolverSettings.EvolverCrossovers, //Default = 10
                evolverMutatability = evolverSettings.EvolverMutatability, //Default = 0.01f
                evolverSurvivalRate = evolverSettings.EvolverSurvivalRate, //Default = -0.5f
                evolverFailureImmunity = evolverSettings.EvolverFailureImmunity, //Default = 0.1f
                evolverSuddenDeath = evolverSettings.EvolverSuddenDeath, //Default = 0.0f
            };

            evolverThread?.Abort();
            Task.Run(() =>
            {
                evolverThread = Thread.CurrentThread;

                while (true)
                {
                    spaceFiller.Generation();

                    if (spaceFiller.generation % 5 == 0)
                    {
                        lock (CurrentRectAttemptsCached)
                        {
                            CurrentRectAttemptsCached = CurrentRectAttempts;

                            bool running = true;
                            Dispatcher.Invoke(() =>
                            {
                                UpdateImage();
                                UpdateCounter();
                                running = false;
                            });
                            while (running) ;
                        }
                    }
                }
            });
        }
        
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void EvolverSettings_Click(object sender, RoutedEventArgs e)
        {
            evolverSettings.Show();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Solve();
        }
    }
}
