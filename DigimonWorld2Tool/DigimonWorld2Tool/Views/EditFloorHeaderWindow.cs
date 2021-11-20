using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class EditFloorHeaderWindow : Form
    {
        private readonly List<Label> DigimonNameLabels = new List<Label>();
        private readonly List<Label> ItemNameLabels = new List<Label>();

        public EditFloorHeaderWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);
            Utility.ColourTheme.SetColourScheme(DigimonPacksGroupbox.Controls);
            Utility.ColourTheme.SetColourScheme(ChestDataGroupbox.Controls);


            DigimonNameLabels.Add(Pack0CenterNameLabel);
            DigimonNameLabels.Add(Pack0LeftNameLabel);
            DigimonNameLabels.Add(Pack0RightNameLabel);
            DigimonNameLabels.Add(Pack1CenterNameLabel);
            DigimonNameLabels.Add(Pack1LeftNameLabel);
            DigimonNameLabels.Add(Pack1RightNameLabel);
            DigimonNameLabels.Add(Pack2CenterNameLabel);
            DigimonNameLabels.Add(Pack2LeftNameLabel);
            DigimonNameLabels.Add(Pack2RightNameLabel);
            DigimonNameLabels.Add(Pack3CenterNameLabel);
            DigimonNameLabels.Add(Pack3LeftNameLabel);
            DigimonNameLabels.Add(Pack3RightNameLabel);

            ItemNameLabels.Add(Treasure0ItemNameLabel);
            ItemNameLabels.Add(Treasure1ItemNameLabel);
            ItemNameLabels.Add(Treasure2ItemNameLabel);
            ItemNameLabels.Add(Treasure3ItemNameLabel);
            ItemNameLabels.Add(Treasure4ItemNameLabel);
            ItemNameLabels.Add(Treasure5ItemNameLabel);
            ItemNameLabels.Add(Treasure6ItemNameLabel);
            ItemNameLabels.Add(Treasure7ItemNameLabel);
        }

        public void SetFloorHeaderDigimonPackData(byte[] digimonIds)
        {
            Pack0IDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Pack1IDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Pack2IDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Pack3IDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];

            Pack0IDNumericUpDown.Value = digimonIds[0];
            Pack1IDNumericUpDown.Value = digimonIds[1];
            Pack2IDNumericUpDown.Value = digimonIds[2];
            Pack3IDNumericUpDown.Value = digimonIds[3];

            for (int i = 0; i < digimonIds.Length; i++)
                SetPackDigimonNameData(digimonIds[i], i);
        }

        private void Pack0IDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetPackDigimonNameData((byte)send.Value, 0);
        }


        private void Pack1IDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetPackDigimonNameData((byte)send.Value, 1);
        }


        private void Pack2IDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetPackDigimonNameData((byte)send.Value, 2);
        }

        private void Pack3IDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetPackDigimonNameData((byte)send.Value, 3);
        }

        private void SetPackDigimonNameData(byte digimonID, int index)
        {
            var enemySet = Settings.Settings.ENEMYSETFile.GetSetHeaderByCenterDigiID(digimonID);
            if (enemySet == null)
            {
                Pack0CenterNameLabel.Text = "No data available";
                Pack0LeftNameLabel.Text = "No data available";
                Pack0RightNameLabel.Text = "No data available";
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                if (enemySet.DigimonInSet[i].DigimonID == 0x00)
                    continue;

                int labelIndex = index * 3 + i;
                var nameData = Settings.Settings.MODELDT0File.GetDigimonByDigimonID(enemySet.DigimonInSet[i].DigimonID).NameData;
                DigimonNameLabels[labelIndex].Text = $"{TextConversion.DigiStringToASCII(nameData)}";
            }
        }

        public void SetFloorHeaderChestData(List<byte[]> chestData)
        {
            Treasure0ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure1ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure2ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure3ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure4ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure5ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure6ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            Treasure7ItemIDNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];

            Treasure0ItemIDNumericUpDown.Value = chestData[0][0];
            Treasure1ItemIDNumericUpDown.Value = chestData[1][0];
            Treasure2ItemIDNumericUpDown.Value = chestData[2][0];
            Treasure3ItemIDNumericUpDown.Value = chestData[3][0];
            Treasure4ItemIDNumericUpDown.Value = chestData[4][0];
            Treasure5ItemIDNumericUpDown.Value = chestData[5][0];
            Treasure6ItemIDNumericUpDown.Value = chestData[6][0];
            Treasure7ItemIDNumericUpDown.Value = chestData[7][0];

            Treasure0TrapLevelNumericUpDown.Value = chestData[0][1];
            Treasure1TrapLevelNumericUpDown.Value = chestData[1][1];
            Treasure2TrapLevelNumericUpDown.Value = chestData[2][1];
            Treasure3TrapLevelNumericUpDown.Value = chestData[3][1];
            Treasure4TrapLevelNumericUpDown.Value = chestData[4][1];
            Treasure5TrapLevelNumericUpDown.Value = chestData[5][1];
            Treasure6TrapLevelNumericUpDown.Value = chestData[6][1];
            Treasure7TrapLevelNumericUpDown.Value = chestData[7][1];

            for (int i = 0; i < chestData.Count; i++)
                SetTreasureName(chestData[i][0], i);
        }

        private void SetTreasureName(byte data, int index)
        {
            if (data == 0x00)
            {
                ItemNameLabels[index].Text = "No treasure";
                return;
            }
            var item0Data = Settings.Settings.ITEMDATAFile.ItemData.FirstOrDefault(o => o.ID == data);
            ItemNameLabels[index].Text = TextConversion.DigiStringToASCII(item0Data.NameData);
        }

        private void Treasure0ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 0);
        }

        private void Treasure1ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 1);
        }

        private void Treasure2ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 2);
        }

        private void Treasure3ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 3);
        }

        private void Treasure4ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 4);
        }

        private void Treasure5ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 5);
        }

        private void Treasure6ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 6);
        }

        private void Treasure7ItemIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown send = (NumericUpDown)sender;
            SetTreasureName((byte)send.Value, 7);
        }
    }
}
