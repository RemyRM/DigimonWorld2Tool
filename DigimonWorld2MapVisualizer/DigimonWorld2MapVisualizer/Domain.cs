using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DigimonWorld2MapVisualizer
{
    class Domain
    {
        private static readonly string FilePathToMapDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Maps\\";
        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        public readonly string DomainName;
        private readonly string[] DomainData;

        private List<DomainFloorLayout> floorLayouts = new List<DomainFloorLayout>();

        public Domain(string domainFilename)
        {
            DomainData = ReadDomainMapDataFile(domainFilename);
            string[] startPointer = GetPointer(0, out int pointerDecimalAddress);
            string[] pointerToFileName = GetPointer(pointerDecimalAddress, out int domainNamePointerDecimalAddress);
            string[] domainNameBytes = GetDomainNameBytes(domainNamePointerDecimalAddress);
            DomainName = TextConversion.DigiBytesToString(domainNameBytes);
            Console.WriteLine();
            Console.WriteLine(DomainName);
        }

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

        private string[] GetPointer(int pointerStartIndex, out int pointerDecimalAddress)
        {
            string[] pointerBigEndian = DomainData[pointerStartIndex..(pointerStartIndex + 4)];
            string[] pointerLittleEndian = pointerBigEndian.Reverse().ToArray();
            pointerDecimalAddress = Int32.Parse(string.Join("", pointerLittleEndian), System.Globalization.NumberStyles.HexNumber);
            return pointerLittleEndian;
        }

        private string[] GetDomainNameBytes(int pointerStartIndex)
        {
            int delimiterIndex = Array.IndexOf(DomainData, "FF", pointerStartIndex);
            string[] domainNameBigEndian = DomainData[pointerStartIndex..delimiterIndex];
            
            return domainNameBigEndian.ToArray();
        }
    }
}
