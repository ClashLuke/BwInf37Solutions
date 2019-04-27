using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Localization = Helper.Localization;
using MaterialDesign2;
using MaterialDesign2.Controls;
using TwistAPI;

namespace TwistGUI
{
    public partial class MainWindow : MaterialWindow
    {
        public string path = null;
        public LangCook detwistingLanguage = LangCook.BwInfDefaultGerman;

        public MainWindow()
        {
            InitializeComponent();
            Localization.Set(Properties.Resources.German);
            Loaded += (s, e) => Dispatcher.Invoke(Localize);
        }

        public void Localize()
        {
            detwist.GetChildOfType<TextBlock>().Text = Localization.Get("Twist.DetwistButton");
            twist.GetChildOfType<TextBlock>().Text = Localization.Get("Twist.TwistButton");
            settings.GetChildOfType<TextBlock>().Text = Localization.Get("Twist.Settings");
        }

        private void ClickDeTwist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".txt",
                Title = Localization.Get("Twist.FileSelector.Long")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string file = openFileDialog.FileName;

                if (!File.Exists(file))
                {
                    MessageBox.Show(Localization.Get("Twist.FileSelector.Invalid"));
                }
                else
                {
                    path = file;
                    Task.Run(() =>
                    {
                        string output = HandleProgress(x =>
                            sender == detwist ?
                                Twister.DeTwist(File.ReadAllText(path), detwistingLanguage, x)
                                : Twister.Twist(File.ReadAllText(path), x),
                            sender == detwist ? 100 : int.MaxValue);

                        Dispatcher.Invoke(() =>
                        {
                            progressBar.Value = 0;

                            SaveFileDialog saveFileDialog = new SaveFileDialog()
                            {
                                AddExtension = true,
                                DefaultExt = ".txt",
                                OverwritePrompt = true,
                                Title = Localization.Get("Twist.Finished.SaveResults")
                            };

                            if (saveFileDialog.ShowDialog() == true)
                            {
                                File.WriteAllText(saveFileDialog.FileName, output);
                            }
                        });
                    });
                }
            }
        }

        private T HandleProgress<T>(Func<Action<double>, T> func, int n = 100)
        {
            int counter = 0;
            return func(progress =>
            {
                if (progress == 1 || counter++ % n == 0)
                {
                    Dispatcher.Invoke(() => progressBar.Value = progress * progressBar.Maximum);
                }
            });
        }

        private void ClickSelectLanguage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".langcook",
                Title = Localization.Get("Twist.Langcook.Open"),
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string cook = openFileDialog.FileNames.ToList().Find(x => System.IO.Path.GetExtension(x) == ".langcook");

                if (cook != null)
                {
                    detwistingLanguage = new LangCook(cook);
                }
                else
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog()
                    {
                        AddExtension = true,
                        DefaultExt = ".langcook",
                        OverwritePrompt = true,
                        Title = Localization.Get("Twist.Langcook.Save")
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        detwistingLanguage = new LangCook(saveFileDialog.FileName, true, openFileDialog.FileNames);
                    }
                }
            }
        }
    }
}
