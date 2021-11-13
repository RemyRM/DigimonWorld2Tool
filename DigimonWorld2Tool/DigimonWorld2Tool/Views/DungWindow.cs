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
        public static DungWindow Instance { get; private set; }

        private string dungFileDir;
        public string DungFileDir
        {
            get => dungFileDir;
            private set
            {
                dungFileDir = value;
                DungDirLabel.Text = dungFileDir;
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
            private set => loadedDungFile = value;
        }

        private DungFloorHeader loadedDungFloorHeader;
        public DungFloorHeader LoadedDungFloorHeader
        {
            get => loadedDungFloorHeader;
            private set => loadedDungFloorHeader = value;
        }

        private DungFloorLayoutHeader loadedDungFloorLayout;
        public DungFloorLayoutHeader LoadedDungFloorLayout
        {
            get => loadedDungFloorLayout;
            private set => loadedDungFloorLayout = value;
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

        //private Dictionary<int, int> DistinctFloorLayoutPointersOccurance { get; set; } = new Dictionary<int, int>();
        private EnemySetHeader[] possibleDigimonSets;

        private DUNGLayoutRenderer.TileType SelectedTileTypeToPaint { get; set; }
        private DUNGEditor DungEditor { get; set; }

        private string SelectedDungFilePath { get; set; }

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
            Debug.WriteLine("Added event listener for EnableEditMode");
        }

        private void CleanUpEvents()
        {
            MainWindow.EditModeChanged -= OnEditModeChanged;
            Debug.WriteLine("Removed event listener for EnableEditMode");
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
            SaveChangesButton.Visible = enabled;

            if (enabled)
            {
                OnEditorTileTypeClick(EmptyTileTypePictureBox, null);
                //DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, LoadedDungFloorHeader, LoadedDungFloorLayout);
                DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, SelectDungFloorComboBox.SelectedIndex, LoadedDungFloorLayoutIndex);

                //DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDungFloorLayoutHeader);
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDUNGData.DungFloorHeaders[SelectDungFloorComboBox.SelectedIndex].DungFloorLayoutHeaders[LoadedDungFloorLayoutIndex]);
            }
            else
            {
                if (DungEditor != null)
                {
                    // Pop-up if there is unsaved changed
                }
                else
                {
                    DungEditor = null;

                }
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(LoadedDungFloorLayout);

            }

            DUNGLayoutRenderer.Instance.SetupDungFloorBitmap();
            DUNGLayoutRenderer.Instance.DrawDungFloorLayout();
        }

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

        public void OnWindowResizeEnded()
        {
            FloorLayoutButton_Click(LastPressedFloorLayoutButton, null);
        }

        private void LoadUserSettings()
        {
            DungFileDir = (string)Properties.Settings.Default["DefaultMapDataDirectory"];
        }

        /// <summary>
        /// Select the directory in which the dung files are stored
        /// </summary>
        private void SelectDungDirButton_Click(object sender, EventArgs e)
        {
            DungFileDir = GetDungFilesDirectory();
            DungFileNames = GetFileNamesInDungDirectory();
        }

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
                    return "";
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

        /// <summary>
        /// Select the clicked entry in the combo box as file to load
        /// </summary>
        private void SelectDungComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDomainNamesComboBox((ComboBox)sender);

            if (MainWindow.EditModeEnabled)
            {
                if (DungEditor != null)
                {
                    //If there is unsaved data ask to save data first
                    if (MessageBox.Show("There are unsaved changes, do you want to save before switching maps?", "Save changes?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Save 
                    }
                }
                //DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, LoadedDungFloorHeader, LoadedDungFloorLayout);
                DungEditor = new DUNGEditor(SelectedDungFilePath, LoadedDungFile, SelectDungFloorComboBox.SelectedIndex, LoadedDungFloorLayoutIndex);
            }
            else
                DungEditor = null;
        }

        private void PopulateDomainNamesComboBox(ComboBox comboBox)
        {
            string selectedDomainName = (string)comboBox.SelectedItem;
            string selectedFileName = Settings.Settings.DungMapping.FirstOrDefault(o => o.DomainName == selectedDomainName)?.Filename;
            selectedFileName ??= selectedDomainName;

            LoadDungFileRawData(selectedFileName);
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
            LoadedDungFile = new DUNG(data);
            DungInterpreter = new DUNGInterpreter(LoadedDungFile, LoadedDungFloorHeader);
        }

        private void PopulateSelectDungFloorComboBox()
        {
            SelectDungFloorComboBox.DataSource = LoadedDungFile.DungFloorHeaders.Select(floorHeader => DUNGInterpreter.GetFloorName(floorHeader.FloorNameData)).ToList();
        }

        private void SelectDungFloorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cBox = (ComboBox)sender;
            LoadSelectedFloorData((string)cBox.SelectedItem);
            DungInterpreter.UpdateDungFloor(LoadedDungFloorHeader);

            FloorLayoutButton_Click(FloorLayoutButton1, null);
        }

        private void LoadSelectedFloorData(string floorName)
        {
            //DistinctFloorLayoutPointersOccurance = new Dictionary<int, int>();
            LoadedDungFloorHeader = loadedDungFile.DungFloorHeaders.FirstOrDefault(floorHeader => DUNGInterpreter.GetFloorName(floorHeader.FloorNameData) == floorName);
            //LoadedDungFloorLayoutIndex = LoadedDungFloorHeader.DungFloorLayoutHeaders.ToList().IndexOf(LoadedDungFloorLayout);

            if (LoadedDungFloorHeader == null)
                return;

            //foreach (var item in LoadedDungFloorHeader.DungFloorLayoutHeaders)
            //{
            //    if (!DistinctFloorLayoutPointersOccurance.ContainsKey(item.FloorLayoutPointer))
            //        DistinctFloorLayoutPointersOccurance.Add(item.FloorLayoutPointer, 1);
            //    else
            //        DistinctFloorLayoutPointersOccurance[item.FloorLayoutPointer]++;
            //}
            //DisableUnusedFloorLayoutButtons(DistinctFloorLayoutPointersOccurance.Count);
        }

        private void DisableUnusedFloorLayoutButtons(int uniqueLayoutCount)
        {
            for (int i = 0; i < FloorLayoutButtons.Length; i++)
                FloorLayoutButtons[i].Enabled = i < uniqueLayoutCount;
                //FloorLayoutButtons[i].Visible = i < uniqueLayoutCount;
        }

        private void FloorLayoutButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (LastPressedFloorLayoutButton != null)
                LastPressedFloorLayoutButton.BackColor = (Color)Settings.Settings.ButtonBackgroundColour;

            button.BackColor = (Color)Settings.Settings.ButtonSelectedBackgroundColour;
            LastPressedFloorLayoutButton = button;

            var buttonId = FloorLayoutButtons.ToList().IndexOf(button);
            //int distinctHeaderPointer = DistinctFloorLayoutPointersOccurance.ElementAt(buttonId).Key;
            //LoadedDungFloorLayout = LoadedDungFloorHeader.DungFloorLayoutHeaders.FirstOrDefault(o => o.FloorLayoutPointer == distinctHeaderPointer);
            LoadedDungFloorLayout = LoadedDungFloorHeader.DungFloorLayoutHeaders[buttonId];
            LoadedDungFloorLayoutIndex = buttonId;

            if (LoadedDungFloorLayout == null)
                throw new NullReferenceException();

            if (MainWindow.EditModeEnabled)
                //DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDungFloorLayoutHeader);
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(DungEditor.LoadedDUNGData.DungFloorHeaders[SelectDungFloorComboBox.SelectedIndex].DungFloorLayoutHeaders[LoadedDungFloorLayoutIndex]);
            else
                DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(LoadedDungFloorLayout);

            DUNGLayoutRenderer.Instance.SetupDungFloorBitmap();
            DUNGLayoutRenderer.Instance.DrawDungFloorLayout();
        }

        private void DrawGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FloorLayoutButton_Click(LastPressedFloorLayoutButton, null);
        }

        private void DisplayMousePositionOnGrid(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PaintSelectedTileType(e.Location);

            if (loadedDungFile == null)
                return;

            MousePositionLabel.Text = $"X: {(int)e.Location.X / DUNGLayoutRenderer.Instance.TileSizeWidth:D2} Y: {(int)e.Location.Y / DUNGLayoutRenderer.Instance.TileSizeHeight:D2}";
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
                PositionLabel.Text = $"Position: {trap.X}, {trap.Y}";

                SlotOneLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(trap.TypeAndLevel[0].Type)}, Level: {DUNGInterpreter.GetTrapLevel(trap.TypeAndLevel[0].Level)}";
                SlotTwoLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(trap.TypeAndLevel[1].Type)}, Level: {DUNGInterpreter.GetTrapLevel(trap.TypeAndLevel[1].Level)}";
                SlotThreeLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(trap.TypeAndLevel[2].Type)}, Level: {DUNGInterpreter.GetTrapLevel(trap.TypeAndLevel[2].Level)}";
                SlotFourLabel.Text = $"Type: {DUNGInterpreter.GetTrapType(trap.TypeAndLevel[3].Type)}, Level: {DUNGInterpreter.GetTrapLevel(trap.TypeAndLevel[3].Level)}";

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
                PositionLabel.Text = $"Position: {warp.X}, {warp.Y}";
                SlotOneLabel.Text = "";
                SlotTwoLabel.Text = "";
                SlotThreeLabel.Text = "";
                SlotFourLabel.Text = "";

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
                PositionLabel.Text = $"Position: {digimon.X}, {digimon.Y}";

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
                PositionLabel.Text = $"Position: {treasure.X}, {treasure.Y}";

                string[] treasures = new string[4];
                for (int i = 0; i < treasures.Length; i++)
                {
                    if (treasure.ItemSlots[i] == 0)
                        treasures[i] = "No chest";
                    else
                    {
                        var treasureIndex = treasure.ItemSlots[i];
                        var treasureData = LoadedDungFloorHeader.FloorTreasureTable[treasureIndex - 1];

                        var itemID = treasureData.ItemID;
                        var trapLevel = treasureData.TrapLevel;

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

                ShoowHideDigiInfoButtons(false);
            }
        }

        private void PaintSelectedTileType(Point mousePos)
        {
            var clickX = (int)mousePos.X / DUNGLayoutRenderer.Instance.TileSizeWidth;
            var clickY = (int)mousePos.Y / DUNGLayoutRenderer.Instance.TileSizeHeight;

            DungEditor.ChangeTileTypeAtIndex(clickX, clickY, SelectedTileTypeToPaint);
        }

        private void ShoowHideDigiInfoButtons(bool show)
        {
            SlotOneInfoPictureBox.Visible = show;
            SlotTwoInfoPictureBox.Visible = show;
            SlotThreeInfoPictureBox.Visible = show;
            SlotFourInfoPictureBox.Visible = show;
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
    }
}
