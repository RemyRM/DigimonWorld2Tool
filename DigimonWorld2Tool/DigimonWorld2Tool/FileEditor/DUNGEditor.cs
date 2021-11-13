using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Views;
using System.IO;

namespace DigimonWorld2Tool.FileEditor
{
    class DUNGEditor : BaseFileEditor, IBaseFileEditor
    {
        public DUNG LoadedDUNGData { get; private set; }
        //public DungFloorHeader LoadedDungFloorHeader {get; private set;}
        //public DungFloorLayoutHeader LoadedDungFloorLayoutHeader { get; private set; }
        private int FloorIndex { get; set; }
        private int LayoutIndex{ get; set;}

        private string FilePath { get; set; }

        public DUNGEditor(string filePath, DUNG DUNGData, int floorIndex, int layoutIndex /*DungFloorHeader DUNGFloorHeader, DungFloorLayoutHeader DUNGFloorLayoutHeader*/) : base(filePath)
        {
            LoadedDUNGData = DUNGData;
            //LoadedDungFloorHeader = DUNGFloorHeader;
            //LoadedDungFloorLayoutHeader = DUNGFloorLayoutHeader;
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
            if(clickX % 2 == 0)
                ByteToEdit.SetRightNiblet((byte)tileType);
            else
                ByteToEdit.SetLeftNiblet((byte)tileType);

            LoadedDUNGData.DungFloorHeaders[FloorIndex].DungFloorLayoutHeaders[LayoutIndex].FloorLayoutData[tileIndex] = ByteToEdit;

            DUNGLayoutRenderer.Instance.UpdateTileAtPosition(clickX, clickY, ByteToEdit);
        }

        public void SaveFileData()
        {
            if(LoadedDUNGData == null)
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
        }
    }
}
