using System;
using System.Drawing;
using System.Windows.Forms;
using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Views
{
    public partial class MainWindow : Form
    {
        public static Control CurrentControl { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            new CsvParser();

            OpenMapWindowButton_Click(null, null);
            this.BackColor = (Color)Settings.Settings.PanelBackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Colours.SetColourScheme(this.Controls);
        }

        private void OpenMapWindowButton_Click(object sender, EventArgs e)
        {
            if (CurrentControl != null)
                MainWindowHostPanel.Controls.Remove(CurrentControl);

            CurrentControl = new DungWindow();
            MainWindowHostPanel.Controls.Add(CurrentControl);
        }

        private void OpenTexturesWindowButton_Click(object sender, EventArgs e)
        {
            if(CurrentControl != null)
                MainWindowHostPanel.Controls.Remove(CurrentControl);

            CurrentControl = new TextureWindow();
            MainWindowHostPanel.Controls.Add(CurrentControl);
        }       
    }
}
