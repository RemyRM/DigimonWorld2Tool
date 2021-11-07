using System.Drawing;
using System.Windows.Forms;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.FileFormats;

namespace DigimonWorld2Tool.Views
{
    public partial class ExtendedEnemySetInfoWindow : Form
    {
        public ExtendedEnemySetInfoWindow(byte setID)
        {
            InitializeComponent();

            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;

            var enemySet = Settings.Settings.ENEMYSETFile.GetSetHeaderByCenterDigiID(setID);
            //var enemySet = Settings.Settings.ENEMYSETFile.EnemySets[setID];
            SetCenterDigimonData(enemySet.DigimonInSet[0]);
            SetLeftDigimonData(enemySet.DigimonInSet[1]);
            SetRightDigimonData(enemySet.DigimonInSet[2]);
        }

        private void SetCenterDigimonData(EnemySetSlot enemy)
        {
            var nameData = Settings.Settings.MODELDT0File.GetDigimonByDigimonID(enemy.DigimonID).NameData;
            CenterDigimonNameLabel.Text = $"Name: {TextConversion.DigiStringToASCII(nameData)}";
            CenterDigimonLevelLabel.Text = $"Lv: {enemy.Lv:D2}";
            CenterDigimonHpLabel.Text = $"HP: {enemy.HP:D2}";
            CenterDigimonMpLabel.Text = $"MP: {enemy.MP:D2}";
            CenterDigimonAtkLabel.Text = $"Atk: {enemy.Atk:D2}";
            CenterDigimonDefLabel.Text = $"Def: {enemy.Def:D2}";
            CenterDigimonSpdLabel.Text = $"Spd: {enemy.Spd:D2}";
            CenterDigimonExpLabel.Text = $"Exp: {enemy.EXP:D2}";
            CenterDigimonBitsLabel.Text = $"Bits: {enemy.BITS:D2}";
        }

        private void SetLeftDigimonData(EnemySetSlot enemy)
        {
            if (enemy.DigimonID == 0x00)
                return;

            var nameData = Settings.Settings.MODELDT0File.GetDigimonByDigimonID(enemy.DigimonID).NameData;
            LeftDigimonNameLabel.Text = $"Name: {TextConversion.DigiStringToASCII(nameData)}";
            LeftDigimonLevelLabel.Text = $"Lv: {enemy.Lv:D2}";
            LeftDigimonHpLabel.Text = $"HP: {enemy.HP:D2}";
            LeftDigimonMpLabel.Text = $"MP: {enemy.MP:D2}";
            LeftDigimonAtkLabel.Text = $"Atk: {enemy.Atk:D2}";
            LeftDigimonDefLabel.Text = $"Def: {enemy.Def:D2}";
            LeftDigimonSpdLabel.Text = $"Spd: {enemy.Spd:D2}";
            LeftDigimonExpLabel.Text = $"Exp: {enemy.EXP:D2}";
            LeftDigimonBitsLabel.Text = $"Bits: {enemy.BITS:D2}";
        }

        private void SetRightDigimonData(EnemySetSlot enemy)
        {
            if (enemy.DigimonID == 0x00)
                return;

            var nameData = Settings.Settings.MODELDT0File.GetDigimonByDigimonID(enemy.DigimonID).NameData;
            RightDigimonNameLabel.Text = $"Name: {TextConversion.DigiStringToASCII(nameData)}";
            RightDigimonLevelLabel.Text = $"Lv: {enemy.Lv:D2}";
            RightDigimonHpLabel.Text = $"HP: {enemy.HP:D2}";
            RightDigimonMpLabel.Text = $"MP: {enemy.MP:D2}";
            RightDigimonAtkLabel.Text = $"Atk: {enemy.Atk:D2}";
            RightDigimonDefLabel.Text = $"Def: {enemy.Def:D2}";
            RightDigimonSpdLabel.Text = $"Spd: {enemy.Spd:D2}";
            RightDigimonExpLabel.Text = $"Exp: {enemy.EXP:D2}";
            RightDigimonBitsLabel.Text = $"Bits: {enemy.BITS:D2}";
        }
    }
}
