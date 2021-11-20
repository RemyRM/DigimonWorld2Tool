using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class EditDigimonWindow : Form
    {
        public EditDigimonWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);

            Slot1DigimonIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 4;
            Slot2DigimonIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 4;
            Slot3DigimonIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 4;
            Slot4DigimonIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 4;
        }

        public void SetCurrentDigimonData(int posX, int posY, byte[] digimonSlots)
        {
            DigimonPositionXNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            DigimonPositionXNumericUpDown.Value = posX;
            DigimonPositionYNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            DigimonPositionYNumericUpDown.Value = posY;

            Slot1DigimonIndexNumericUpDown.Value = digimonSlots[0];
            Slot2DigimonIndexNumericUpDown.Value = digimonSlots[1];
            Slot3DigimonIndexNumericUpDown.Value = digimonSlots[2];
            Slot4DigimonIndexNumericUpDown.Value = digimonSlots[3];
        }
    }
}
