using Helper;
using MaterialDesign2.Controls;
using SuperstarAPI;
using System.ComponentModel;

namespace SuperstarGUI
{
    public partial class LogicLog : MaterialWindow
    {
        public MainWindow mainWindow;
        public string text = "";
        public bool needsUpdate = false;
        public SuperstarOutput superstarOutput;

        public LogicLog(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            superstarOutput = new SuperstarOutput
            {
                exclusion = (user, propability) => WriteLine(string.Format(Localization.Get("Superstar.Log.Exclusion"), user, propability)),
                increasedPropability = (user, propability) => WriteLine(string.Format(Localization.Get("Superstar.Log.RaisedPropability"), user, propability)),

                failed = (user, bill) => WriteLine(string.Format(Localization.Get("Superstar.Log.Failed"), user, bill)),
                found = (user, bill) => WriteLine(string.Format(Localization.Get("Superstar.Log.Found"), user, bill)),

                investigate = (user, propability) => WriteLine(string.Format(Localization.Get("Superstar.Log.Investigate"), user, propability)),

                newline = () => WriteLine(""),
                following = (a, b, following, bill) =>
                {
                    string followingString = following ? Localization.Get("Superstar.Log.Follows.True") : Localization.Get("Superstar.Log.Follows.False");

                    WriteLine(
                        string.Format(
                            Localization.Get("Superstar.Log.Follows.Left"),
                            a, b, followingString, bill
                        )
                        .PadRight(int.Parse(Localization.Get("Superstar.Log.Follows.Padding"))) +
                        string.Format(
                            Localization.Get("Superstar.Log.Follows.Right"),
                            a, b, followingString, bill
                        )
                    );
                }
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void WriteLine(string line)
        {
            text += line + "\n";
            Refresh();
        }

        public void Clear()
        {
            text = "";
            Refresh();
        }

        public void Refresh()
        {
            if (!needsUpdate)
            {
                needsUpdate = true;
                Dispatcher.Invoke(() =>
                {
                    textBox.Text = text;
                    needsUpdate = false;
                });
            }
        }
    }
}
