using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigimonWorld2MapVisualizer.Files;
using DigimonWorld2MapVisualizer.Domains;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.MapObjects;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.UserControls;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

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

        private static List<string> LogStack = new List<string>();

        private static Domain CurrentDomain { get; set; }
        private static DomainFloor CurrentDomainFloor { get; set; }
        public static DomainMapLayout CurrentMapLayout { get; set; }

        public static RenderLayoutTab[] FloorLayoutRenderTabs { get; private set; } = new RenderLayoutTab[8];
        public static RenderLayoutTab CurrentLayoutRenderTab { get; private set; }

        public static string FilePathToMapDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Maps\\";

        public static int CurrentFloorIndex;
        public static int CurrentLayoutTabIndex;

        public enum Strictness
        {
            Strict,
            Sloppy,
        }
        public static Strictness ErrorMode;

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

            ErrorCheckingComboBox.Items.Add(Strictness.Strict);
            ErrorCheckingComboBox.Items.Add(Strictness.Sloppy);
            LoadUserSettings();
            AddDungeonFilesToComboBox();
            TabControlMain.SelectedIndex = 0;
            DungeonFilesComboBox.SelectedIndex = 0;

            // We select anything non-start index here so the indexChanged gets fired on rendering the first layout
            MapLayoutsTabControl.SelectedIndex = MapLayoutsTabControl.TabCount;
        }

        private void LoadUserSettings()
        {
            FilePathToMapDirectory = (string)Properties.Settings.Default["MapDataFolder"];
            if (FilePathToMapDirectory == "")
                FilePathToMapDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Maps\\";
            CurrentMapDataFolderLabel.Text = FilePathToMapDirectory;

            GridPosHexCheckBox.Checked = (bool)Properties.Settings.Default["ShowGridPosAsHex"];
            ShowGridCheckbox.Checked = (bool)Properties.Settings.Default["ShowGridLines"];
            TileSizeInput.Value = (int)Properties.Settings.Default["GridTileSize"];
            ErrorCheckingComboBox.SelectedIndex = (int)Enum.Parse(typeof(Strictness), (string)Properties.Settings.Default["ErrorCheckingLevel"]);
            ShowLogsCheckBox.Checked = (bool)Properties.Settings.Default["ShowLogs"];
        }

        #region MapVisualizer
        /// <summary>
        /// Add each render tab to the array of possible render tabs
        /// </summary>
        private void SetupLayoutRenderTabs()
        {
            CurrentLayoutRenderTab = FloorLayoutRenderTabs[0] = renderLayoutTab0;
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
            AddLogToLogWindow($"Loading {filename}");

            try
            {
                CurrentDomain = new Domain(filename);

                CurrentLayoutRenderTab = FloorLayoutRenderTabs[0];

                //Automatically select the first floor when we create a new domain
                FloorSelectorComboBox.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                AddErrorToLogWindow(e, false);
            }
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

        public void SetCurrentObjectInformation(Tile tile)
        {
            ResetCurrentObjectInformation();
            ObjectTypeLabel.Text = $"Type: {tile.FloorObject.ObjectType}";
            ObjectPositionLabel.Text = GridPosHexCheckBox.Checked ? $"Position: ({tile.Position.x:X2}, {tile.Position.y:X2})" : $"Position: {tile.Position}";

            switch (tile.FloorObject.ObjectType)
            {
                case IFloorLayoutObject.MapObjectType.Warp:
                    Warp warp = (Warp)tile.FloorObject;
                    ObjectSubTypeLabel.Text = $"Sub type: {warp.Type}";
                    break;
                case IFloorLayoutObject.MapObjectType.Chest:
                    Chest chest = (Chest)tile.FloorObject;
                    ObjectSlotOneLabel.Text = $"Slot 1: {chest.chestSlots[0].ItemName} - {chest.chestSlots[0].TrapLevel}";
                    ObjectSlotTwoLabel.Text = $"Slot 2: {chest.chestSlots[1].ItemName} - {chest.chestSlots[1].TrapLevel}";
                    ObjectSlotThreeLabel.Text = $"Slot 3: {chest.chestSlots[2].ItemName} - {chest.chestSlots[2].TrapLevel}";
                    ObjectSlotFourLabel.Text = $"Slot 4: {chest.chestSlots[3].ItemName} - {chest.chestSlots[3].TrapLevel}";
                    break;
                case IFloorLayoutObject.MapObjectType.Trap:
                    Trap trap = (Trap)tile.FloorObject;
                    ObjectSubTypeLabel.Text = $"Sub type: {trap.Type}";
                    ObjectSlotOneLabel.Text = $"Slot 1: {trap.TrapSlots[0]}";
                    ObjectSlotTwoLabel.Text = $"Slot 2: {trap.TrapSlots[1]}";
                    ObjectSlotThreeLabel.Text = $"Slot 3: {trap.TrapSlots[2]}";
                    ObjectSlotFourLabel.Text = $"Slot 4: {trap.TrapSlots[3]}";
                    break;
                case IFloorLayoutObject.MapObjectType.Digimon:
                    Digimon digimon = (Digimon)tile.FloorObject;
                    ObjectSubTypeLabel.Text = $"Pack 1 Level: {digimon.DigimonPacks[0].Level}";
                    ObjectSlotOneLabel.Text = $"Slot 1: {digimon.DigimonPacks[0].ObjectModelDigimonName:X2}";
                    ObjectSlotTwoLabel.Text = $"Slot 2: {digimon.DigimonPacks[1].ObjectModelDigimonName:X2}";
                    ObjectSlotThreeLabel.Text = $"Slot 3: {digimon.DigimonPacks[2].ObjectModelDigimonName:X2}";
                    ObjectSlotFourLabel.Text = $"Slot 4: {digimon.DigimonPacks[3].ObjectModelDigimonName:X2}";
                    break;
                default:
                    break;
            }
        }

        public void ResetCurrentObjectInformation()
        {
            ObjectTypeLabel.Text = $"Type: ";
            ObjectPositionLabel.Text = "Position: ";
            ObjectSubTypeLabel.Text = "Sub type: ";
            ObjectSlotOneLabel.Text = "Slot 1:";
            ObjectSlotTwoLabel.Text = "Slot 2:";
            ObjectSlotThreeLabel.Text = "Slot 3:";
            ObjectSlotFourLabel.Text = "Slot 4:";
        }

        private void TabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[0];
                DrawCurrentMapLayout();
                SetCurrentLayoutInformation();
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
            if(CurrentDomain == null)
                return;

            CurrentLayoutTabIndex = MapLayoutsTabControl.SelectedIndex;
            CurrentLayoutRenderTab = FloorLayoutRenderTabs[CurrentLayoutTabIndex];

            var availableLayouts = CurrentDomain.floorsInThisDomain[CurrentFloorIndex].UniqueDomainMapLayouts.Count - 1;
            if (CurrentLayoutTabIndex > availableLayouts)
            {
                CurrentLayoutTabIndex = MapLayoutsTabControl.SelectedIndex = availableLayouts;
                CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[CurrentLayoutTabIndex];

                AddWarningToLogWindow($"Selected layout was not unique, selecting last unique layout.", false);

                if (CurrentLayoutRenderTab.MapRenderLayer == FloorLayoutRenderTabs[CurrentLayoutTabIndex].MapRenderLayer)
                    return;
            }

            CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[CurrentLayoutTabIndex];

            SetCurrentLayoutInformation();

            CurrentLayoutRenderTab = FloorLayoutRenderTabs[CurrentLayoutTabIndex];
            DrawCurrentMapLayout();
        }

        /// <summary>
        /// Call the renderer for the current floor's current layout tab
        /// </summary>
        /// <param name="mapLayoutIndex"></param>
        private void DrawCurrentMapLayout()
        {
            LayoutRenderer.SetupFloorLayerBitmap();
            CurrentMapLayout.DrawMap();
        }

        /// <summary>
        /// Display the current mouse position on the grid
        /// </summary>
        private void DisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            if (GridPosHexCheckBox.Checked)
            {
                MousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / LayoutRenderer.tileSize:X2} Y: {(int)e.Location.Y / LayoutRenderer.tileSize:X2}";
            }
            else
            {
                MousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / LayoutRenderer.tileSize:00} Y: {(int)e.Location.Y / LayoutRenderer.tileSize:00}";
            }
        }

        /// <summary>
        /// Update the per-tile size of the grid, and redraw the grid at this size.
        /// </summary>
        private void ResizeGridButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["GridTileSize"] = (int)TileSizeInput.Value;
            LayoutRenderer.tileSize = (int)TileSizeInput.Value;
            DrawCurrentMapLayout();
            if (ShowGridCheckbox.Checked)
                LayoutRenderer.DrawGrid();
        }

        private void ShowGridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowGridLines"] = ShowGridCheckbox.Checked;
            if (ShowGridCheckbox.Checked)
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

        #region tab2
        private void SelectFileButton_Click(object sender, EventArgs e)
        {

            ReadFilesRecursively();


            //using (OpenFileDialog fd = new OpenFileDialog())
            //{
            //    fd.InitialDirectory = @"D:\Program Files (x86)\ePSXe\Games\DigimonWorld2\Extracted\AAA\4.AAA";
            //    fd.Filter = "Bin files (.bin)|*.bin";
            //    fd.RestoreDirectory = true;

            //    if(fd.ShowDialog() == DialogResult.OK)
            //    {
            //        var filePath = fd.FileName;

            //        var fileStream = fd.OpenFile();

            //        byte[] arr;
            //        using (BinaryReader reader = new BinaryReader(fileStream))
            //        {
            //            using MemoryStream memoryStream = new MemoryStream();
            //            reader.BaseStream.CopyTo(memoryStream);
            //            arr =  memoryStream.ToArray();
            //        }

            //        var result = TextConversion.DigiBytesToString(arr);
            //    }
            //}

        }
        #endregion

        static string originalBaseDirectory = @"D:\Program Files (x86)\ePSXe\Games\DigimonWorld2\Extracted\AAA\4.AAA";
        static string destinationBaseDirection = @"D:\Dev\C#\DigimonWorld2MapVisualizer\ConvertedFiles\AAA\4.AAA";
        private static void ReadFilesRecursively()
        {

            ProcessDirectory(originalBaseDirectory);
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (subdirectory.Contains(@"4.AAA\CITY\BG") ||
                    subdirectory.Contains(@"4.AAA\CITY\SOUND") ||
                    subdirectory.Contains(@"4.AAA\DUNG\DUNG") ||
                    subdirectory.Contains(@"4.AAA\DUNG\SOUND"))
                    continue;

                var targetDir = subdirectory.Replace(originalBaseDirectory, destinationBaseDirection);
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);

                ProcessDirectory(subdirectory);
            }
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            //Debug.WriteLine("Processed file '{0}'.", path);
            string targetPath = path.Replace(originalBaseDirectory, destinationBaseDirection);
            targetPath = targetPath.Replace(".BIN", ".txt");

            Debug.WriteLine(targetPath);
            byte[] arr;
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                using MemoryStream memoryStream = new MemoryStream();
                reader.BaseStream.CopyTo(memoryStream);
                arr = memoryStream.ToArray();
            }

            var result = TextConversion.DigiBytesToString(arr);

            File.WriteAllText(targetPath, result);
        }

        private void GridPosHexCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowGridPosAsHex"] = GridPosHexCheckBox.Checked;
        }

        private void ErrorCheckingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ErrorCheckingLevel"] = ErrorCheckingComboBox.SelectedItem.ToString();
            ErrorMode = (Strictness)ErrorCheckingComboBox.SelectedIndex;
            switch (ErrorMode)
            {
                case Strictness.Strict:
                    AddLogToLogWindow($"Error checking set to {Strictness.Strict}, Errors will stop execution.");
                    break;
                case Strictness.Sloppy:
                    AddLogToLogWindow($"Error checking set to {Strictness.Sloppy}, Errors will be ignored when possible.");
                    break;
            }
        }

        public void AddErrorToLogWindow(object error, bool stackTrace = true)
        {
            StackFrame callStack = new StackFrame(1, true);
            var delimiterIndex = callStack.GetFileName().LastIndexOf("\\");
            var fileName = callStack.GetFileName().Substring(delimiterIndex + 1, callStack.GetFileName().Length - delimiterIndex - 1);
            var errorWithStack = $"{fileName}:{callStack.GetFileLineNumber()} - {error}{Environment.NewLine}";

            if (ShowLogsCheckBox.Checked)
            {
                LogRichTextBox.SelectionColor = Color.Red;

                if (stackTrace)
                    LogRichTextBox.AppendText($"{errorWithStack}");
                else
                    LogRichTextBox.AppendText($"{error}{Environment.NewLine}");

                LogRichTextBox.SelectionColor = Color.White;
            }

            LogStack.Add(errorWithStack);
        }

        public void AddWarningToLogWindow(object warning, bool stackTrace = true)
        {
            StackFrame callStack = new StackFrame(1, true);
            var delimiterIndex = callStack.GetFileName().LastIndexOf("\\");
            var fileName = callStack.GetFileName().Substring(delimiterIndex + 1, callStack.GetFileName().Length - delimiterIndex - 1);
            var warningWithStack = $"{fileName}:{callStack.GetFileLineNumber()} - {warning}{Environment.NewLine}";

            if (ShowLogsCheckBox.Checked)
            {
                LogRichTextBox.SelectionColor = Color.Yellow;

                if (stackTrace)
                    LogRichTextBox.AppendText($"{warningWithStack}");
                else
                    LogRichTextBox.AppendText($"{warning}{Environment.NewLine}");

                LogRichTextBox.SelectionColor = Color.White;
            }
            LogStack.Add(warningWithStack);
        }

        public void AddLogToLogWindow(object log)
        {
            LogRichTextBox.SelectionColor = Color.White;
            LogRichTextBox.AppendText($"{log}{Environment.NewLine}");

            LogStack.Add(log.ToString());
        }

        /// <summary>
        /// Make sure we automatically scroll to the bottom of the log
        /// </summary>
        private void LogRichTextBox_TextChanged(object sender, EventArgs e)
        {
            LogRichTextBox.SelectionStart = LogRichTextBox.Text.Length;
            LogRichTextBox.ScrollToCaret();
        }

        private void ShowLogsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowLogs"] = ShowLogsCheckBox.Checked;
        }

        private void SelectMapDataFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if(folderBrowser.ShowDialog() == DialogResult.OK)
            {
                folderBrowser.SelectedPath += "\\";
                Properties.Settings.Default["MapDataFolder"] = folderBrowser.SelectedPath;
                FilePathToMapDirectory = folderBrowser.SelectedPath;
                AddLogToLogWindow($"Changed map data directory to {FilePathToMapDirectory}");
                CurrentMapDataFolderLabel.Text = folderBrowser.SelectedPath;
            }
        }
    }
}