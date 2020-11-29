using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigimonWorld2MapVisualizer.Files;
using DigimonWorld2MapVisualizer.Domains;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.UserControls;
using System.Diagnostics;

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
        private static DomainFloor CurrentDomainFloor { get; set; }
        private static DomainMapLayout CurrentMapLayout { get; set; }

        public static RenderLayoutTab[] FloorLayoutRenderTabs { get; private set; } = new RenderLayoutTab[8];
        public static RenderLayoutTab CurrentLayoutRenderTab { get; private set; }

        public static int CurrentFloorIndex;
        public static int CurrentLayoutTabIndex;

        public DigimonWorld2ToolForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.AllowTransparency = true;
            Main = this;
        }

        private void DigimonWorld2ToolForm_Load(object sender, EventArgs e)
        {
            SetupLayoutRenderTabs();

            AddDungeonFilesToComboBox();
            TabControlMain.SelectedIndex = 0;
            DungeonFilesComboBox.SelectedIndex = 0;

            // We select anything non-start index here so the indexChanged gets fired on rendering the first layout
            MapLayoutsTabControl.SelectedIndex = MapLayoutsTabControl.TabCount;  
            
        }

        #region MapVisualizer
        /// <summary>
        /// Add each render tab to the array of possible render tabs
        /// </summary>
        private void SetupLayoutRenderTabs()
        {
            CurrentLayoutRenderTab =  FloorLayoutRenderTabs[0] = renderLayoutTab0;
            FloorLayoutRenderTabs[1] = renderLayoutTab1;
            FloorLayoutRenderTabs[2] = renderLayoutTab2;
            FloorLayoutRenderTabs[3] = renderLayoutTab3;
            FloorLayoutRenderTabs[4] = renderLayoutTab4;
            FloorLayoutRenderTabs[5] = renderLayoutTab5;
            FloorLayoutRenderTabs[6] = renderLayoutTab6;
            FloorLayoutRenderTabs[7] = renderLayoutTab7;

            // Add the function for displaying the current mouse position on the grid to the MouseMove event
            foreach (var item in FloorLayoutRenderTabs)
            {
                item.GridRenderLayer.MouseMove += DisplayMousePositionOnGrid;
            }
        }

        /// <summary>
        /// Add all the domain names to the combobox for selecting the domain
        /// </summary>
        private void AddDungeonFilesToComboBox()
        {
            foreach (var item in DungeonFiles)
            {
                DungeonFilesComboBox.Items.Add(item.DomainName);
            }
        }

        /// <summary>
        /// Create a new domain whenever a new domain gets selected from the combobox
        /// </summary>
        private void DungeonFilesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var filename = DungeonFiles.FirstOrDefault(o => o.DomainName == DungeonFilesComboBox.SelectedItem.ToString()).Filename;
            CreateNewDomain(filename);
        }

        /// <summary>
        /// Create a new domain and start rendering the first floor, first layout.
        /// </summary>
        /// <param name="filename">The filename of the domain to load</param>
        private void CreateNewDomain(string filename)
        {
            FloorSelectorComboBox.Items.Clear();
#if DEBUG
            Debug.WriteLine($"Loading {filename}");
#endif
            CurrentDomain = new Domain(filename);

            CurrentLayoutRenderTab = FloorLayoutRenderTabs[0];
            
            //Automatically select the first floor when we create a new domain
            FloorSelectorComboBox.SelectedIndex = 0;
        }

        private void SetCurrentLayoutInformation()
        {
            if (CurrentMapLayout == null)
                return; 

            OccuranceChanceLabel.Text = $"Occurance chance: {CurrentMapLayout.OccuranceRatePercentage}%";
            MapDataAddressLabel.Text = $"Map data address: {CurrentMapLayout.BaseMapPlanPointerAddressDecimal:X8}";
            WarpsAddressLabel.Text = $"Warps address: {CurrentMapLayout.BaseMapWarpsPointerAddressDecimal:X8}";
            ChestsAddressLabel.Text = $"Chests address: {CurrentMapLayout.BaseMapChestsPointerAddressDecimal:X8}";
            TrapsAddressLabel.Text = $"Traps address: {CurrentMapLayout.BaseMapTrapsPointerAddressDecimal:X8}";
            DigimonAddressLabel.Text = $"Digimons address: {CurrentMapLayout.BaseMapDigimonPointerAddressDecimal:X8}";
        }

        private void SetCurrentFloorInformation()
        {
            if (CurrentDomainFloor == null)
                return;

            FloorNameLabel.Text = $"Name: {CurrentDomainFloor.FloorName}";
            FloorHeaderAddressLabel.Text = $"Header address: {CurrentDomainFloor.FloorBasePointerAddressDecimal:X8}";
            UnknownData1Label.Text = $"Unknown 1: {CurrentDomainFloor.UnknownDataDecimal:X8}";
            UnknownData2Label.Text = $"Unknown 2: {CurrentDomainFloor.UnknownData2Decimal:X8}";
            TrapLevelLabel.Text = $"Trap level: {TextConversion.ByteArrayToHexString(CurrentDomainFloor.TrapLevel)}";
            DigimonPacksLabel.Text = $"Digimon packs: {TextConversion.ByteArrayToHexString(CurrentDomainFloor.DigimonPacks)}";
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlMain.SelectedIndex == 0)
            {
                //We should only load the visualizer when the tab is selected
            }
        }

        /// <summary>
        /// Update the <see cref="CurrentFloorIndex"/> and select the first Layout tab.
        /// Start rendering the first layout of the selected floor
        /// </summary>
        private void FloorSelectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentFloorIndex = FloorSelectorComboBox.SelectedIndex;
            CurrentDomainFloor = CurrentDomain.floorsInThisDomain[CurrentFloorIndex];

            if (MapLayoutsTabControl.SelectedIndex != 0)
            {
                MapLayoutsTabControl.SelectedIndex = 0;
            }
            else
            {
                DrawCurrentMapLayout();
            }

            SetCurrentFloorInformation();
        }

        /// <summary>
        /// Set the <see cref="CurrentLayoutTabIndex"/> to the new index and get the current floor index.
        /// Start rendering the current floor index.
        /// If the selected floor index is higher than the max amount of unique floors we select the last unique floor instead
        /// </summary>
        private void MapLayoutsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentLayoutTabIndex = MapLayoutsTabControl.SelectedIndex;
            CurrentLayoutRenderTab = FloorLayoutRenderTabs[CurrentLayoutTabIndex];

            var availableLayouts = CurrentDomain.floorsInThisDomain[CurrentFloorIndex].UniqueDomainMapLayouts.Count - 1;
            if (CurrentLayoutTabIndex > availableLayouts)
            {
                CurrentLayoutTabIndex = MapLayoutsTabControl.SelectedIndex = availableLayouts;
                CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[CurrentLayoutTabIndex];

                LayoutNotAvailableLabel.Visible = true;
                DisableLayoutNotAvailableMessage();

                if (CurrentLayoutRenderTab.MapRenderLayer == FloorLayoutRenderTabs[CurrentLayoutTabIndex].MapRenderLayer)
                    return;
            }

            CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[CurrentLayoutTabIndex];

            SetCurrentLayoutInformation();

            CurrentLayoutRenderTab = FloorLayoutRenderTabs[CurrentLayoutTabIndex];
            DrawCurrentMapLayout();
        }

        private async void DisableLayoutNotAvailableMessage()
        {
            await Task.Delay(5000);
            LayoutNotAvailableLabel.Visible = false;
        }

        /// <summary>
        /// Call the renderer for the current floor's current layout tab
        /// </summary>
        /// <param name="mapLayoutIndex"></param>
        private void DrawCurrentMapLayout()
        {
            LayoutRenderer.SetupFloorLayerBitmap();
            CurrentDomain.floorsInThisDomain[CurrentFloorIndex].UniqueDomainMapLayouts[CurrentLayoutTabIndex].DrawMap();
        }

        /// <summary>
        /// Display the current mouse position on the grid
        /// </summary>
        private void DisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            MousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / LayoutRenderer.tileSize:00} Y: {(int)e.Location.Y / LayoutRenderer.tileSize:00}";
        }

        /// <summary>
        /// Update the per-tile size of the grid, and redraw the grid at this size.
        /// </summary>
        private void ResizeGridButton_Click(object sender, EventArgs e)
        {
            LayoutRenderer.tileSize = (int)TileSizeInput.Value;
            LayoutRenderer.DrawGrid();
        }

        private void ShowGridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if(ShowGridCheckbox.Checked)
                LayoutRenderer.DrawGrid();
            else
                LayoutRenderer.HideGrid();
        }

        /// <summary>
        /// Save the currently rendered layout to an image file format (png or bmp)
        /// </summary>
        private void SaveLayoutToFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|Bitmap Image|*.bmp",
                Title = "Save current layout to file"
            };

            saveFileDialog1.FileName = $"{FloorSelectorComboBox.Text.Replace(" ", "_")}_Layout_{CurrentLayoutTabIndex}";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    System.IO.FileStream fs =
                        (System.IO.FileStream)saveFileDialog1.OpenFile();

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            CurrentLayoutRenderTab.MapRenderLayer.Image.Save(fs, ImageFormat.Png);
                            break;

                        case 2:
                            CurrentLayoutRenderTab.MapRenderLayer.Image.Save(fs, ImageFormat.Bmp);
                            break;
                    }

                    fs.Close();
                }
            }
        }
        #endregion
    }
}