using System.Drawing;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class EditWarpWindow : Form
    {
        public EditWarpWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);
        }

        public void SetCurrentWarpData(int type, int posX, int posY)
        {
            EditWarpTypeComboBox.SelectedIndex = type;
            WarpPositionXNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            WarpPositionXNumericUpDown.Value = posX;
            WarpPositionYNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            WarpPositionYNumericUpDown.Value = posY;
        }
    }
}
