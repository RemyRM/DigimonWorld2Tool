using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class EditChestWindow : Form
    {
        public EditChestWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);

            Slot1ItemIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 8;
            Slot2ItemIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 8;
            Slot3ItemIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 8;
            Slot4ItemIndexNumericUpDown.Maximum = Settings.Settings.RemoveIDCap ? 255 : 8;
        }

        public void SetCurrentChestData(int posX, int posY, byte[] itemSlots)
        {
            ChestPositionXNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            ChestPositionXNumericUpDown.Value = posX;
            ChestPositionYNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            ChestPositionYNumericUpDown.Value = posY;

            Slot1ItemIndexNumericUpDown.Value = itemSlots[0];
            Slot2ItemIndexNumericUpDown.Value = itemSlots[1];
            Slot3ItemIndexNumericUpDown.Value = itemSlots[2];
            Slot4ItemIndexNumericUpDown.Value = itemSlots[3];
        }
    }
}
