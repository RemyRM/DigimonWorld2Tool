using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
{
    public class Domain
    {
        private static readonly string FilePathToMapDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Maps\\";

        public static string[] DomainData { get; private set; }
        public readonly string DomainName;

        public List<DomainFloor> floorsInThisDomain = new List<DomainFloor>();

        public Domain(string domainFilename)
        {
            DomainData = ReadDomainMapDataFile(domainFilename);

            var searchingDomainFloors = true;
            do
            {
                string[] floorHeaderBasePointerAddress = GetPointer(floorsInThisDomain.Count * 4, out int floorHeaderBasePointerDecimalAddress);
                if (floorHeaderBasePointerDecimalAddress == 0)
                {
                    searchingDomainFloors = false;
                    continue;
                }

                floorsInThisDomain.Add(new DomainFloor(floorHeaderBasePointerAddress, floorHeaderBasePointerDecimalAddress));
            }
            while (searchingDomainFloors);
        }

        /// <summary>
        /// Read the entire contents of a map file
        /// </summary>
        /// <param name="domainFilename">The name of the dungeon file to load</param>
        /// <returns>String array containing all the hex values in the binary</returns>
        private string[] ReadDomainMapDataFile(string domainFilename)
        {
            string result;
            using (BinaryReader reader = new BinaryReader(File.Open(FilePathToMapDirectory + domainFilename, FileMode.Open)))
            {
                using MemoryStream memoryStream = new MemoryStream();
                reader.BaseStream.CopyTo(memoryStream);
                result = BitConverter.ToString(memoryStream.ToArray());
            }
            return result.Split('-');
        }
    }
}
