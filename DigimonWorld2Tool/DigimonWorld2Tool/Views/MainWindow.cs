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

        public static bool EditModeEnabled { get; private set; }
        private const string EditModeEnabledText = "Disable Edit Mode";
        private const string EditModeDisabledText = "Enable Edit Mode";
        //public event EventHandler EditModeChangedEvent;
        public delegate void EditModeChangedEventHandler(bool editModeEnabled);
        public static event EditModeChangedEventHandler EditModeChanged;

        public MainWindow()
        {
            InitializeComponent();

            SetupClasses();

            OpenMapWindowButton_Click(null, null);
            this.BackColor = (Color)Settings.Settings.PanelBackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            ColourTheme.SetColourScheme(this.Controls);

            ShowValuesAsHexToolStripMenuItem.Checked = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            removeIDCapToolStripMenuItem.Checked = (bool)Properties.Settings.Default["RemoveIDCap"];
        }

        private void SetupClasses()
        {
            new CsvParser();
            DebugWin = new DebugWindow();
        }

        private void OpenMapWindowButton_Click(object sender, EventArgs e)
        {
            if (CurrentControl != null)
            {
                CurrentControl.Hide();
                MainWindowHostPanel.Controls.Remove(CurrentControl);
            }

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
            {
                CurrentControl.Hide();
                MainWindowHostPanel.Controls.Remove(CurrentControl);
            }

            CurrentControl = new TextureWindow
            {
                Anchor = AnchorAll,
                MinimumSize = new Size(1100, 660)
            };

            MainWindowHostPanel.Controls.Add(CurrentControl);
        }

        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            if (CurrentControl == null)
                return;

            ((IHostWindow)CurrentControl).OnWindowResizeEnded();
        }

        private void MainToolStripOpenLogWindow_Click(object sender, EventArgs e)
        {
            DebugWin.Show();
        }

        private void EnableEditModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditModeEnabled = !EditModeEnabled;
            EnableEditModeToolStripMenuItem.Text = EditModeEnabled ? EditModeEnabledText : EditModeDisabledText;
            EditModeChanged.Invoke(EditModeEnabled);
        }

        private void ShowValuesAsHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            Properties.Settings.Default["ShowValuesAsHex"] = menuItem.Checked;
            Settings.Settings.ValueTextFormat = menuItem.Checked ? "X2" : "D2";
        }

        private void RemoveIDCapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            Properties.Settings.Default["RemoveIDCap"] = menuItem.Checked;
            Settings.Settings.RemoveIDCap = menuItem.Checked;
        }

        private void ENEMYSETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EditENEMTSETWindow().Show();
        }
    }
}
