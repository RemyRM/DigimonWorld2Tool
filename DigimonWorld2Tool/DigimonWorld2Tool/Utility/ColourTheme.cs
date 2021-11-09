using System.Drawing;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Utility
{
    class ColourTheme
    {
        public static void SetColourScheme(Control.ControlCollection controls)
        {
            foreach (Control component in controls)
            {
                if(component.Parent is Form)
                {
                    component.BackColor = (Color)Settings.Settings.BackgroundColour;
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if (component is UserControl)
                {
                    component.BackColor = (Color)Settings.Settings.BackgroundColour;
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if (component is Panel)
                {
                    component.BackColor = (Color)Settings.Settings.BackgroundColour;
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if (component is Button)
                {
                    component.BackColor = (Color)Settings.Settings.ButtonBackgroundColour;
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if (component is Label)
                {
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if (component is CheckBox)
                {
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if(component is GroupBox)
                {
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
                else if(component is ListBox)
                {
                    component.BackColor = (Color)Settings.Settings.ButtonBackgroundColour;
                    component.ForeColor = (Color)Settings.Settings.TextColour;
                }
            }
        }
    }
}
