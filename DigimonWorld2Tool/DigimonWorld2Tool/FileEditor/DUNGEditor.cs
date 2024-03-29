﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Views;
using System.Collections.Generic;

namespace DigimonWorld2Tool.FileEditor
{
    class DUNGEditor : BaseFileEditor, IBaseFileEditor
    {
        public DUNG LoadedDUNGData { get; private set; }
        private int FloorIndex { get; set; }
        private int LayoutIndex { get; set; }

        private string FilePath { get; set; }

        public DUNGEditor(string filePath, DUNG DUNGData, int floorIndex, int layoutIndex) : base(filePath)
        {
            LoadedDUNGData = DUNGData;
            FloorIndex = floorIndex;
            LayoutIndex = layoutIndex;
            FilePath = filePath;

            DUNGLayoutRenderer.Instance.SetupFloorLayoutToDraw(LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex]);
            DUNGLayoutRenderer.Instance.SetupDungFloorBitmap();
        }

        public void ChangeTileTypeAtIndex(int clickX, int clickY, DUNGLayoutRenderer.TileType tileType)
        {
            double tileXPos = clickX / 2;
            int tileIndex = (int)Math.Floor(tileXPos + ((DUNGLayoutRenderer.MAP_WIDTH / 2) * clickY));

            byte ByteToEdit = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutData[tileIndex];
            if (clickX % 2 == 0)
                ByteToEdit.SetRightNiblet((byte)tileType);
            else
                ByteToEdit.SetLeftNiblet((byte)tileType);

            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutData[tileIndex] = ByteToEdit;

            DUNGLayoutRenderer.Instance.UpdateTileAtPosition(clickX, clickY, ByteToEdit);
        }

