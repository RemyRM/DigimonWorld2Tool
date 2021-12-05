using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.FileEditor;
using DigimonWorld2Tool.FileInterpreter;

namespace DigimonWorld2Tool.Views
{
    public partial class DungWindow : UserControl, IHostWindow
    {
        #region enums
        public enum SelectedMapObjectType
        {
            Warp,
            Chest,
            Trap,
            Digimon
        }
        #endregion

        #region properties
        public static DungWindow Instance { get; private set; }

        private string dungFileDir;
        public string DungFileDir
        {
            get => dungFileDir;
            private set
            {
                dungFileDir = value;

                string labelText = value;
                if (labelText.Length > 80)
                {
                    int characterCountToCull = labelText.Length - 80;
                    labelText = labelText.Remove(10, characterCountToCull);
                    labelText = labelText.Insert(10, @"\...\");
                }
                DungDirLabel.Text = labelText;
            }
        }

        private string[] dungFileNames;
        public string[] DungFileNames
        {
            get => dungFileNames;
            private set
            {
                dungFileNames = value;
                SelectDungComboBox.DataSource = dungFileNames;
            }
        }

        private byte[] loadedDungFileRawData;
        public byte[] LoadedDungFileRawData
        {
            get => loadedDungFileRawData;
            private set => loadedDungFileRawData = value;
        }

        private DUNG loadedDungFile;
        public DUNG LoadedDungFile
        {
            get => loadedDungFile;
            set => loadedDungFile = value;
        }

        private DungFloorHeader loadedDungFloorHeader;
        public DungFloorHeader LoadedDungFloorHeader
        {
            get => loadedDungFloorHeader;
            set => loadedDungFloorHeader = value;
        }

        private DungFloorLayoutHeader loadedDungFloorLayout;
        public DungFloorLayoutHeader LoadedDungFloorLayout
        {
            get => loadedDungFloorLayout;
            set => loadedDungFloorLayout = value;
        }
        private int LoadedDungFloorLayoutIndex { get; set; }

        private DUNGInterpreter dungInterpreter;
        public DUNGInterpreter DungInterpreter
        {
            get => dungInterpreter;
            private set => dungInterpreter = value;
        }

        private Button[] FloorLayoutButtons { get; set; } = new Button[8];
        private Button LastPressedFloorLayoutButton { get; set; }

        private DUNGLayoutRenderer.TileType SelectedTileTypeToPaint { get; set; }
        private DUNGEditor DungEditor { get; set; }

        private EnemySetHeader[] possibleDigimonSets;

        private Vector2 SelectedObjectPosition { get; set; }
        private SelectedMapObjectType SelectedObjectType { get; set; }

        private string SelectedDungFilePath { get; set; }
        #endregion

        #region initialization
        public DungWindow()
        {
            Instance = this;
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;

            FloorLayoutButtons[0] = FloorLayoutButton1;
            FloorLayoutButtons[1] = FloorLayoutButton2;
            FloorLayoutButtons[2] = FloorLayoutButton3;
            FloorLayoutButtons[3] = FloorLayoutButton4;
            FloorLayoutButtons[4] = FloorLayoutButton5;
            FloorLayoutButtons[5] = FloorLayoutButton6;
            FloorLayoutButtons[6] = FloorLayoutButton7;
            FloorLayoutButtons[7] = FloorLayoutButton8;

            //Warm up some settings files
            _ = new DUNGLayoutRenderer(FloorLayoutPictureBox);
            _ = Settings.Settings.DIGIMNDTFile;
            _ = Settings.Settings.ENEMYSETFile;
            _ = Settings.Settings.MODELDT0File;
            _ = Settings.Settings.ITEMDATAFile;

            LoadUserSettings();
            ColourTheme.SetColourScheme(this.Controls);
            ColourTheme.SetColourScheme(ObjectInfoGroupBox.Controls);
            ColourTheme.SetColourScheme(FloorHeaderInfoGroupBox.Controls);
            ColourTheme.SetColourScheme(LayoutHeaderGroupbox.Controls);

            WarpsPtrNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            TrapsPtrNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            DigimonPtrNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];
            ChestsPtrNumericUpDown.Hexadecimal = (bool)Properties.Settings.Default["ShowValuesAsHex"];

            PostInit();
            SetupEvents();
        }

