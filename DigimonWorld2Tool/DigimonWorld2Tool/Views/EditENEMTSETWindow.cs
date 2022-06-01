using System.Drawing;
using System.Windows.Forms;

using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Views
{
    public partial class EditENEMTSETWindow : Form
    {
        public EditENEMTSETWindow()
        {
            InitializeComponent();

            this.BackColor = (Color)Settings.Settings.PanelBackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            ColourTheme.SetColourScheme(this.Controls);
        }
    }
}
