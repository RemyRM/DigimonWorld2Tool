using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
