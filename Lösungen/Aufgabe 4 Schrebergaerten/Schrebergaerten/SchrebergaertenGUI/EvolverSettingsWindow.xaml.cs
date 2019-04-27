using MaterialDesign2.Controls;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using Localization = Helper.Localization;

namespace SchrebergaertenGUI
{
    public partial class EvolverSettingsWindow : MaterialWindow
    {
        public string GendersText
        {
            get { return (string)GetValue(GendersTextProperty); }
            set { SetValue(GendersTextProperty, value); }
        }
        public static readonly DependencyProperty GendersTextProperty =
            DependencyProperty.Register("GendersText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("2"));

        public int Genders
        {
            set => GendersText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(GendersText, out int value) ? value : 0;
        }
        public string CrossoversText
        {
            get { return (string)GetValue(CrossoversTextProperty); }
            set { SetValue(CrossoversTextProperty, value); }
        }
        public static readonly DependencyProperty CrossoversTextProperty =
            DependencyProperty.Register("CrossoversText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("5"));

        public int Crossovers
        {
            set => CrossoversText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(CrossoversText, out int value) ? value : 0;
        }
        public string MutatabilityText
        {
            get { return (string)GetValue(MutatabilityTextProperty); }
            set { SetValue(MutatabilityTextProperty, value); }
        }
        public static readonly DependencyProperty MutatabilityTextProperty =
            DependencyProperty.Register("MutatabilityText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.005"));

        public double Mutatability
        {
            set => MutatabilityText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(MutatabilityText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string SurvivalRateText
        {
            get { return (string)GetValue(SurvivalRateTextProperty); }
            set { SetValue(SurvivalRateTextProperty, value); }
        }
        public static readonly DependencyProperty SurvivalRateTextProperty =
            DependencyProperty.Register("SurvivalRateText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.5"));

        public double SurvivalRate
        {
            set => SurvivalRateText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(SurvivalRateText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string FailureImmunityText
        {
            get { return (string)GetValue(FailureImmunityTextProperty); }
            set { SetValue(FailureImmunityTextProperty, value); }
        }
        public static readonly DependencyProperty FailureImmunityTextProperty =
            DependencyProperty.Register("FailureImmunityText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.5"));

        public double FailureImmunity
        {
            set => FailureImmunityText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(FailureImmunityText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string SuddenDeathText
        {
            get { return (string)GetValue(SuddenDeathTextProperty); }
            set { SetValue(SuddenDeathTextProperty, value); }
        }
        public static readonly DependencyProperty SuddenDeathTextProperty =
            DependencyProperty.Register("SuddenDeathText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.1"));

        public double SuddenDeath
        {
            set => SuddenDeathText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(SuddenDeathText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }

        public string EvolverGendersText
        {
            get { return (string)GetValue(EvolverGendersTextProperty); }
            set { SetValue(EvolverGendersTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverGendersTextProperty =
            DependencyProperty.Register("EvolverGendersText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("2"));

        public int EvolverGenders
        {
            set => EvolverGendersText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(EvolverGendersText, out int value) ? value : 0;
        }
        public string EvolverCrossoversText
        {
            get { return (string)GetValue(EvolverCrossoversTextProperty); }
            set { SetValue(EvolverCrossoversTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverCrossoversTextProperty =
            DependencyProperty.Register("EvolverCrossoversText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("10"));

        public int EvolverCrossovers
        {
            set => EvolverCrossoversText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(EvolverCrossoversText, out int value) ? value : 0;
        }
        public string EvolverMutatabilityText
        {
            get { return (string)GetValue(EvolverMutatabilityTextProperty); }
            set { SetValue(EvolverMutatabilityTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverMutatabilityTextProperty =
            DependencyProperty.Register("EvolverMutatabilityText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.01"));

        public double EvolverMutatability
        {
            set => EvolverMutatabilityText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(EvolverMutatabilityText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string EvolverSurvivalRateText
        {
            get { return (string)GetValue(EvolverSurvivalRateTextProperty); }
            set { SetValue(EvolverSurvivalRateTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverSurvivalRateTextProperty =
            DependencyProperty.Register("EvolverSurvivalRateText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("-0.5"));

        public double EvolverSurvivalRate
        {
            set => EvolverSurvivalRateText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(EvolverSurvivalRateText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string EvolverFailureImmunityText
        {
            get { return (string)GetValue(EvolverFailureImmunityTextProperty); }
            set { SetValue(EvolverFailureImmunityTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverFailureImmunityTextProperty =
            DependencyProperty.Register("EvolverFailureImmunityText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.1"));

        public double EvolverFailureImmunity
        {
            set => EvolverFailureImmunityText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(EvolverFailureImmunityText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string EvolverSuddenDeathText
        {
            get { return (string)GetValue(EvolverSuddenDeathTextProperty); }
            set { SetValue(EvolverSuddenDeathTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverSuddenDeathTextProperty =
            DependencyProperty.Register("EvolverSuddenDeathText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.0"));

        public double EvolverSuddenDeath
        {
            set => EvolverSuddenDeathText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(EvolverSuddenDeathText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }

        public string DensityExponentText
        {
            get { return (string)GetValue(DensityExponentTextProperty); }
            set { SetValue(DensityExponentTextProperty, value); }
        }
        public static readonly DependencyProperty DensityExponentTextProperty =
            DependencyProperty.Register("DensityExponentText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("1.1"));

        public double DensityExponent
        {
            set => DensityExponentText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(DensityExponentText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string AreaExponentText
        {
            get { return (string)GetValue(AreaExponentTextProperty); }
            set { SetValue(AreaExponentTextProperty, value); }
        }
        public static readonly DependencyProperty AreaExponentTextProperty =
            DependencyProperty.Register("AreaExponentText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0.5"));

        public double AreaExponent
        {
            set => AreaExponentText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(AreaExponentText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string CenterDistanceExponentText
        {
            get { return (string)GetValue(CenterDistanceExponentTextProperty); }
            set { SetValue(CenterDistanceExponentTextProperty, value); }
        }
        public static readonly DependencyProperty CenterDistanceExponentTextProperty =
            DependencyProperty.Register("CenterDistanceExponentText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0"));

        public double CenterDistanceExponent
        {
            set => CenterDistanceExponentText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(CenterDistanceExponentText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }
        public string WeightedCenterDistanceExponentText
        {
            get { return (string)GetValue(WeightedCenterDistanceExponentTextProperty); }
            set { SetValue(WeightedCenterDistanceExponentTextProperty, value); }
        }
        public static readonly DependencyProperty WeightedCenterDistanceExponentTextProperty =
            DependencyProperty.Register("WeightedCenterDistanceExponentText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("0"));

        public double WeightedCenterDistanceExponent
        {
            set => WeightedCenterDistanceExponentText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => double.TryParse(WeightedCenterDistanceExponentText, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : 0;
        }

        public string BatchSizeText
        {
            get { return (string)GetValue(BatchSizeTextProperty); }
            set { SetValue(BatchSizeTextProperty, value); }
        }
        public static readonly DependencyProperty BatchSizeTextProperty =
            DependencyProperty.Register("BatchSizeText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("48"));

        public int BatchSize
        {
            set => BatchSizeText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(BatchSizeText, out int value) ? value : 0;
        }
        public string EvolverBatchSizeText
        {
            get { return (string)GetValue(EvolverBatchSizeTextProperty); }
            set { SetValue(EvolverBatchSizeTextProperty, value); }
        }
        public static readonly DependencyProperty EvolverBatchSizeTextProperty =
            DependencyProperty.Register("EvolverBatchSizeText", typeof(string), typeof(EvolverSettingsWindow), new PropertyMetadata("10"));

        public int EvolverBatchSize
        {
            set => EvolverBatchSizeText = value.ToString("0.0###########", CultureInfo.InvariantCulture);
            get => int.TryParse(EvolverBatchSizeText, out int value) ? value : 0;
        }
        
        public bool AllowRotation => allowRotation.IsToggledOn;

        public MainWindow mainWindow;

        public EvolverSettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            local.Text = Localization.Get("Schrebergaerten.Evolver.Local");
            local.ToolTip = Localization.Get("Schrebergaerten.Evolver.Local.Tooltip");
            global.Text = Localization.Get("Schrebergaerten.Evolver.Global");
            global.ToolTip = Localization.Get("Schrebergaerten.Evolver.Global.Tooltip");
            fitnessExponents.Text = Localization.Get("Schrebergaerten.Evolver.FitnessExponents");
            fitnessExponents.ToolTip = Localization.Get("Schrebergaerten.Evolver.FitnessExponents.Tooltip");
            
            allowRotation.Text = Localization.Get("Schrebergaerten.Evolver.AllowRotation");
            allowRotation.ToolTip = Localization.Get("Schrebergaerten.Evolver.AllowRotation.Tooltip");


            textBatchSize.Label = Localization.Get("Schrebergaerten.Evolver.BatchSize");
            textBatchSize.ToolTip = Localization.Get("Schrebergaerten.Evolver.BatchSize.Tooltip");

            textGenders.Label = Localization.Get("Schrebergaerten.Evolver.Genders");
            textGenders.ToolTip = Localization.Get("Schrebergaerten.Evolver.Genders.Tooltip");
            textCrossovers.Label = Localization.Get("Schrebergaerten.Evolver.Crossovers");
            textCrossovers.ToolTip = Localization.Get("Schrebergaerten.Evolver.Crossovers.Tooltip");
            textMutatability.Label = Localization.Get("Schrebergaerten.Evolver.Mutatability");
            textMutatability.ToolTip = Localization.Get("Schrebergaerten.Evolver.Mutatability.Tooltip");
            textSurvivalRate.Label = Localization.Get("Schrebergaerten.Evolver.SurvivalRate");
            textSurvivalRate.ToolTip = Localization.Get("Schrebergaerten.Evolver.SurvivalRate.Tooltip");
            textFailureImmuntiy.Label = Localization.Get("Schrebergaerten.Evolver.FailureImmuntiy");
            textFailureImmuntiy.ToolTip = Localization.Get("Schrebergaerten.Evolver.FailureImmuntiy.Tooltip");
            textSuddenDeath.Label = Localization.Get("Schrebergaerten.Evolver.SuddenDeath");
            textSuddenDeath.ToolTip = Localization.Get("Schrebergaerten.Evolver.SuddenDeath.Tooltip");


            textEvolverBatchSize.Label = Localization.Get("Schrebergaerten.Evolver.EvolverBatchSize");
            textEvolverBatchSize.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverBatchSize.Tooltip");

            textEvolverGenders.Label = Localization.Get("Schrebergaerten.Evolver.EvolverGenders");
            textEvolverGenders.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverGenders.Tooltip");
            textEvolverCrossovers.Label = Localization.Get("Schrebergaerten.Evolver.EvolverCrossovers");
            textEvolverCrossovers.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverCrossovers.Tooltip");
            textEvolverMutatability.Label = Localization.Get("Schrebergaerten.Evolver.EvolverMutatability");
            textEvolverMutatability.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverMutatability.Tooltip");
            textEvolverSurvivalRate.Label = Localization.Get("Schrebergaerten.Evolver.EvolverSurvivalRate");
            textEvolverSurvivalRate.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverSurvivalRate.Tooltip");
            textEvolverFailureImmuntiy.Label = Localization.Get("Schrebergaerten.Evolver.EvolverFailureImmuntiy");
            textEvolverFailureImmuntiy.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverFailureImmuntiy.Tooltip");
            textEvolverSuddenDeath.Label = Localization.Get("Schrebergaerten.Evolver.EvolverSuddenDeath");
            textEvolverSuddenDeath.ToolTip = Localization.Get("Schrebergaerten.Evolver.EvolverSuddenDeath.Tooltip");

            textDensity.Label = Localization.Get("Schrebergaerten.Evolver.Density");
            textDensity.ToolTip = Localization.Get("Schrebergaerten.Evolver.Density.Tooltip");
            textArea.Label = Localization.Get("Schrebergaerten.Evolver.Area");
            textArea.ToolTip = Localization.Get("Schrebergaerten.Evolver.Area.Tooltip");
            textBBCenterDistance.Label = Localization.Get("Schrebergaerten.Evolver.BBCenterDistance");
            textBBCenterDistance.ToolTip = Localization.Get("Schrebergaerten.Evolver.BBCenterDistance.Tooltip");
            textCoMDistance.Label = Localization.Get("Schrebergaerten.Evolver.CoMDistance");
            textCoMDistance.ToolTip = Localization.Get("Schrebergaerten.Evolver.CoMDistance.Tooltip");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
