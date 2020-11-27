using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigimonWorld2MapVisualizer.Files;
using DigimonWorld2MapVisualizer.Domains;
using DigimonWorld2Tool.Rendering;

namespace DigimonWorld2Tool
{
    public partial class DigimonWorld2ToolForm : Form
    {
        public static DigimonWorld2ToolForm Main;
        private static readonly DungFile[] DungeonFiles = new DungFile[]
        {
            new DungFile("DUNG4000", "SCSI Domain 1", 0x00, 0x3A),
            new DungFile("DUNG4100", "Video Domain 1", 0x01, 0xE1),
            new DungFile("DUNG4200", "Disk Domain 1", 0x02, 0xE2),
            new DungFile("DUNG4300", "BIOS Domain 1", 0x03, 0x26),
            new DungFile("DUNG4400", "Drive Domain 1", 0x04, 0xE3),
            new DungFile("DUNG4500", "Web Domain 1", 0x05, 0xE4),
            new DungFile("DUNG4600", "Modem Domain 1", 0x06, 0xE5),
            new DungFile("DUNG4700", "SCSI Domain 2", 0x07, 0xE6),
            new DungFile("DUNG4800", "Video Domain 2", 0x08, 0xE7),
            new DungFile("DUNG4900", "Disk Domain 2", 0x09, 0xE8),
            new DungFile("DUNG5000", "BIOS Domain 2", 0x0A, 0xE9),
            new DungFile("DUNG5100", "Drive Domain 2", 0x0B, 0xEA),
            new DungFile("DUNG5200", "Web Domain 2", 0x0C, 0xEB),
            new DungFile("DUNG5300", "Modem Domain 2", 0x0D, 0xEC),
            new DungFile("DUNG5400", "Code Domain", 0x0E, 0xED),
            new DungFile("DUNG5500", "Laser Domain", 0x0F, 0xEE),
            new DungFile("DUNG5600", "Giga Domain", 0x10, 0xEF),
            new DungFile("DUNG5700", "Diode Domain", 0x11, 0xF0),
            new DungFile("DUNG5800", "Port Domain", 0x12, 0xF1),
            new DungFile("DUNG5900", "Scan Domain", 0x13, 0xF2),
            new DungFile("DUNG6000", "Data Domain", 0x14, 0xF3),
            new DungFile("DUNG6100", "Patch Domain", 0x15, 0xF4),
            new DungFile("DUNG6200", "Mega Domain", 0x16, 0xF5),
            new DungFile("DUNG6300", "Soft Domain", 0x17, 0xF6),
            new DungFile("DUNG6400", "Bug Domian", 0x18, 0xF7),
            new DungFile("DUNG6500", "RAM Domian", 0x19, 0xF8),
            new DungFile("DUNG6600", "ROM Domain", 0x1A, 0xF9),
            new DungFile("DUNG6700", "Core Tower", 0x1B, 0xFA),
            new DungFile("DUNG6800", "Chaos Tower", 0x1C, 0xFB),
            new DungFile("DUNG6900", "Boot Domain", 0x1D, 0xFC),
            new DungFile("DUNG7000", "DVD Domain", 0x1E, 0xFD),
            new DungFile("DUNG7100", "Power Domain", 0x1F, 0xFE),
            new DungFile("DUNG7200", "Tera Domain", 0x20, 0xFF),
            new DungFile("DUNG7300", "ABCDE", 0x21, 0x00),
        };
        private static Domain CurrentDomain { get; set; }

        public static PictureBox[] FloorLayoutRenderers { get; private set; } = new PictureBox[8];

        public static bool ShowOriginalValueInMapTile = false;

        public DigimonWorld2ToolForm()
        {
            InitializeComponent();
            Main = this;
        }

        private void DigimonWorld2ToolForm_Load(object sender, EventArgs e)
        {
            AddPictureBoxes();

            AddDungeonFilesToComboBox();
            TabControlMain.SelectedIndex = 0;
            DungeonFilesComboBox.SelectedIndex = 0;
        }

        #region MapVisualizer
        private void AddPictureBoxes()
        {
            FloorLayoutRenderers[0] = PictureBox0Layout0;
            FloorLayoutRenderers[1] = PictureBox0Layout1;
            FloorLayoutRenderers[2] = PictureBox0Layout2;
            FloorLayoutRenderers[3] = PictureBox0Layout3;
            FloorLayoutRenderers[4] = PictureBox0Layout4;
            FloorLayoutRenderers[5] = PictureBox0Layout5;
            FloorLayoutRenderers[6] = PictureBox0Layout6;
            FloorLayoutRenderers[7] = PictureBox0Layout7;
        }

        private void AddDungeonFilesToComboBox()
        {
            foreach (var item in DungeonFiles)
            {
                DungeonFilesComboBox.Items.Add(item.DomainName);
            }
        }

        private void DungeonFilesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filename = DungeonFiles.FirstOrDefault(o => o.DomainName == DungeonFilesComboBox.SelectedItem.ToString()).Filename;
            CreateNewDomain(filename);
        }

        private void CreateNewDomain(string filename)
        {
            FloorSelectorComboBox.Items.Clear();
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Loading {filename}");
#endif
            CurrentDomain = new Domain(filename);

            LayoutRenderer.SetRenderTarget(FloorLayoutRenderers[0]);
            FloorSelectorComboBox.SelectedIndex = 0;
            MapLayoutsTabControl.SelectedIndex = 0;
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
            {
                //We should only load the visualizer when the tab is selected
            }
        }

        private void FloorSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LayoutRenderer.SetRenderTarget(FloorLayoutRenderers[MapLayoutsTabControl.SelectedIndex]);
            CurrentDomain.floorsInThisDomain[FloorSelectorComboBox.SelectedIndex].UniqueDomainMapLayouts[0].DrawMap();
        }

        private void MapLayoutsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MapLayoutsTabControl.SelectedIndex > CurrentDomain.floorsInThisDomain[FloorSelectorComboBox.SelectedIndex].UniqueDomainMapLayouts.Count - 1)
            {
                MapLayoutsTabControl.SelectedIndex = CurrentDomain.floorsInThisDomain[FloorSelectorComboBox.SelectedIndex].UniqueDomainMapLayouts.Count - 1;
                LayoutNotAvailableLabel.Visible = true;
                DisableLayoutNotAvailableMessage();
                if (LayoutRenderer.currentTargetRenderer == FloorLayoutRenderers[MapLayoutsTabControl.SelectedIndex]) 
                    return;
            }
            LayoutRenderer.SetRenderTarget(FloorLayoutRenderers[MapLayoutsTabControl.SelectedIndex]);
            CurrentDomain.floorsInThisDomain[FloorSelectorComboBox.SelectedIndex].UniqueDomainMapLayouts[MapLayoutsTabControl.SelectedIndex].DrawMap();
        }

        private async void DisableLayoutNotAvailableMessage()
        {
            await Task.Delay(5000);
            LayoutNotAvailableLabel.Visible = false;
        }
        #endregion
    }
}