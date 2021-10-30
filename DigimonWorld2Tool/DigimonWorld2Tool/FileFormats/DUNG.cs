using System;
using System.Collections.Generic;

using DigimonWorld2MapTool;
using DigimonWorld2MapTool.Utility;
using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.FileFormats
{
    public class DUNG
    {
        public byte[] RawFileData { get; private set; }
        private int[] DomainFloorPointers { get; set; }
        public DungFloorHeader[] DungFloorHeaders { get; private set; }

        public DUNG(byte[] rawFileData)
        {
            RawFileData = rawFileData;
            DomainFloorPointers = GetDomainFloorPointers();

            DungFloorHeaders = new DungFloorHeader[DomainFloorPointers.Length];

            //Create a new DungFloorHeader for every pointer found.
            for (int i = 0; i < DungFloorHeaders.Length; i++)
                DungFloorHeaders[i] = new DungFloorHeader(RawFileData, DomainFloorPointers[i]);
        }

        public DUNG(string filepath)
        {
            RawFileData = BinReader.ReadAllBytesInFile(filepath);
            DomainFloorPointers = GetDomainFloorPointers();

            DungFloorHeaders = new DungFloorHeader[DomainFloorPointers.Length];

            //Create a new DungFloorHeader for every pointer found.
            for (int i = 0; i < DungFloorHeaders.Length; i++)
                DungFloorHeaders[i] = new DungFloorHeader(RawFileData, DomainFloorPointers[i]);
        }

        /// <summary>
        /// Get the 4 byte pointers to the headers of the floors in this domain. 
        /// This is a list delimited by 0x00000000
        /// </summary>
        /// <returns></returns>
        private int[] GetDomainFloorPointers()
        {
            List<byte[]> pointers = BinReader.ReadBytesToDelimiter(RawFileData, 0, 4, new byte[] { 0x00, 0x00, 0x00, 0x00 });

            int[] result = new int[pointers.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = BitConverter.ToInt32(pointers[i]);

            return result;
        }
    }

    public class DungFloorHeader
    {
        //These are pre-determined offsets in the header to certain data entries
        private enum DomainDataHeaderOffset
        {
            FileName = 0,
            ScriptID = 4,
            FloorLayout0 = 8,
            FloorLayout1 = 12,
            FloorLayout2 = 16,
            FloorLayout3 = 20,
            FloorLayout4 = 24,
            FloorLayout5 = 28,
            FloorLayout6 = 32,
            FloorLayout7 = 36,
            WallTextureID = 40,
            FloorTypeOverride = 44,
            TrapLevel = 46,
            DigimonTable = 48,
            TreasureTable = 52
        }

        private readonly byte[] RawFileData;

        private int DomainFloorBasePointer { get; set; }
        private int DomainFloorNamePointer { get; set; }

        public string FloorName { get; private set; }
        public int ScriptID { get; private set; }

        public DungFloorLayoutHeader[] DungFloorLayoutHeaders { get; private set; } = new DungFloorLayoutHeader[8];

        public int WallTextureID { get; private set; }
        public short FloorTypeOverride { get; private set; }

        public short TrapLevel { get; private set; }
        public byte[] DigimonEncounterTable { get; private set; } = new byte[4];
        public DungFloorTreasureContents[] FloorTreasureTable { get; private set; } = new DungFloorTreasureContents[8];


        internal DungFloorHeader(byte[] rawData, int startFloorDataPointer)
        {
            RawFileData = rawData;

            DomainFloorBasePointer = startFloorDataPointer;
            DomainFloorNamePointer = BinReader.GetPointer(RawFileData, startFloorDataPointer);
            FloorName = GetFloorName();
            ScriptID = GetScriptID();

            DungFloorLayoutHeaders = GetFloorLayoutPointers();

            WallTextureID = GetWallTextureID();
            FloorTypeOverride = GetFloorTypeOverride();
            TrapLevel = GetTrapLevel();

            DigimonEncounterTable = GetDigimonEncounterTable();
            FloorTreasureTable = GetFloorTreasureTable();
        }

        /// <summary>
        /// Get the bytes stored at the Floor name pointer. This is a list of bytes delimited by 0xFF.
        /// Gets parsed by <see cref="TextConversion.DigiStringToASCII(byte[])"/> to be translated to english.
        /// </summary>
        /// <returns>ASCII string containing the name of the floor</returns>
        private string GetFloorName()
        {
            int delimiterIndex = Array.IndexOf(RawFileData, (byte)0xFF, DomainFloorNamePointer);
            byte[] nameData = RawFileData[DomainFloorNamePointer..delimiterIndex];

            return TextConversion.DigiStringToASCII(nameData);
        }

        /// <summary>
        /// Get the ID of the script that needs to be executed on this floor
        /// </summary>
        /// <returns>The Int32 representation of the script ID</returns>
        private int GetScriptID()
        {
            int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.ScriptID;
            return BitConverter.ToInt32(RawFileData[startAddress..(startAddress + 4)]);
        }

        /// <summary>
        /// Every floor has up to 8 unique layouts, but can have duplicates.
        /// Get the pointers to the header each of these 8 layouts
        /// </summary>
        /// <returns>The header data of every layout on this specific floor</returns>
        private DungFloorLayoutHeader[] GetFloorLayoutPointers()
        {
            DungFloorLayoutHeader[] results = new DungFloorLayoutHeader[8];
            for (int i = 0; i < DungFloorLayoutHeaders.Length; i++)
            {
                DomainDataHeaderOffset floorPointerAddressOffset = (DomainDataHeaderOffset)Enum.Parse(typeof(DomainDataHeaderOffset), $"FloorLayout{i}");
                int floorHeaderOffset = DomainFloorBasePointer + (int)floorPointerAddressOffset;
                results[i] = new DungFloorLayoutHeader(RawFileData, BinReader.GetPointer(RawFileData, floorHeaderOffset));
            }
            return results;
        }

        /// <summary>
        /// Get the ID indicating which tile set needs to be used
        /// </summary>
        /// <returns>The Int32 representation of the ID dictating which tileset is used</returns>
        private int GetWallTextureID()
        {
            int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.WallTextureID;
            return BitConverter.ToInt32(RawFileData[startAddress..(startAddress + 4)]);
        }

        /// <summary>
        /// Get the ID of the floor type that gets used by the floor type override.
        /// This is used to make tiles with the ID of 0x07 set to the type found in this override 
        /// </summary>
        /// <returns>The byte ID of the override type</returns>
        private short GetFloorTypeOverride()
        {
            int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.FloorTypeOverride;
            return BitConverter.ToInt16(RawFileData[startAddress..(startAddress + 2)]);
        }

        /// <summary>
        /// Get the trap level of the current layout.
        /// Not exactly sure what its used for tbh
        /// </summary>
        /// <returns>A byte array with lenght of 3, containing the trap level(s)</returns>
        private short GetTrapLevel()
        {
            //byte[] results = new byte[3];
            //int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.TrapLevel;

            //for (int i = 0; i < results.Length; i++)
            //    results[i] = RawFileData[startAddress + i];

            //return results;
            int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.TrapLevel;
            return BitConverter.ToInt16(RawFileData[startAddress..(startAddress + 2)]);
        }

        /// <summary>
        /// Every floor has a table containing 4 digimon IDs determining which digimon pack can spawn.
        /// This ID is looked up against the ENEMYSET.Bin file in \AAA\4.AAA\DATAFILE
        /// </summary>
        /// <returns>Byte array of length 4 containing all the encounter pack IDs</returns>
        private byte[] GetDigimonEncounterTable()
        {
            byte[] results = new byte[4];
            int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.DigimonTable;

            for (int i = 0; i < results.Length; i++)
                results[i] = RawFileData[startAddress + i];

            return results;
        }

        /// <summary>
        /// Every floor has a table with IDs containing the possible obtainable treasures for this floor, and the level of the trap on the chest.
        /// This ID is looked up against the ITEMDATA.BIN file in \AAA\4.AAA\DATAFILE
        /// </summary>
        /// <returns><see cref="DungFloorTreasureContents"/> array of length 8 containing all the possible treasure for the floor</returns>
        private DungFloorTreasureContents[] GetFloorTreasureTable()
        {
            DungFloorTreasureContents[] results = new DungFloorTreasureContents[8];
            for (int i = 0; i < results.Length; i++)
            {
                byte[] data = new byte[4];
                int startAddress = DomainFloorBasePointer + (int)DomainDataHeaderOffset.TreasureTable + (i * 4);
                for (int j = 0; j < data.Length; j++)
                    data[j] = RawFileData[startAddress + j];
                results[i] = new DungFloorTreasureContents(data);
            }
            return results;
        }
    }

    public class DungFloorTreasureContents
    {
        public byte ItemID { get; private set; }
        public byte TrapLevel { get; private set; }

        public DungFloorTreasureContents(byte[] data)
        {
            ItemID = data[0];
            TrapLevel = data[1];
        }
    }

    public class DungFloorLayoutHeader
    {
        private enum FloorLayoutHeaderOffset : byte
        {
            FloorPlan = 0,
            Warps = 4,
            Chests = 8,
            Traps = 12,
            Digimon = 16,
        }

        private enum MapObjectDataLength : byte
        {
            Warps = 3,
            Chests = 4,
            Traps = 8,
            Digimon = 4,
        }

        private readonly byte[] RawFileData;

        private int DungFloorLayoutHeaderBasePointer { get; set; }

        public int FloorLayoutPointer { get; private set; }
        public byte[] FloorLayoutData { get; private set; } = new byte[1536]; //All the layout data for a given map is 1536 bytes long (32x48)

        private int FloorLayoutWarpsPointer { get; set; }
        public DungFloorWarp[] FloorLayoutWarps { get; private set; }

        private int FloorLayoutChestsPointer { get; set; }
        public DungFloorChest[] FloorLayoutChests { get; private set; }

        private int FloorLayoutTrapsPointer { get; set; }
        public DungFloorTrap[] FloorLayoutTraps { get; private set; }

        private int FloorLayoutDigimonsPointer { get; set; }
        public DungFloorDigimon[] FloorLayoutDigimons { get; private set; }

        public DungFloorLayoutHeader(byte[] fileData, int pointer)
        {
            RawFileData = fileData;

            DungFloorLayoutHeaderBasePointer = pointer;

            FloorLayoutPointer = GetFloorLayoutDataPointer();
            FloorLayoutData = GetFloorLayoutData();

            FloorLayoutWarpsPointer = GetFloorLayoutWarpsPointer();
            FloorLayoutWarps = GetFloorLayoutWarps();

            FloorLayoutChestsPointer = GetFloorLayoutChestsPointer();
            FloorLayoutChests = GetFloorLayoutChests();

            FloorLayoutTrapsPointer = GetFloorLayoutTrapsPointer();
            FloorLayoutTraps = GetFloorLayoutTraps();

            FloorLayoutDigimonsPointer = GetFloorLayoutDigimonsPointer();
            FloorLayoutDigimons = GetFloorLayoutDigimons();
        }

        /// <summary>
        /// Get the pointer to the start of the floor layout data
        /// </summary>
        /// <returns>Int32 representation of the floor layout start address</returns>
        private int GetFloorLayoutDataPointer()
        {
            return BinReader.GetPointer(RawFileData, DungFloorLayoutHeaderBasePointer);
        }

        /// <summary>
        /// Get the 1536 bytes of map layout data
        /// </summary>
        /// <returns>Byte array of length 1536 with floor layout data</returns>
        private byte[] GetFloorLayoutData()
        {
            return RawFileData[FloorLayoutPointer..(FloorLayoutPointer + FloorLayoutData.Length)];
        }

        /// <summary>
        /// Get the pointer to the start of the floor warps data
        /// </summary>
        /// <returns>Int32 representation of the floor warps start address</returns>
        private int GetFloorLayoutWarpsPointer()
        {
            return BinReader.GetPointer(RawFileData, DungFloorLayoutHeaderBasePointer + (int)FloorLayoutHeaderOffset.Warps);
        }

        /// <summary>
        /// Get the data for the location and warp types for all the warps on 
        /// </summary>
        /// <returns>An array of <see cref="DungFloorWarp"/> containing the layouts warp info</returns>
        private DungFloorWarp[] GetFloorLayoutWarps()
        {
            List<byte[]> data = BinReader.ReadBytesToDelimiter(RawFileData, FloorLayoutWarpsPointer, (int)MapObjectDataLength.Warps, new byte[] { 0xFF });

            DungFloorWarp[] results = new DungFloorWarp[data.Count];
            for (int i = 0; i < results.Length; i++)
            {
                byte[] warpData = new byte[(int)MapObjectDataLength.Warps];
                for (int j = 0; j < warpData.Length; j++)
                {
                    warpData[j] = data[i][j];
                }
                results[i] = new DungFloorWarp(warpData);
            }
            return results;
        }

        /// <summary>
        /// Get the pointer to the start of the floor chests data
        /// </summary>
        /// <returns>Int32 representation of the floor chest start address</returns>
        private int GetFloorLayoutChestsPointer()
        {
            return BinReader.GetPointer(RawFileData, DungFloorLayoutHeaderBasePointer + (int)FloorLayoutHeaderOffset.Chests);
        }

        /// <summary>
        /// Get the data for this layouts chests and its contents
        /// </summary>
        /// <returns>Array containing <see cref="DungFloorChest"/></returns>
        private DungFloorChest[] GetFloorLayoutChests()
        {
            List<byte[]> data = BinReader.ReadBytesToDelimiter(RawFileData, FloorLayoutChestsPointer, (int)MapObjectDataLength.Chests, new byte[] { 0xFF });

            DungFloorChest[] results = new DungFloorChest[data.Count];
            for (int i = 0; i < results.Length; i++)
            {
                byte[] chestData = new byte[(int)MapObjectDataLength.Chests];
                for (int j = 0; j < chestData.Length; j++)
                {
                    chestData[j] = data[i][j];
                }
                results[i] = new DungFloorChest(chestData);
            }

            return results;
        }

        private int GetFloorLayoutTrapsPointer()
        {
            return BinReader.GetPointer(RawFileData, DungFloorLayoutHeaderBasePointer + (int)FloorLayoutHeaderOffset.Traps);
        }

        /// <summary>
        /// There are 2 padding bytes in the data of traps
        /// </summary>
        /// <returns></returns>
        private DungFloorTrap[] GetFloorLayoutTraps()
        {
            List<byte[]> data = BinReader.ReadBytesToDelimiter(RawFileData, FloorLayoutTrapsPointer, (int)MapObjectDataLength.Traps, new byte[] { 0xFF });

            DungFloorTrap[] results = new DungFloorTrap[data.Count];
            for (int i = 0; i < results.Length; i++)
            {
                byte[] trapsData = new byte[(int)MapObjectDataLength.Traps];
                for (int j = 0; j < trapsData.Length; j++)
                {
                    trapsData[j] = data[i][j];
                }
                results[i] = new DungFloorTrap(trapsData);
            }

            return results;
        }


        /// <summary>
        /// Get the pointer to the start of the floor digimons data
        /// </summary>
        /// <returns>Int32 representation of the floor digimons start address</returns>
        private int GetFloorLayoutDigimonsPointer()
        {
            return BinReader.GetPointer(RawFileData, DungFloorLayoutHeaderBasePointer + (int)FloorLayoutHeaderOffset.Digimon);
        }

        /// <summary>
        /// Get the data to the floors digimon spawns
        /// </summary>
        /// <returns></returns>
        private DungFloorDigimon[] GetFloorLayoutDigimons()
        {
            List<byte[]> data = BinReader.ReadBytesToDelimiter(RawFileData, FloorLayoutDigimonsPointer, (int)MapObjectDataLength.Digimon, new byte[] { 0xFF });

            DungFloorDigimon[] results = new DungFloorDigimon[data.Count];
            for (int i = 0; i < results.Length; i++)
            {
                byte[] digimonData = new byte[(int)MapObjectDataLength.Digimon];
                for (int j = 0; j < digimonData.Length; j++)
                {
                    digimonData[j] = data[i][j];
                }
                results[i] = new DungFloorDigimon(digimonData);
            }

            return results;
        }
    }

    public class DungFloorWarp : IDungLayoutObject
    {
        public enum WarpType
        {
            Entrance = 0,
            Next = 1,
            Exit = 2,
        }

        public WarpType Type { get; private set; }

        public DungFloorWarp(byte[] data)
        {
            X = data[0];
            Y = data[1];
            Type = (WarpType)data[2];
        }
    }

    public class DungFloorChest : IDungLayoutObject
    {
        public byte[] ItemSlots { get; private set; } = new byte[4];

        public DungFloorChest(byte[] data)
        {
            X = data[0];
            Y = data[1];

            ItemSlots[0] = data[2].GetLeftHalfByte();
            ItemSlots[1] = data[2].GetRightHalfByte();
            ItemSlots[2] = data[3].GetLeftHalfByte();
            ItemSlots[3] = data[3].GetRightHalfByte();
        }
    }

    public class DungFloorTrap : IDungLayoutObject
    {
        public enum TrapType : byte
        {
            None = 0,
            Swamp = 1,
            Spore = 2,
            Rock = 3,
            Mine = 4,
            Bit_Bug = 5,
            Energy_Bug = 6,
            Return_Bug = 7,
            Memory_bug = 8,
        }
        public enum TrapLevel : byte
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }

        private byte[] TypeAndLevelData { get; set; } = new byte[4];
        public TrapTypeAndLevel[] TypeAndLevel { get; private set; } = new TrapTypeAndLevel[4];

        public DungFloorTrap(byte[] data)
        {
            X = data[0];
            Y = data[1];

            for (int i = 0; i < TypeAndLevelData.Length; i++)
            {
                //Start at 2 to offset X and Y
                TypeAndLevelData[i] = data[i + 2];
                TypeAndLevel[i] = new TrapTypeAndLevel(data[i + 2]);
            }
        }

        public class TrapTypeAndLevel
        {
            public TrapType Type { get; private set; }
            public TrapLevel Level { get; private set; }

            public TrapTypeAndLevel(byte data)
            {
                Type = (TrapType)data.GetRightHalfByte();
                Level = (TrapLevel)data.GetLeftHalfByte();
            }
        }
    }

    public class DungFloorDigimon : IDungLayoutObject
    {
        public byte[] DigimonPackID { get; private set; } = new byte[4];

        public DungFloorDigimon(byte[] data)
        {
            X = data[0];
            Y = data[1];
            DigimonPackID[0] = data[2].GetLeftHalfByte();
            DigimonPackID[1] = data[2].GetRightHalfByte();
            DigimonPackID[2] = data[3].GetLeftHalfByte();
            DigimonPackID[3] = data[3].GetRightHalfByte();
        }
    }
}