        private void PostInit()
        {
            if (DungFileDir != "")
            {
                DungFileNames = GetFileNamesInDungDirectory();
                PopulateDomainNamesComboBox(SelectDungComboBox);
            }
        }

        private void LoadUserSettings()
        {
            DungFileDir = (string)Properties.Settings.Default["DefaultMapDataDirectory"];
        }
        #endregion

        #region events
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && !Disposing)
                SetupEvents();
            else if (!Visible)
                CleanUpEvents();
        }

        private void SetupEvents()
        {
            MainWindow.EditModeChanged += OnEditModeChanged;
        }

        private void CleanUpEvents()
        {
            MainWindow.EditModeChanged -= OnEditModeChanged;
        }

        public void OnWindowResizeEnded()
        {
            FloorLayoutButton_Click(LastPressedFloorLayoutButton, null);
        }

        private void OnEditModeChanged(bool enabled)
        {
            CorridorTileTypePictureBox.Visible = enabled;
            RoomTileTypePictureBox.Visible = enabled;
            FireTileTypePictureBox.Visible = enabled;
            NatureTileTypePictureBox.Visible = enabled;
            WaterTileTypePictureBox.Visible = enabled;
            MachineTileTypePictureBox.Visible = enabled;
            DarkTileTypePictureBox.Visible = enabled;
            EmptyTileTypePictureBox.Visible = enabled;
            EditorSelectedTileTypePicturebox.Visible = enabled;
            EditObjectInfoButton.Visible = enabled;
            SaveChangesButton.Visible = enabled;
            FloorHeaderInfoGroupBox.Visible = enabled;
            LayoutHeaderGroupbox.Visible = enabled;

            if (enabled)
            {
                OnEditorTileTypeClick(EmptyTileTypePictureBox, null);
                DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, SelectDungFloorComboBox.SelectedIndex, LoadedDungFloorLayoutIndex);
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDUNGData.DungFloorHeaders[SelectDungFloorComboBox.SelectedIndex].DungFloorLayoutHeaders[LoadedDungFloorLayoutIndex]);
            }
            else
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(LoadedDungFloorLayout);

            DUNGLayoutRenderer.Instance.SetupDungFloorBitmap();
            DUNGLayoutRenderer.Instance.DrawDungFloorLayout();
        }
        #endregion

        #region UserInput
        private void OnEditorTileTypeClick(object sender, EventArgs e)
        {
            PictureBox clickedBox = (PictureBox)sender;
            string tileName = clickedBox.Name.Replace("TileTypePictureBox", "");
            var clickedTile = (DUNGLayoutRenderer.TileType)Enum.Parse(typeof(DUNGLayoutRenderer.TileType), tileName);
            if (SelectedTileTypeToPaint == clickedTile)
            {
                SelectedTileTypeToPaint = DUNGLayoutRenderer.TileType.None;
                EditorSelectedTileTypePicturebox.Visible = false;
            }
            else
            {
                SelectedTileTypeToPaint = clickedTile;
                EditorSelectedTileTypePicturebox.Location = new Point(clickedBox.Location.X - 3, clickedBox.Location.Y - 3);
                EditorSelectedTileTypePicturebox.Visible = true;
            }
        }

        /// <summary>
        /// Select the directory in which the dung files are stored
        /// </summary>
        private void SelectDungDirButton_Click(object sender, EventArgs e)
        {
            DungFileDir = GetDungFilesDirectory();
            DungFileNames = GetFileNamesInDungDirectory();
        }

        /// <summary>
        /// Select the clicked entry in the combo box as file to load
        /// </summary>
        private void SelectDungComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDomainNamesComboBox((ComboBox)sender);

            if (MainWindow.EditModeEnabled)
                DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, SelectDungFloorComboBox.SelectedIndex, LoadedDungFloorLayoutIndex);
            else
                DungEditor = null;
        }

        private void SelectDungFloorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cBox = (ComboBox)sender;
            LoadSelectedFloorData((string)cBox.SelectedItem);
            DungInterpreter.UpdateDungFloor(LoadedDungFloorHeader);
            SetFloorHeaderData();

            FloorLayoutButton_Click(FloorLayoutButton1, null);
        }

        private void FloorLayoutButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (LastPressedFloorLayoutButton != null)
                LastPressedFloorLayoutButton.BackColor = (Color)Settings.Settings.ButtonBackgroundColour;

            button.BackColor = (Color)Settings.Settings.ButtonSelectedBackgroundColour;
            LastPressedFloorLayoutButton = button;

            var buttonId = FloorLayoutButtons.ToList().IndexOf(button);
            LoadedDungFloorLayout = LoadedDungFloorHeader.DungFloorLayoutHeaders[buttonId];
            LoadedDungFloorLayoutIndex = buttonId;

            if (LoadedDungFloorLayout == null)
                throw new NullReferenceException();

            if (MainWindow.EditModeEnabled)
            {
                DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, SelectDungFloorComboBox.SelectedIndex, LoadedDungFloorLayoutIndex);
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDUNGData.DungFloorHeaders[SelectDungFloorComboBox.SelectedIndex].DungFloorLayoutHeaders[LoadedDungFloorLayoutIndex]);
            }
            else
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(LoadedDungFloorLayout);

            DUNGLayoutRenderer.Instance.SetupDungFloorBitmap();
            DUNGLayoutRenderer.Instance.DrawDungFloorLayout();

            WarpsPtrNumericUpDown.Value = loadedDungFloorLayout.FloorLayoutWarpsPointer;
            TrapsPtrNumericUpDown.Value = loadedDungFloorLayout.FloorLayoutTrapsPointer;
            DigimonPtrNumericUpDown.Value = loadedDungFloorLayout.FloorLayoutDigimonsPointer;
            ChestsPtrNumericUpDown.Value = loadedDungFloorLayout.FloorLayoutChestsPointer;
        }

        private void DrawGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FloorLayoutButton_Click(LastPressedFloorLayoutButton, null);
        }

        private void DisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            if (MainWindow.EditModeEnabled && e.Button == MouseButtons.Left && SelectedTileTypeToPaint != DUNGLayoutRenderer.TileType.None)
                PaintSelectedTileType(e.Location);

            if (loadedDungFile == null)
                return;

            MousePositionLabel.Text = $"X: {(e.Location.X / DUNGLayoutRenderer.Instance.TileSizeWidth).ToString(Settings.Settings.ValueTextFormat)} Y: {(e.Location.Y / DUNGLayoutRenderer.Instance.TileSizeHeight).ToString(Settings.Settings.ValueTextFormat)}";
        }

        private void FloorLayoutPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            var clickX = (int)e.Location.X / DUNGLayoutRenderer.Instance.TileSizeWidth;
            var clickY = (int)e.Location.Y / DUNGLayoutRenderer.Instance.TileSizeHeight;

            if (MainWindow.EditModeEnabled && SelectedTileTypeToPaint != DUNGLayoutRenderer.TileType.None)
            {
                DungEditor.ChangeTileTypeAtIndex(clickX, clickY, SelectedTileTypeToPaint);
                return;
            }

            foreach (var trap in LoadedDungFloorLayout.FloorLayoutTraps)
            {
                DungFloorTrap selectredTrap = null;
                if (trap.X == clickX && trap.Y == clickY)
                    selectredTrap = trap;

                if (selectredTrap == null)
                    continue;

                TypeLabel.Text = $"Type: Trap";
                SubTypeLabel.Text = "";
                PositionLabel.Text = $"Position: {trap.X.ToString(Settings.Settings.ValueTextFormat)}, {trap.Y.ToString(Settings.Settings.ValueTextFormat)}";

                SlotOneLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[0]).Type)}, Level: {DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[0]).Level}";
                SlotTwoLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[1]).Type)}, Level: {DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[1]).Level}";
                SlotThreeLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[2]).Type)}, Level: {DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[2]).Level}";
                SlotFourLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[3]).Type)}, Level: {DUNGInterpreter.GetTrapTypeAndLevelFromData(trap.TypeAndLevelData[3]).Level}";

                SelectedObjectPosition = new Vector2(trap.X, trap.Y);
                SelectedObjectType = SelectedMapObjectType.Trap;
                ShoowHideDigiInfoButtons(false);
            }

            foreach (var warp in LoadedDungFloorLayout.FloorLayoutWarps)
            {
                DungFloorWarp selectedWarp = null;
                if (warp.X == clickX && warp.Y == clickY)
                    selectedWarp = warp;

                if (selectedWarp == null)
                    continue;

                TypeLabel.Text = $"Type: Warp";
                SubTypeLabel.Text = $"Sub type: {DUNGInterpreter.GetWarpType(warp.Type)}";
                PositionLabel.Text = $"Position: {warp.X.ToString(Settings.Settings.ValueTextFormat)}, {warp.Y.ToString(Settings.Settings.ValueTextFormat)}";
                SlotOneLabel.Text = "";
                SlotTwoLabel.Text = "";
                SlotThreeLabel.Text = "";
                SlotFourLabel.Text = "";

                SelectedObjectPosition = new Vector2(warp.X, warp.Y);
                SelectedObjectType = SelectedMapObjectType.Warp;

                ShoowHideDigiInfoButtons(false);
            }

            foreach (var digimon in LoadedDungFloorLayout.FloorLayoutDigimons)
            {
                DungFloorDigimon selectedDigimon = null;
                if (digimon.X == clickX && digimon.Y == clickY)
                    selectedDigimon = digimon;

                if (selectedDigimon == null)
                    continue;

                TypeLabel.Text = $"Type: Digimon";
                SubTypeLabel.Text = "";
                PositionLabel.Text = $"Position: {digimon.X.ToString(Settings.Settings.ValueTextFormat)}, {digimon.Y.ToString(Settings.Settings.ValueTextFormat)}";

                possibleDigimonSets = DUNGInterpreter.GetDigimonSetHeaders(digimon.DigimonPackIndex);
                string[] digimonNames = new string[4];
                for (int i = 0; i < digimonNames.Length; i++)
                {
                    if (possibleDigimonSets[i] == null)
                        digimonNames[i] = "No spawn";
                    else
                    {
                        var digiID = possibleDigimonSets[i].DigimonInSet[0].DigimonID;
                        var digi = Settings.Settings.MODELDT0File.GetDigimonByDigimonID(digiID);
                        var nameData = digi.NameData;
                        digimonNames[i] = TextConversion.DigiStringToASCII(nameData);
                    }
                }

                SlotOneLabel.Text = digimonNames[0];
                SlotTwoLabel.Text = digimonNames[1];
                SlotThreeLabel.Text = digimonNames[2];
                SlotFourLabel.Text = digimonNames[3];

                SelectedObjectPosition = new Vector2(digimon.X, digimon.Y);
                SelectedObjectType = SelectedMapObjectType.Digimon;
                ShoowHideDigiInfoButtons(true);
            }

            foreach (var treasure in LoadedDungFloorLayout.FloorLayoutChests)
            {
                DungFloorChest selectedChest = null;
                if (treasure.X == clickX && treasure.Y == clickY)
                    selectedChest = treasure;

                if (selectedChest == null)
                    continue;

                TypeLabel.Text = "Treasure";
                SubTypeLabel.Text = "";
                PositionLabel.Text = $"Position: {treasure.X.ToString(Settings.Settings.ValueTextFormat)}, {treasure.Y.ToString(Settings.Settings.ValueTextFormat)}";

                string[] treasures = new string[4];
                for (int i = 0; i < treasures.Length; i++)
                {
                    if (treasure.ItemSlots[i] == 0)
                        treasures[i] = "No chest";
                    else
                    {
                        var treasureIndex = treasure.ItemSlots[i];
                        var treasureData = LoadedDungFloorHeader.FloorTreasureTable[treasureIndex - 1];

                        var itemID = treasureData[0];
                        var trapLevel = treasureData[1];
                        //var itemID = treasureData.ItemID;
                        //var trapLevel = treasureData.TrapLevel;

                        if (itemID == 0x00)
                        {
                            treasures[i] = $"Trap lv: {trapLevel} - Empty";
                            continue;
                        }

                        var itemData = Settings.Settings.ITEMDATAFile.ItemData.FirstOrDefault(o => o.ID == itemID);
                        treasures[i] = $"Trap lv: {trapLevel} - {TextConversion.DigiStringToASCII(itemData.NameData)}";
                    }
                }

                SlotOneLabel.Text = treasures[0];
                SlotTwoLabel.Text = treasures[1];
                SlotThreeLabel.Text = treasures[2];
                SlotFourLabel.Text = treasures[3];

                SelectedObjectPosition = new Vector2(treasure.X, treasure.Y);
                SelectedObjectType = SelectedMapObjectType.Chest;

                ShoowHideDigiInfoButtons(false);
            }
        }

        private void FloorLayoutPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            DUNGLayoutRenderer.Instance.DrawGridLayout();
        }

        private void SlotOneInfoPictureBox_Click(object sender, EventArgs e) => ShowExtendedDigimonPackInfo(0);
        private void SlotTwoInfoPictureBox_Click(object sender, EventArgs e) => ShowExtendedDigimonPackInfo(1);
        private void SlotThreeInfoPictureBox_Click(object sender, EventArgs e) => ShowExtendedDigimonPackInfo(2);
        private void SlotFourInfoPictureBox_Click(object sender, EventArgs e) => ShowExtendedDigimonPackInfo(3);
        private void ShowExtendedDigimonPackInfo(int SlotID)
        {
            if (possibleDigimonSets[SlotID] == null)
                return;

            byte digimonSetIndex = possibleDigimonSets[SlotID].ID;
            ExtendedEnemySetInfoWindow ext = new ExtendedEnemySetInfoWindow(digimonSetIndex);
            ext.Show();
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            DungEditor.SaveFileData();
        }

        private void EditObjectInfoButton_Click(object sender, EventArgs e)
        {
            if (MainWindow.EditModeEnabled)
                DungEditor.EditObjectData(SelectedObjectPosition.x, SelectedObjectPosition.y, SelectedObjectType);
        }

        private void EditFloorHeaderButton_Click(object sender, EventArgs e)
        {
            if (MainWindow.EditModeEnabled)
                DungEditor.EditFloorHeaderData();
        }

        private void WarpsPtrNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown input = (NumericUpDown)sender;
            if (DungEditor == null)
                return;
            DungEditor.UpdateWarpDataPointers((int)input.Value);
        }

        private void ChestsPtrNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown input = (NumericUpDown)sender;
            if (DungEditor == null)
                return;
            DungEditor.UpdateChestDataPointers((int)input.Value);
        }

        private void TrapsPtrNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown input = (NumericUpDown)sender;
            if (DungEditor == null)
                return;
            DungEditor.UpdateTrapDataPointers((int)input.Value);
        }

        private void DigimonPtrNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown input = (NumericUpDown)sender;
            if (DungEditor == null)
                return;
            DungEditor.UpdateDigimonDataPointers((int)input.Value);
        }
        #endregion

        #region RenderLayout
        private void PaintSelectedTileType(Point mousePos)
        {
            var clickX = (int)mousePos.X / DUNGLayoutRenderer.Instance.TileSizeWidth;
            var clickY = (int)mousePos.Y / DUNGLayoutRenderer.Instance.TileSizeHeight;

            DungEditor.ChangeTileTypeAtIndex(clickX, clickY, SelectedTileTypeToPaint);
        }
        #endregion


        private string GetDungFilesDirectory()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.ShowNewFolderButton = false;
                folderDialog.RootFolder = Environment.SpecialFolder.Personal;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default["DefaultMapDataDirectory"] = folderDialog.SelectedPath;
                    return folderDialog.SelectedPath;
                }
                else
                {
                    return (string)Properties.Settings.Default["DefaultMapDataDirectory"];
                }
            }
        }

        /// <summary>
        /// Get all the filenames of the files contained in <see cref="DungFileDir"/>
        /// </summary>
        private string[] GetFileNamesInDungDirectory()
        {
            if (DungFileDir == null || !Directory.Exists(DungFileDir))
                return null;

            //Filter out the generated backup files that end with ".bak"
            string[] filePaths = Directory.GetFiles(DungFileDir).Where(o => !o.Contains(".bak")).ToArray();
            string[] fileNames = new string[filePaths.Length];

            for (int i = 0; i < fileNames.Length; i++)
                fileNames[i] = filePaths[i].Replace($"{DungFileDir}\\", "");

            string[] mappedFileNames = new string[fileNames.Length];
            for (int i = 0; i < mappedFileNames.Length; i++)
            {
                var result = Settings.Settings.DungMapping.FirstOrDefault(o => o.Filename == fileNames[i]);
                if (result == null)
                    mappedFileNames[i] = fileNames[i];
                else
                    mappedFileNames[i] = result.DomainName;
            }

            return mappedFileNames;
        }

        private void PopulateDomainNamesComboBox(ComboBox comboBox)
        {
            string selectedDomainName = (string)comboBox.SelectedItem;
            string selectedFileName = Settings.Settings.DungMapping.FirstOrDefault(o => o.DomainName == selectedDomainName)?.Filename;
            selectedFileName ??= selectedDomainName;

            LoadDungFileRawData(selectedFileName);
        }


        public void SetFloorHeaderData()
        {
            DigimonPack0Label.Text = $"0) {LoadedDungFloorHeader.DigimonEncounterTable[0].ToString(Settings.Settings.ValueTextFormat)}";
            DigimonPack1Label.Text = $"1) {LoadedDungFloorHeader.DigimonEncounterTable[1].ToString(Settings.Settings.ValueTextFormat)}";
            DigimonPack2Label.Text = $"2) {LoadedDungFloorHeader.DigimonEncounterTable[2].ToString(Settings.Settings.ValueTextFormat)}";
            DigimonPack3Label.Text = $"3) {LoadedDungFloorHeader.DigimonEncounterTable[3].ToString(Settings.Settings.ValueTextFormat)}";

            ChestContentData0Label.Text = $"0) {LoadedDungFloorHeader.FloorTreasureTable[0][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[0][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData1Label.Text = $"1) {LoadedDungFloorHeader.FloorTreasureTable[1][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[1][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData2Label.Text = $"2) {LoadedDungFloorHeader.FloorTreasureTable[2][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[2][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData3Label.Text = $"3) {LoadedDungFloorHeader.FloorTreasureTable[3][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[3][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData4Label.Text = $"4) {LoadedDungFloorHeader.FloorTreasureTable[4][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[4][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData5Label.Text = $"5) {LoadedDungFloorHeader.FloorTreasureTable[5][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[5][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData6Label.Text = $"6) {LoadedDungFloorHeader.FloorTreasureTable[6][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[6][1].ToString(Settings.Settings.ValueTextFormat)}";
            ChestContentData7Label.Text = $"7) {LoadedDungFloorHeader.FloorTreasureTable[7][0].ToString(Settings.Settings.ValueTextFormat)}, {LoadedDungFloorHeader.FloorTreasureTable[7][1].ToString(Settings.Settings.ValueTextFormat)}";
        }

        /// <summary>
        /// Check if the filename exists in the directory <see cref="DungFileDir"/> and load its bytes
        /// </summary>
        /// <param name="filename">The name of the file to load</param>
        private void LoadDungFileRawData(string filename)
        {
            SelectedDungFilePath = $"{DungFileDir}\\{filename}";
            if (!File.Exists(SelectedDungFilePath))
                return;

            LoadedDungFileRawData = File.ReadAllBytes(SelectedDungFilePath);
            SerializeDungFile(LoadedDungFileRawData);
            PopulateSelectDungFloorComboBox();
        }

        private void SerializeDungFile(byte[] data)
        {
            //Dirty try-catch the entire block. The issue is that there is no identifiable header for DUNG files we could use instead...
            //Should be done properly later (never said that before)
            try
            {
                LoadedDungFile = new DUNG(data);
                DungInterpreter = new DUNGInterpreter(LoadedDungFile, LoadedDungFloorHeader);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void PopulateSelectDungFloorComboBox()
        {
            SelectDungFloorComboBox.DataSource = LoadedDungFile.DungFloorHeaders.Select(floorHeader => DUNGInterpreter.GetFloorName(floorHeader.FloorNameData)).ToList();
        }

        private void LoadSelectedFloorData(string floorName)
        {
            LoadedDungFloorHeader = loadedDungFile.DungFloorHeaders.FirstOrDefault(floorHeader => DUNGInterpreter.GetFloorName(floorHeader.FloorNameData) == floorName);

            if (LoadedDungFloorHeader == null)
                return;
        }

        private void ShoowHideDigiInfoButtons(bool show)
        {
            SlotOneInfoPictureBox.Visible = show;
            SlotTwoInfoPictureBox.Visible = show;
            SlotThreeInfoPictureBox.Visible = show;
            SlotFourInfoPictureBox.Visible = show;
        }
    }
}