        public void UpdateWarpDataPointers(int newPtr)
        {
            int oldPtr = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].DungFloorLayoutHeaderBasePointer + (int)DungFloorLayoutHeader.FloorLayoutHeaderOffset.WarpsPointer;
            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarpsPointer = newPtr;
            //Override the old pointer value with the new pointer
            Array.Copy(BitConverter.GetBytes(newPtr), 0, LoadedDUNGData.RawFileData, oldPtr, 4);
        }

        public void UpdateChestDataPointers(int newPtr)
        {
            int oldPtr = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].DungFloorLayoutHeaderBasePointer + (int)DungFloorLayoutHeader.FloorLayoutHeaderOffset.ChestsPointer;
            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChestsPointer = newPtr;
            //Override the old pointer value with the new pointer
            Array.Copy(BitConverter.GetBytes(newPtr), 0, LoadedDUNGData.RawFileData, oldPtr, 4);
        }

        public void UpdateTrapDataPointers(int newPtr)
        {
            int oldPtr = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].DungFloorLayoutHeaderBasePointer + (int)DungFloorLayoutHeader.FloorLayoutHeaderOffset.TrapsPointer;
            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTrapsPointer = newPtr;
            //Override the old pointer value with the new pointer
            Array.Copy(BitConverter.GetBytes(newPtr), 0, LoadedDUNGData.RawFileData, oldPtr, 4);
        }

        public void UpdateDigimonDataPointers(int newPtr)
        {
            int oldPtr = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].DungFloorLayoutHeaderBasePointer + (int)DungFloorLayoutHeader.FloorLayoutHeaderOffset.DigimonPointer;
            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimonsPointer = newPtr;
            //Override the old pointer value with the new pointer
            Array.Copy(BitConverter.GetBytes(newPtr), 0, LoadedDUNGData.RawFileData, oldPtr, 4);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="curPosX"></param>
        /// <param name="curPosY"></param>
        /// <param name="type">0 = warps, 1 = chests, 2 = traps, 3 = digimons</param>
        public void EditObjectData(int curPosX, int curPosY, DungWindow.SelectedMapObjectType type)
        {
            switch (type)
            {
                case DungWindow.SelectedMapObjectType.Warp:
                    EditWarpData(curPosX, curPosY);
                    break;
                case DungWindow.SelectedMapObjectType.Chest:
                    EditChestData(curPosX, curPosY);
                    break;
                case DungWindow.SelectedMapObjectType.Trap:
                    EditTrapData(curPosX, curPosY);
                    break;
                case DungWindow.SelectedMapObjectType.Digimon:
                    EditDigimonData(curPosX, curPosY);
                    break;

            }

            DUNGLayoutRenderer.Instance.DrawDungFloorLayout();
            DungWindow.Instance.LoadedDungFile = LoadedDUNGData;
            DungWindow.Instance.LoadedDungFloorHeader = LoadedDUNGData.DungFloorHeaders[FloorIndex];
            DungWindow.Instance.LoadedDungFloorLayout = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex];
        }

        private void EditWarpData(int curPosX, int curPosY)
        {
            var selectedWarp = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps.FirstOrDefault(o => o.X == curPosX && o.Y == curPosY);
            var selectedWarpIndex = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps.ToList().IndexOf(selectedWarp);

            if (selectedWarp == null)
            {
                DebugWindow.DebugLogMessages.Add($"No warp found at position {curPosX.ToString(Settings.Settings.ValueTextFormat)},{curPosY.ToString(Settings.Settings.ValueTextFormat)}");
                return;
            }

            EditWarpWindow editWarpWindow = new EditWarpWindow();
            editWarpWindow.SetCurrentWarpData(selectedWarp.Type, selectedWarp.X, selectedWarp.Y);
            if (editWarpWindow.ShowDialog(DungWindow.Instance) == DialogResult.OK)
            {
                byte tpe = (byte)editWarpWindow.EditWarpTypeComboBox.SelectedIndex;
                byte x = (byte)editWarpWindow.WarpPositionXNumericUpDown.Value;
                byte y = (byte)editWarpWindow.WarpPositionYNumericUpDown.Value;

                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps[selectedWarpIndex].Type = tpe;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps[selectedWarpIndex].X = x;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps[selectedWarpIndex].Y = y;

                var bytes = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps[selectedWarpIndex].ToBytes();
                //warp entry is 3 bytes long
                var warpOffset = selectedWarpIndex * 3;
                var warpsDataPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarpsPointer;

                //update warp data in raw data
                Array.Copy(bytes, 0, LoadedDUNGData.RawFileData, warpsDataPointer + warpOffset, bytes.Length);
            }

            editWarpWindow.Dispose();
        }

        private void EditChestData(int curPosX, int curPosY)
        {
            var selectedChest = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests.FirstOrDefault(o => o.X == curPosX && o.Y == curPosY);
            var selectedChestIndex = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests.ToList().IndexOf(selectedChest);

            if (selectedChest == null)
            {
                DebugWindow.DebugLogMessages.Add($"No Chest found at position {curPosX.ToString(Settings.Settings.ValueTextFormat)},{curPosY.ToString(Settings.Settings.ValueTextFormat)}");
                return;
            }

            EditChestWindow editChestWindow = new EditChestWindow();
            editChestWindow.SetCurrentChestData(selectedChest.X, selectedChest.Y, selectedChest.ItemSlots);
            if (editChestWindow.ShowDialog(DungWindow.Instance) == DialogResult.OK)
            {
                byte x = (byte)editChestWindow.ChestPositionXNumericUpDown.Value;
                byte y = (byte)editChestWindow.ChestPositionYNumericUpDown.Value;

                var slot1ItemIndex = (byte)editChestWindow.Slot1ItemIndexNumericUpDown.Value;
                var slot2ItemIndex = (byte)editChestWindow.Slot2ItemIndexNumericUpDown.Value;
                var slot3ItemIndex = (byte)editChestWindow.Slot3ItemIndexNumericUpDown.Value;
                var slot4ItemIndex = (byte)editChestWindow.Slot4ItemIndexNumericUpDown.Value;

                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].X = x;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].Y = y;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].ItemSlots[0] = slot1ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].ItemSlots[1] = slot2ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].ItemSlots[2] = slot3ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].ItemSlots[3] = slot4ItemIndex;

                var bytes = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests[selectedChestIndex].ToBytes();
                //chest entry is 4 bytes long
                var chestOffset = selectedChestIndex * 4;
                var chestsDataPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChestsPointer;

                //update warp data in raw data
                Array.Copy(bytes, 0, LoadedDUNGData.RawFileData, chestsDataPointer + chestOffset, bytes.Length);
            }
            editChestWindow.Dispose();
        }

        private void EditTrapData(int curPosX, int curPosY)
        {
            var selectedTrap = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps.FirstOrDefault(o => o.X == curPosX && o.Y == curPosY);
            var selectedTrapIndex = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps.ToList().IndexOf(selectedTrap);

            if (selectedTrap == null)
            {
                DebugWindow.DebugLogMessages.Add($"No Trap found at position {curPosX.ToString(Settings.Settings.ValueTextFormat)},{curPosY.ToString(Settings.Settings.ValueTextFormat)}");
                return;
            }

            EditTrapsWindow editTrapWindow = new EditTrapsWindow();
            editTrapWindow.SetCurrentTrapData(selectedTrap.X, selectedTrap.Y, selectedTrap.TypeAndLevelData);
            if (editTrapWindow.ShowDialog(DungWindow.Instance) == DialogResult.OK)
            {
                byte x = (byte)editTrapWindow.TrapPositionXNumericUpDown.Value;
                byte y = (byte)editTrapWindow.TrapPositionYNumericUpDown.Value;
                var trapAndTypeLevel = editTrapWindow.GetTrapTypeAndLevelData();

                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].X = x;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].Y = y;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].TypeAndLevelData[0] = trapAndTypeLevel[0];
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].TypeAndLevelData[1] = trapAndTypeLevel[1];
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].TypeAndLevelData[2] = trapAndTypeLevel[2];
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].TypeAndLevelData[3] = trapAndTypeLevel[3];

                var bytes = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps[selectedTrapIndex].ToBytes();
                //trapentry is 8 bytes long
                var trapOffset = selectedTrapIndex * 8;
                var trapssDataPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTrapsPointer;

                //update warp data in raw data
                Array.Copy(bytes, 0, LoadedDUNGData.RawFileData, trapssDataPointer + trapOffset, bytes.Length);
            }
            editTrapWindow.Dispose();
        }

        private void EditDigimonData(int curPosX, int curPosY)
        {
            var selectedDigimon = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons.FirstOrDefault(o => o.X == curPosX && o.Y == curPosY);
            var selectedDigimonIndex = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons.ToList().IndexOf(selectedDigimon);

            if (selectedDigimon == null)
            {
                DebugWindow.DebugLogMessages.Add($"No Chest found at position {curPosX.ToString(Settings.Settings.ValueTextFormat)},{curPosY.ToString(Settings.Settings.ValueTextFormat)}");
                return;
            }

            EditDigimonWindow editDigimonWindow = new EditDigimonWindow();
            editDigimonWindow.SetCurrentDigimonData(selectedDigimon.X, selectedDigimon.Y, selectedDigimon.DigimonPackIndex);
            if (editDigimonWindow.ShowDialog(DungWindow.Instance) == DialogResult.OK)
            {
                byte x = (byte)editDigimonWindow.DigimonPositionXNumericUpDown.Value;
                byte y = (byte)editDigimonWindow.DigimonPositionYNumericUpDown.Value;

                var slot1ItemIndex = (byte)editDigimonWindow.Slot1DigimonIndexNumericUpDown.Value;
                var slot2ItemIndex = (byte)editDigimonWindow.Slot2DigimonIndexNumericUpDown.Value;
                var slot3ItemIndex = (byte)editDigimonWindow.Slot3DigimonIndexNumericUpDown.Value;
                var slot4ItemIndex = (byte)editDigimonWindow.Slot4DigimonIndexNumericUpDown.Value;

                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].X = x;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].Y = y;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].DigimonPackIndex[0] = slot1ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].DigimonPackIndex[1] = slot2ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].DigimonPackIndex[2] = slot3ItemIndex;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].DigimonPackIndex[3] = slot4ItemIndex;

                var bytes = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons[selectedDigimonIndex].ToBytes();
                //chest entry is 4 bytes long
                var digimonOffset = selectedDigimonIndex * 4;
                var digimonDataPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimonsPointer;

                //update warp data in raw data
                Array.Copy(bytes, 0, LoadedDUNGData.RawFileData, digimonDataPointer + digimonOffset, bytes.Length);
            }
            editDigimonWindow.Dispose();
        }

        internal void EditFloorHeaderData()
        {
            EditFloorHeaderWindow floorHeaderWindow = new EditFloorHeaderWindow();
            floorHeaderWindow.SetFloorHeaderDigimonPackData(LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable);
            floorHeaderWindow.SetFloorHeaderChestData(LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable);
            floorHeaderWindow.SetFloorName(LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorNameData);
            floorHeaderWindow.SetFloorScriptBytes(LoadedDUNGData.DungFloorHeaders[FloorIndex].ScriptID);
            floorHeaderWindow.SetFloorWallTextureID(LoadedDUNGData.DungFloorHeaders[FloorIndex].WallTextureID);
            floorHeaderWindow.SetFloorTypeOverride(LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTypeOverride);
            floorHeaderWindow.SetFloorTrapLevel(LoadedDUNGData.DungFloorHeaders[FloorIndex].TrapLevel);

            if (floorHeaderWindow.ShowDialog(DungWindow.Instance) == DialogResult.OK)
            {
                #region DigimonData
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable[0] = (byte)floorHeaderWindow.Pack0IDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable[1] = (byte)floorHeaderWindow.Pack1IDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable[2] = (byte)floorHeaderWindow.Pack2IDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable[3] = (byte)floorHeaderWindow.Pack3IDNumericUpDown.Value;

                byte[] digimonPackBytes = LoadedDUNGData.DungFloorHeaders[FloorIndex].DigimonEncounterTable;
                int digimonPackPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.DigimonTable;
                //Write the digimon pack data to the raw BIN file
                Array.Copy(digimonPackBytes, 0, LoadedDUNGData.RawFileData, digimonPackPointer, digimonPackBytes.Length);
                #endregion

                #region TreasureData
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[0][0] = (byte)floorHeaderWindow.Treasure0ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[0][1] = (byte)floorHeaderWindow.Treasure0TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[1][0] = (byte)floorHeaderWindow.Treasure1ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[1][1] = (byte)floorHeaderWindow.Treasure1TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[2][0] = (byte)floorHeaderWindow.Treasure2ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[2][1] = (byte)floorHeaderWindow.Treasure2TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[3][0] = (byte)floorHeaderWindow.Treasure3ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[3][1] = (byte)floorHeaderWindow.Treasure3TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[4][0] = (byte)floorHeaderWindow.Treasure4ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[4][1] = (byte)floorHeaderWindow.Treasure4TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[5][0] = (byte)floorHeaderWindow.Treasure5ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[5][1] = (byte)floorHeaderWindow.Treasure5TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[6][0] = (byte)floorHeaderWindow.Treasure6ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[6][1] = (byte)floorHeaderWindow.Treasure6TrapLevelNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[7][0] = (byte)floorHeaderWindow.Treasure7ItemIDNumericUpDown.Value;
                LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable[7][1] = (byte)floorHeaderWindow.Treasure7TrapLevelNumericUpDown.Value;

                List<byte[]> treasureData = LoadedDUNGData.DungFloorHeaders[FloorIndex].FloorTreasureTable;
                int treasureDataPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.TreasureTable;
                byte[] treasureDataBytes = new byte[treasureData.Count * 4];
                for (int i = 0; i < treasureData.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        treasureDataBytes[i * 4 + j] = treasureData[i][j];
                    }
                }
                //Write the treasure data to the raw BIN file
                Array.Copy(treasureDataBytes.ToArray(), 0, LoadedDUNGData.RawFileData, treasureDataPointer, treasureDataBytes.ToArray().Length);
                #endregion

                #region NameData
                string floorNameCharacters = floorHeaderWindow.FloorNameTextBox.Text;
                List<byte> floorNameData = Utility.TextConversion.ASCIIToDigiString(floorNameCharacters).ToList();
                floorNameData.Add(0xFF);
                int floorNamePointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorNamePointer;
                Array.Copy(floorNameData.ToArray(), 0, LoadedDUNGData.RawFileData, floorNamePointer, floorNameData.Count);
                #endregion

                #region ScriptID
                int scriptIDValue = (int)floorHeaderWindow.ScriptID0NumericUpDown.Value;
                byte[] scriptIDData = BitConverter.GetBytes(scriptIDValue);
                int scriptIDPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.ScriptID;
                Array.Copy(scriptIDData, 0, LoadedDUNGData.RawFileData, scriptIDPointer, scriptIDData.Length);
                #endregion

                #region WallTextureID
                int wallTextureIDValue = (int)floorHeaderWindow.WallTextureIDNumericUpDown.Value;
                byte[] wallTextureIDData = BitConverter.GetBytes(wallTextureIDValue);
                int wallTextureIDPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.WallTextureID;
                Array.Copy(wallTextureIDData, 0, LoadedDUNGData.RawFileData, wallTextureIDPointer, wallTextureIDData.Length);
                #endregion

                #region FloorTypeOverride
                short floorTypeOverride = (short)floorHeaderWindow.FloorTypeOverrideNumericUpDown.Value;
                byte[] floorTypeOverrideData = BitConverter.GetBytes(floorTypeOverride);
                int floorTypeOverridePointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.FloorTypeOverride;
                Array.Copy(floorTypeOverrideData, 0, LoadedDUNGData.RawFileData, floorTypeOverridePointer, floorTypeOverrideData.Length);
                #endregion

                #region TrapLevel
                short trapLevel = (short)floorHeaderWindow.TrapLevelNumericUpDown.Value;
                byte[] trapLevelData = BitConverter.GetBytes(trapLevel);
                int trapLevelPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DomainFloorBasePointer + (int)DungFloorHeader.DomainDataHeaderOffset.TrapLevel;
                Array.Copy(trapLevelData, 0, LoadedDUNGData.RawFileData, trapLevelPointer, trapLevelData.Length);
                #endregion

            }
            floorHeaderWindow.Dispose();
        }

        public void SaveFileData()
        {
            if (LoadedDUNGData == null)
            {
                DebugWindow.DebugLogMessages.Add($"No DUNG was loaded in edit mode to save data for.");
                return;
            }

            UpdateRawDDUNGData();

            File.WriteAllBytes(FilePath, LoadedDUNGData.RawFileData);
        }

        private void UpdateRawDDUNGData()
        {
            int editedFloorLayoutPointer = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutPointer;
            var floorData = LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutData;

            Array.Copy(floorData, 0, LoadedDUNGData.RawFileData, editedFloorLayoutPointer, floorData.Length);

            List<byte> warpData = new List<byte>();
            foreach (var item in LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarps)
            {
                foreach (byte b in item.ToBytes())
                {
                    warpData.Add(b);
                }
            }
            warpData.Add(0xFF);
            Array.Copy(warpData.ToArray(), 0, LoadedDUNGData.RawFileData, LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutWarpsPointer, warpData.Count);

            List<byte> chestData = new List<byte>();
            foreach (var item in LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChests)
            {
                foreach (byte b in item.ToBytes())
                {
                    chestData.Add(b);
                }
            }
            chestData.Add(0xFF);
            Array.Copy(chestData.ToArray(), 0, LoadedDUNGData.RawFileData, LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutChestsPointer, chestData.Count);

            List<byte> trapData = new List<byte>();
            foreach (var item in LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTraps)
            {
                foreach (byte b in item.ToBytes())
                {
                    trapData.Add(b);
                }
            }
            trapData.Add(0xFF);
            Array.Copy(trapData.ToArray(), 0, LoadedDUNGData.RawFileData, LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutTrapsPointer, trapData.Count);

            List<byte> digimonData = new List<byte>();
            foreach (var item in LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimons)
            {
                foreach (byte b in item.ToBytes())
                {
                    digimonData.Add(b);
                }
            }
            digimonData.Add(0xFF);
            Array.Copy(digimonData.ToArray(), 0, LoadedDUNGData.RawFileData, LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutDigimonsPointer, digimonData.Count);
        }
    }
}
