using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.Views
{
    public partial class MainWindow : Form
    {
        private const AnchorStyles AnchorAll = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;

        public static Control CurrentControl { get; private set; }
        public static DebugWindow DebugWin { get; private set; } 

        public MainWindow()
        {
            InitializeComponent();

            SetupClasses();

            OpenMapWindowButton_Click(null, null);
            this.BackColor = (Color)Settings.Settings.PanelBackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            ColourTheme.SetColourScheme(this.Controls);
        }

        private void SetupClasses()
        {
            new CsvParser();
            DebugWin = new DebugWindow();
        }

        private void OpenMapWindowButton_Click(object sender, EventArgs e)
        {
            if (CurrentControl != null)
                MainWindowHostPanel.Controls.Remove(CurrentControl);

            CurrentControl = new DungWindow
            {
                Anchor = AnchorAll,
                MinimumSize = new Size(1100, 660)
            };
            MainWindowHostPanel.Controls.Add(CurrentControl);
        }

        private void OpenTexturesWindowButton_Click(object sender, EventArgs e)
        {
            if (CurrentControl != null)
                MainWindowHostPanel.Controls.Remove(CurrentControl);

            CurrentControl = new TextureWindow
            {
                Anchor = AnchorAll,
                MinimumSize = new Size(1100, 660)
            };

            MainWindowHostPanel.Controls.Add(CurrentControl);
        }

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            ((IHostWindow)CurrentControl).OnWindowResizeEnded();
        }

        private void MainToolStripOpenLogWindow_Click(object sender, EventArgs e)
        {
            DebugWin.Show();
        }
    }
}
