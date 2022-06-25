using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

using DigimonWorld2Tool.Files;
using DigimonWorld2Tool.Domains;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.MapObjects;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.UserControls;
using DigimonWorld2Tool.Textures;
using DigimonWorld2Tool.Files;

using DigimonWorld2Tool.FileFormat;

namespace DigimonWorld2Tool
{
    public partial class DigimonWorld2ToolForm : Form
    {
        public static DigimonWorld2ToolForm Main;

        private static readonly List<DungFile> DungeonFiles = new List<DungFile>();
        private static List<string> LogStack = new List<string>();

        private static Domain CurrentDomain { get; set; }
        private static DomainFloor CurrentDomainFloor { get; set; }
        public static DomainMapLayout CurrentMapLayout { get; set; }

        public static RenderLayoutTab[] FloorLayoutRenderTabs { get; private set; } = new RenderLayoutTab[8];
        public static RenderLayoutTab CurrentLayoutRenderTab { get; private set; }

        public static RenderLayoutTab[] EditorFloorLayoutRenderTabs { get; private set; } = new RenderLayoutTab[8];
        public static RenderLayoutTab EditorLayoutRenderTab { get; private set; }

        public static Tile.DomainTileTypeOld EditorSelectedTileType { get; private set; }

        private RichTextBox CurrentLogTextBox;

        public static string FilePathToMapDirectory;
        public static string FilePathToSelectedTexture;

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
            //new DUNG("");
            //return;
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(10, 0);

            this.AllowTransparency = true;
            Main = this;
        }

        private void DigimonWorld2ToolForm_Load(object sender, EventArgs e)
        {
            DungeonFilesComboBox.Items.Clear();
            CurrentLogTextBox = MapVisualizerLogRichTextBox;
            SetupLayoutRenderTabs();
            SetupEditorLayoutRenderTabs();

            PopulateComboBoxes();

            LoadUserSettings();
            LoadDungeonFiles();

            AddDungeonFilesToComboBox();
            MainTabControl.SelectedIndex = 0;
            DungeonFilesComboBox.SelectedIndex = 0;

            // We select anything non-start index here so the indexChanged gets fired on rendering the first layout
            MapLayoutsTabControl.SelectedIndex = MapLayoutsTabControl.TabCount;
            EditorLayoutRendererOld.SetupFloorLayerBitmap();
        }

        private void PopulateComboBoxes()
        {
            ErrorCheckingComboBox.Items.Add(Strictness.Strict);
            ErrorCheckingComboBox.Items.Add(Strictness.Sloppy);

            TextureTypeComboBox.Items.Add(TextureParser.TextureType.Generic);
            TextureTypeComboBox.Items.Add(TextureParser.TextureType.Model);
        }

        private void LoadUserSettings()
        {
            // Directories
            FilePathToMapDirectory = (string)Properties.Settings.Default["MapDataFolder"];
            if (FilePathToMapDirectory == "")
                FilePathToMapDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Maps\\";

            FilePathToMapDirectory = @"D:\Program Files (x86)\EmulatorsAndRomhacking\DigimonWorld2\AAA\4.AAA\DUNG\DUNG\";
            CurrentMapDataFolderLabel.Text = FilePathToMapDirectory;

            // Grid
            GridPosHexCheckBox.Checked = (bool)Properties.Settings.Default["ShowGridPosAsHex"];
            ShowGridCheckbox.Checked = (bool)Properties.Settings.Default["ShowGridLines"];
            TileSizeInput.Value = (int)Properties.Settings.Default["GridTileSize"];

            // Error checking
            ErrorCheckingComboBox.SelectedIndex = (int)Enum.Parse(typeof(Strictness), (string)Properties.Settings.Default["ErrorCheckingLevel"]);
            ShowLogsCheckBox.Checked = (bool)Properties.Settings.Default["ShowLogs"];

            // Visible map layers
            ShowWarpsCheckbox.Checked = (bool)Properties.Settings.Default["ShowWarpsLayer"];
            ShowTrapsCheckbox.Checked = (bool)Properties.Settings.Default["ShowTrapsLayer"];
            ShowChestsCheckbox.Checked = (bool)Properties.Settings.Default["ShowChestsLayer"];
            ShowDigimonCheckbox.Checked = (bool)Properties.Settings.Default["ShowDigimonLayer"];

            //Texture visualizer
            ScaleTextureToFitCheckbox.Checked = (bool)Properties.Settings.Default["ScaleTextureToFit"];
            TextureUseAltClutCheckbox.Checked = (bool)Properties.Settings.Default["TextureUseAltClutCheckbox"];
            CLUTFirstColourTransparantCheckbox.Checked = (bool)Properties.Settings.Default["CLUTFirstColourTransparantCheckbox"];
            InvertCLUTColoursCheckbox.Checked = (bool)Properties.Settings.Default["InvertCLUTColours"];
            TextureTypeComboBox.SelectedIndex = (int)Properties.Settings.Default["TextureType"];

            //Map editor
            EditorShowGridCheckbox.Checked = (bool)Properties.Settings.Default["EditorShowGridLines"];
            EditorTileSizeInput.Value = (int)Properties.Settings.Default["EditorGridTileSize"];
        }

