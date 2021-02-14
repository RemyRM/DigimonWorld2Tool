using static DigimonWorld2MapTool.BinReader;
using System;
using System.Collections.Generic;
using System.IO;
using DigimonWorld2Tool;

namespace DigimonWorld2MapTool.Domains
{
    public class Domain
    {
        public static Domain Main;

        public static byte[] DomainData { get; private set; }
        public readonly string DomainName;

        public List<DomainFloor> floorsInThisDomain = new List<DomainFloor>();

        public Domain(string domainFilename)
        {
            Main = this;
            DomainData = ReadDomainMapDataFile(domainFilename);

            bool searchingDomainFloors = true;
            do
            {
                var floorHeaderBasePointerDecimalAddress = GetPointer(floorsInThisDomain.Count * 4);

                if (floorHeaderBasePointerDecimalAddress == 0)
                {
                    searchingDomainFloors = false;
                    continue;
                }

                DomainFloor floor = new DomainFloor(floorHeaderBasePointerDecimalAddress);
                floorsInThisDomain.Add(floor);

                DigimonWorld2ToolForm.Main.FloorSelectorComboBox.Items.Add(floor.FloorName);
            }
            while (searchingDomainFloors);
        }

        /// <summary>
        /// Read the entire contents of a map file
        /// </summary>
        /// <param name="domainFilename">The name of the dungeon file to load</param>
        /// <returns>String array containing all the hex values in the binary</returns>
        /// <remarks>Technically we could use reader.ReadBytes(int.MaxValue), however this may cause an OutOfMemoryException on 32-bit systems.</remarks>
        private byte[] ReadDomainMapDataFile(string domainFilename)
        {
            if (File.Exists(DigimonWorld2ToolForm.FilePathToMapDirectory + domainFilename))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(DigimonWorld2ToolForm.FilePathToMapDirectory + domainFilename, FileMode.Open)))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    reader.BaseStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Error; File {domainFilename} was not found in directory:" +
                                  $"\n{DigimonWorld2ToolForm.FilePathToMapDirectory }" +
                                  $"\nPlease check if the \\Maps\\ folder exists and contains the DUNGxxxx.BIN file(s).");
                return null;
            }
        }
    }
}
