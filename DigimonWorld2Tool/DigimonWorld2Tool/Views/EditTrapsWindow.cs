using DigimonWorld2Tool.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class EditTrapsWindow : Form
    {
        public EditTrapsWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);
        }

        public void SetCurrentTrapData(int posX, int posY, byte[] trapData)
        {
            TrapPositionXNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            TrapPositionXNumericUpDown.Value = posX;
            TrapPositionYNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            TrapPositionYNumericUpDown.Value = posY;

            Slot1TrapLevelNumericUpDown.Value = trapData[0].GetLeftNiblet();
            Slot2TrapLevelNumericUpDown.Value = trapData[1].GetLeftNiblet();
            Slot3TrapLevelNumericUpDown.Value = trapData[2].GetLeftNiblet();
            Slot4TrapLevelNumericUpDown.Value = trapData[3].GetLeftNiblet();

            Slot1TrapTypeComboBox.SelectedIndex = trapData[0].GetRightNiblet();
            Slot2TrapTypeComboBox.SelectedIndex = trapData[1].GetRightNiblet();
            Slot3TrapTypeComboBox.SelectedIndex = trapData[2].GetRightNiblet();
            Slot4TrapTypeComboBox.SelectedIndex = trapData[3].GetRightNiblet();
        }

        public byte[] GetTrapTypeAndLevelData()
        {
            byte[] typeAndLevelData = new byte[4];
            typeAndLevelData[0] = (byte)(((byte)Slot1TrapLevelNumericUpDown.Value << 4) | (byte)Slot1TrapTypeComboBox.SelectedIndex);
            typeAndLevelData[1] = (byte)(((byte)Slot2TrapLevelNumericUpDown.Value << 4) | (byte)Slot2TrapTypeComboBox.SelectedIndex);
            typeAndLevelData[2] = (byte)(((byte)Slot3TrapLevelNumericUpDown.Value << 4) | (byte)Slot3TrapTypeComboBox.SelectedIndex);
            typeAndLevelData[3] = (byte)(((byte)Slot4TrapLevelNumericUpDown.Value << 4) | (byte)Slot4TrapTypeComboBox.SelectedIndex);
            return typeAndLevelData;
        }
    }
}