        private void LoadDungeonFiles()
        {
            DungeonFiles.Clear();

            var filenames = Directory.GetFiles(FilePathToMapDirectory);
            foreach (var item in filenames)
            {
                var filename = item.Replace(FilePathToMapDirectory, "");
                DungFile domain = DungFile.VanillaDungeonFiles.FirstOrDefault(o => o.Filename == filename);

                if (domain == null)
                    domain = new DungFile(filename);

                DungeonFiles.Add(domain);
            }
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
                item.CursorLayer.MouseMove += DisplayMousePositionOnGrid;
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
            TileOverrideTypeLabel.Text = $"Tile type: {CurrentDomainFloor.FloorTypeOverride}";
        }

        public void SetCurrentObjectInformation(IFloorLayoutObject mapObject)
        {
            ResetCurrentObjectInformation();
            ObjectTypeLabel.Text = $"Type: {mapObject.ObjectType}";
            ObjectPositionLabel.Text = GridPosHexCheckBox.Checked ? $"Position: ({mapObject.Position.x:X2}, {mapObject.Position.y:X2})" : $"Position: {mapObject.Position}";

            switch (mapObject.ObjectType)
            {
                case IFloorLayoutObject.MapObjectType.Warp:
                    Warp warp = (Warp)mapObject;
                    ObjectSubTypeLabel.Text = $"Sub type: {warp.Type}";
                    break;
                case IFloorLayoutObject.MapObjectType.Chest:
                    Chest chest = (Chest)mapObject;
                    ObjectSlotOneLabel.Text = $"Slot 1: {chest.chestSlots[0].ItemName} - {chest.chestSlots[0].TrapLevel}";
                    ObjectSlotTwoLabel.Text = $"Slot 2: {chest.chestSlots[1].ItemName} - {chest.chestSlots[1].TrapLevel}";
                    ObjectSlotThreeLabel.Text = $"Slot 3: {chest.chestSlots[2].ItemName} - {chest.chestSlots[2].TrapLevel}";
                    ObjectSlotFourLabel.Text = $"Slot 4: {chest.chestSlots[3].ItemName} - {chest.chestSlots[3].TrapLevel}";
                    break;
                case IFloorLayoutObject.MapObjectType.Trap:
                    Trap trap = (Trap)mapObject;
                    ObjectSubTypeLabel.Text = $"Sub type: {trap.Type}";
                    ObjectSlotOneLabel.Text = $"Slot 1: {trap.TrapSlots[0]}";
                    ObjectSlotTwoLabel.Text = $"Slot 2: {trap.TrapSlots[1]}";
                    ObjectSlotThreeLabel.Text = $"Slot 3: {trap.TrapSlots[2]}";
                    ObjectSlotFourLabel.Text = $"Slot 4: {trap.TrapSlots[3]}";
                    break;
                case IFloorLayoutObject.MapObjectType.Digimon:
                    Digimon digimon = (Digimon)mapObject;
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
            switch (MainTabControl.SelectedIndex)
            {
                case 0:
                    CurrentLogTextBox = MapVisualizerLogRichTextBox;
                    break;

                case 1:
                    CurrentLogTextBox = TextureVisualizerLogRichTextBox;
                    break;
                default:
                    break;
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
                CurrentMapLayout = CurrentDomainFloor.UniqueDomainMapLayouts[0];
                DrawCurrentMapLayout();
                SetCurrentLayoutInformation();
            }

            for (int i = 0; i < 8; i++)
            {
                if (i < CurrentDomainFloor.UniqueDomainMapLayouts.Count)
                {
                    MapLayoutsTabControl.TabPages[i].Text = $"Layout{i}";
                }
                else
                {
                    MapLayoutsTabControl.TabPages[i].Text = "";
                }
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
            if (CurrentDomain == null)
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
            LayoutRendererOld.SetupFloorLayerBitmap();
            CurrentMapLayout.DrawLayout();
        }

        /// <summary>
        /// Display the current mouse position on the grid
        /// </summary>
        private void DisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            if (GridPosHexCheckBox.Checked)
                MousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / LayoutRendererOld.tileSize:X2} Y: {(int)e.Location.Y / LayoutRendererOld.tileSize:X2}";
            else
                MousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / LayoutRendererOld.tileSize:00} Y: {(int)e.Location.Y / LayoutRendererOld.tileSize:00}";
        }

        /// <summary>
        /// Update the per-tile size of the grid, and redraw the grid at this size.
        /// </summary>
        private void ResizeGridButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["GridTileSize"] = (int)TileSizeInput.Value;
            LayoutRendererOld.tileSize = (int)TileSizeInput.Value;
            DrawCurrentMapLayout();
            if (ShowGridCheckbox.Checked)
                LayoutRendererOld.DrawGrid();
        }

        private void ShowGridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowGridLines"] = ShowGridCheckbox.Checked;
            if (ShowGridCheckbox.Checked)
                LayoutRendererOld.DrawGrid();
            else
                LayoutRendererOld.HideGrid();
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
                    FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            LayoutRendererOld.combinedLayer.Save(fs, ImageFormat.Png);
                            break;

                        case 2:
                            LayoutRendererOld.combinedLayer.Save(fs, ImageFormat.Bmp);
                            break;
                    }

                    fs.Close();
                }
            }
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
                CurrentLogTextBox.SelectionColor = Color.Red;

                if (stackTrace)
                    CurrentLogTextBox.AppendText($"{errorWithStack}");
                else
                    CurrentLogTextBox.AppendText($"{error}{Environment.NewLine}");

                CurrentLogTextBox.SelectionColor = Color.White;
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
                CurrentLogTextBox.SelectionColor = Color.Yellow;

                if (stackTrace)
                    CurrentLogTextBox.AppendText($"{warningWithStack}");
                else
                    CurrentLogTextBox.AppendText($"{warning}{Environment.NewLine}");

                CurrentLogTextBox.SelectionColor = Color.White;
            }
            LogStack.Add(warningWithStack);
        }

        public void AddLogToLogWindow(object log)
        {
            CurrentLogTextBox.SelectionColor = Color.White;
            CurrentLogTextBox.AppendText($"{log}{Environment.NewLine}");

            LogStack.Add(log.ToString());
        }

        /// <summary>
        /// Make sure we automatically scroll to the bottom of the log
        /// </summary>
        private void LogRichTextBox_TextChanged(object sender, EventArgs e)
        {
            CurrentLogTextBox.SelectionStart = CurrentLogTextBox.Text.Length;
            CurrentLogTextBox.ScrollToCaret();
        }

        private void ShowLogsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowLogs"] = ShowLogsCheckBox.Checked;
        }

        private void SelectMapDataFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                folderBrowser.SelectedPath += "\\";
                Properties.Settings.Default["MapDataFolder"] = folderBrowser.SelectedPath;
                FilePathToMapDirectory = folderBrowser.SelectedPath;
                AddLogToLogWindow($"Changed map data directory to {FilePathToMapDirectory}");
                CurrentMapDataFolderLabel.Text = folderBrowser.SelectedPath;
            }

            DungeonFilesComboBox.Items.Clear();
            LoadDungeonFiles();
            AddDungeonFilesToComboBox();
        }

        private void ShowWarpsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = ShowWarpsCheckbox.Checked ? LayoutRendererOld.warpsLayer : new Bitmap(LayoutRendererOld.GetGridSizeScaled().x, LayoutRendererOld.GetGridSizeScaled().y);
            foreach (RenderLayoutTab tab in FloorLayoutRenderTabs)
                tab.WarpsRenderLayer.Image = bmp;

            Properties.Settings.Default["ShowWarpsLayer"] = ShowWarpsCheckbox.Checked;
        }

        private void ShowTrapsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = ShowTrapsCheckbox.Checked ? LayoutRendererOld.trapsLayer : new Bitmap(LayoutRendererOld.GetGridSizeScaled().x, LayoutRendererOld.GetGridSizeScaled().y);
            foreach (RenderLayoutTab tab in FloorLayoutRenderTabs)
                tab.TrapsRenderLayer.Image = bmp;

            Properties.Settings.Default["ShowTrapsLayer"] = ShowTrapsCheckbox.Checked;
        }

        private void ShowChestsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = ShowChestsCheckbox.Checked ? LayoutRendererOld.chestsLayer : new Bitmap(LayoutRendererOld.GetGridSizeScaled().x, LayoutRendererOld.GetGridSizeScaled().y);
            foreach (RenderLayoutTab tab in FloorLayoutRenderTabs)
                tab.ChestsRenderLayer.Image = bmp;

            Properties.Settings.Default["ShowChestsLayer"] = ShowTrapsCheckbox.Checked;
        }

        private void ShowDigimonCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap bmp = ShowDigimonCheckbox.Checked ? LayoutRendererOld.digimonLayer : new Bitmap(LayoutRendererOld.GetGridSizeScaled().x, LayoutRendererOld.GetGridSizeScaled().y);
            foreach (RenderLayoutTab tab in FloorLayoutRenderTabs)
                tab.DigimonRenderLayer.Image = bmp;

            Properties.Settings.Default["ShowDigimonLayer"] = ShowDigimonCheckbox.Checked;
        }

        #endregion

        #region TextureVisualizerTab
        private void SelectTextureButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedTextureLabel.Text = fileDialog.FileName;
                FilePathToSelectedTexture = fileDialog.FileName;

                TextureParser.CheckForTIMHeader(fileDialog.FileName);
            }
        }

        private void ReloadTextureButton_Click(object sender, EventArgs e)
        {
            TextureParser.CheckForTIMHeader(SelectedTextureLabel.Text);
        }

        private void ScaleTextureToFitCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ScaleTextureToFit"] = ScaleTextureToFitCheckbox.Checked;
        }

        private void TextureUseAltClutCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["TextureUseAltClutCheckbox"] = TextureUseAltClutCheckbox.Checked;
        }

        private void CLUTFirstColourTransparantCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["CLUTFirstColourTransparantCheckbox"] = CLUTFirstColourTransparantCheckbox.Checked;
        }

        private void InvertCLUTColoursCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["InvertCLUTColours"] = InvertCLUTColoursCheckbox.Checked;
        }

        public void SetSegmentInformationText(int pointer, short positionX, short positionY, byte offsetX, byte offsetY, short unknown, byte Width, byte Height, short colour)
        {
            TextureSegmentPointerLabel.Text = $"Segment pointer: 0x{pointer:X8}";
            TextureSegmentOffsetXLabel.Text = $"Offset X: 0x{offsetX:X2}";
            TextureSegmentOffsetYLabel.Text = $"Offset Y: 0x{offsetY:X2}";
            TextureSegmentPositionXLabel.Text = $"Position X: 0x{positionX:X4}";
            TextureSegmentPositionYLabel.Text = $"Position Y: 0x{positionY:X4}";
            TextureSegmentUnknownLabel.Text = $"Unknown: 0x{unknown:X4}";
            TextureSegmentWidthLabel.Text = $"Width: 0x{Width:X2}";
            TextureSegmentHeightLabel.Text = $"Height: 0x{Height:X2}";
            TextureSegmentColourLabel.Text = $"Colour?: 0x{colour:X4}";
        }

        private void TextureSegmentSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TextureParser.CurrentTexture == null)
                return;

            var currentSegment = TextureParser.CurrentTexture.TextureHeader.TextureSegments[TextureSegmentSelectComboBox.SelectedIndex];

            TextureLayerSelectComboBox.Items.Clear();
            for (int i = 0; i < currentSegment.Layers.Count; i++)
            {
                TextureLayerSelectComboBox.Items.Add(i);
            }

            TextureLayerSelectComboBox.SelectedIndex = 0;
        }

        private void TextureLayerSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentSegment = TextureParser.CurrentTexture.TextureHeader.TextureSegments[TextureSegmentSelectComboBox.SelectedIndex];
            currentSegment.Layers[TextureLayerSelectComboBox.SelectedIndex].DrawInformationToInformationBox(currentSegment.SegmentOffset);

            DrawTextureSegment();
        }

        private void DrawTextureSegment()
        {
            var bmp = TextureParser.CreateTextureSegmentBMP();

            if (bmp != null)
                TextureSegmentPictureBox.Image = bmp;
        }

        private void TextureVisualizerLogRichTextBox_TextChanged(object sender, EventArgs e)
        {
            TextureVisualizerLogRichTextBox.SelectionStart = TextureVisualizerLogRichTextBox.Text.Length;
            TextureVisualizerLogRichTextBox.ScrollToCaret();
        }

        private void TextureTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["TextureType"] = TextureTypeComboBox.SelectedIndex;
        }
        #endregion

        #region Digitext parser
        private void SelectDialogueFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.RestoreDirectory = true;

                //fd.InitialDirectory = @"D:\Program Files (x86)\ePSXe\Games\DigimonWorld2\Extracted\AAA\4.AAA";
                fd.Filter = "Bin files (.bin)|*.bin";
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    var filePath = fd.FileName;

                    var fileStream = fd.OpenFile();

                    byte[] arr;
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        using MemoryStream memoryStream = new MemoryStream();
                        reader.BaseStream.CopyTo(memoryStream);
                        arr = memoryStream.ToArray();
                    }

                    string result = $"File: {filePath}\n\n";
                    result += TextConversion.MessageFileToString(arr);


                    DialogueOutputRichTextbox.Text = result;
                }
            }
        }

        private void ExportAllMessFilesButton_Click(object sender, EventArgs e)
        {
            string targetDirCity = $"{AppDomain.CurrentDomain.BaseDirectory}Output\\MESS\\City";
            if (!Directory.Exists(targetDirCity))
                Directory.CreateDirectory(targetDirCity);

            string targetDirDung = $"{AppDomain.CurrentDomain.BaseDirectory}Output\\MESS\\Dung";
            if (!Directory.Exists(targetDirDung))
                Directory.CreateDirectory(targetDirDung);

            string sourceDirCityMess = @"D:\Program Files (x86)\EmulatorsAndRomhacking\ePSXe\Games\DigimonWorld2\Extracted\AAA\4.AAA\CITY\MESS";
            string sourceDirDungMess = @"D:\Program Files (x86)\EmulatorsAndRomhacking\ePSXe\Games\DigimonWorld2\Extracted\AAA\4.AAA\DUNG\MESS";

            foreach (var file in Directory.GetFiles(sourceDirCityMess))
            {
                byte[] arr;
                using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    reader.BaseStream.CopyTo(memoryStream);
                    arr = memoryStream.ToArray();
                }

                var result = TextConversion.MessageFileToString(arr);

                string filename = file.Substring(file.LastIndexOf("\\"), file.Length - file.LastIndexOf("\\"));
                filename = filename.Replace(".BIN", ".txt");
                string targetPath = targetDirCity + filename;

                File.WriteAllText(targetPath, result);
            }

            foreach (var file in Directory.GetFiles(sourceDirDungMess))
            {
                byte[] arr;
                using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    reader.BaseStream.CopyTo(memoryStream);
                    arr = memoryStream.ToArray();
                }

                var result = TextConversion.MessageFileToString(arr);

                string filename = file.Substring(file.LastIndexOf("\\"), file.Length - file.LastIndexOf("\\"));
                filename = filename.Replace(".BIN", ".txt");
                string targetPath = targetDirDung + filename;

                File.WriteAllText(targetPath, result);
            }
        }
        #endregion

        #region LayoutEditor

        /// <summary>
        /// Add each render tab to the array of possible render tabs
        /// </summary>
        private void SetupEditorLayoutRenderTabs()
        {
            EditorLayoutRenderTab = EditorFloorLayoutRenderTabs[0] = EditorRenderLayoutTab0;
            EditorFloorLayoutRenderTabs[1] = EditorRenderLayoutTab1;
            EditorFloorLayoutRenderTabs[2] = EditorRenderLayoutTab2;
            EditorFloorLayoutRenderTabs[3] = EditorRenderLayoutTab3;
            EditorFloorLayoutRenderTabs[4] = EditorRenderLayoutTab4;
            EditorFloorLayoutRenderTabs[5] = EditorRenderLayoutTab5;
            EditorFloorLayoutRenderTabs[6] = EditorRenderLayoutTab6;
            EditorFloorLayoutRenderTabs[7] = EditorRenderLayoutTab7;

            // Add the function for displaying the current mouse position on the grid to the MouseMove event
            foreach (var item in EditorFloorLayoutRenderTabs)
            {
                item.CursorLayer.MouseMove += EditorDisplayMousePositionOnGrid;
            }
        }

        private void EditorShowGridCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["EditorShowGridLines"] = EditorShowGridCheckbox.Checked;
            if (EditorShowGridCheckbox.Checked)
                EditorLayoutRendererOld.DrawGrid();
            else
                EditorLayoutRendererOld.HideGrid();
        }

        private void EditorResizeGridButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["EditorGridTileSize"] = (int)EditorTileSizeInput.Value;
            EditorLayoutRendererOld.tileSize = (int)EditorTileSizeInput.Value;
            //DrawCurrentMapLayout();
            if (EditorShowGridCheckbox.Checked)
                EditorLayoutRendererOld.DrawGrid();
        }

        /// <summary>
        /// Display the current mouse position on the grid
        /// </summary>
        private void EditorDisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            if (EditorGridPosHexCheckbox.Checked)
                EditorMousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / EditorLayoutRendererOld.tileSize:X2} Y: {(int)e.Location.Y / EditorLayoutRendererOld.tileSize:X2}";
            else
                EditorMousePositionOnGridLabel.Text = $"X: {(int)e.Location.X / EditorLayoutRendererOld.tileSize:00} Y: {(int)e.Location.Y / EditorLayoutRendererOld.tileSize:00}";
        }

        /// <summary>
        /// Call the renderer for the current floor's current layout tab
        /// </summary>
        /// <param name="mapLayoutIndex"></param>
        private void EditorDrawCurrentMapLayout(object sender, EventArgs e)
        {
            EditorLayoutRendererOld.SetupFloorLayerBitmap();
        }

        private void EditorReloadLayout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the current layout?\nThis action can not be reversed.", "Confirm deletion",
                                MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            EditorLayoutRendererOld.tiles = null;
            EditorLayoutRendererOld.SetupFloorLayerBitmap();
        }

        private void EditorTileTypeButton_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            string tileName = pictureBox.Name.Replace("TileTypePictureBox", "");
            EditorSelectedTileType = (Tile.DomainTileTypeOld)Enum.Parse(typeof(Tile.DomainTileTypeOld), tileName);
            PlaceModeCheckbox.Checked = true;

            EditorSelectedTileTypePicturebox.Location = new Point(pictureBox.Location.X - 3, pictureBox.Location.Y - 3);
        }

        private void EditorSaveLayoutToFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Binaries| *.bin",
                Title = "Save current layout to file"
            };

            saveFileDialog.FileName = $"DUNG";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FileName != "")
                {
                    FileStream fs = (FileStream)saveFileDialog.OpenFile();

                    byte[] bytes = new byte[EditorLayoutRendererOld.tiles.Length];
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        bytes[i] = EditorLayoutRendererOld.tiles[i].TileValueDec;
                    }
                    fs.Write(bytes, 0, bytes.Length);

                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        private void EditorLoadLayoutButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Binaries | *.bin",
                Title = "Open custom dungeon layout",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream fs = openFileDialog.OpenFile();
                byte[] data = new byte[1536];
                fs.Read(data, 0, 1536);

                for (int i = 0; i < data.Length; i++)
                {
                    Vector2 pos = new Vector2(i % 32, (int)Math.Floor((double)i / 32)); //32 is the width of the grid
                    pos.x *= 2;

                    Tile.DomainTileTypeOld tileType = Tile.DomainTileTypeOld.Empty;
                    tileType = (Tile.DomainTileTypeOld)data[i].GetRightNiblet();
                    EditorLayoutRendererOld.UpdateTile(pos, tileType);

                    pos += Vector2.Right;
                    tileType = (Tile.DomainTileTypeOld)data[i].GetLeftNiblet();
                    EditorLayoutRendererOld.UpdateTile(pos, tileType);

                }

                fs.Close();
                fs.Dispose();
            }

        }

        private void EditorGridPosHexCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["EditorShowGridPosAsHex"] = EditorGridPosHexCheckbox.Checked;
        }
        #endregion

        #region TIM Injector

        private void InjectorSelectTimButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "(bin)|*.bin";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Debug.WriteLine($"opening {fileDialog.FileName}");

                TextureParser.CheckForTIMHeader(fileDialog.FileName);
                var selectedBmp = TextureParser.CurrentBitmap;
                InjectTimPreviewPictureBox.Image = selectedBmp;
            }
        }


        private void SelectBmpInjectButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "(bmp)|*.bmp";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                byte[] data = File.ReadAllBytes(filePath);

                BmpFile bmpFile = new BmpFile(data);

                InitPictureBox(bmpFile);
                DrawBmp(bmpFile);
            }
        }

        private void InitPictureBox(BmpFile bmpFile)
        {
            BmpInjectPreviewPictureBox.Image?.Dispose();
            BmpInjectPreviewPictureBox.Width = bmpFile.DetailInfoHeader.bV4Width;
            BmpInjectPreviewPictureBox.Height = bmpFile.DetailInfoHeader.bV4Height;
        }

        private void DrawBmp(BmpFile bmpFile)
        {
            Bitmap bmp = new Bitmap(bmpFile.DetailInfoHeader.bV4Width, bmpFile.DetailInfoHeader.bV4Height);

            //Because a single byte defines two pixel colours l/r the actual width of a data row is half the image width
            int actualWidth = bmpFile.DetailInfoHeader.bV4Width / 2;

            for (int y = 0; y < bmpFile.DetailInfoHeader.bV4Height; y++)
            {
                for (int x = 0; x < actualWidth - 1; x++)
                {
                    var index = x + y * actualWidth;
                    var pixelValue = bmpFile.PixelData[index];

                    var rightByte = pixelValue.GetRightNiblet();
                    var leftByte = pixelValue.GetLeftNiblet();

                    var rightPixelCol = bmpFile.Clut[rightByte];
                    var leftPixelCol = bmpFile.Clut[leftByte];

                    var actualX = x * 2;
                    var actualY = (bmpFile.DetailInfoHeader.bV4Height - 1) - y;
                    bmp.SetPixel(actualX, actualY, leftPixelCol);
                    bmp.SetPixel(actualX + 1, actualY, rightPixelCol);
                }
            }
            BmpInjectPreviewPictureBox.Image = bmp;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = SelectedTextureLabel.Text;
            var lastSlash = filePath.LastIndexOf('\\');
            var fileNAme = filePath.Substring(lastSlash + 1, filePath.Length - lastSlash -1);
            fileNAme = fileNAme.Replace(".BIN", ".png");
            var targetDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase) + "\\textures\\";
            targetDir = targetDir[6..];
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            var finalDest = $"{targetDir}{fileNAme}";
            Debug.WriteLine("Output:" + finalDest);
            SelectedTextureRenderLayer.Image.Save(finalDest, ImageFormat.Png);
        }
    }
}