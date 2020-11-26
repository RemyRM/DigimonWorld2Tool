using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace DigimonWorld2MapVisualizer
{
    public static class BinReader
    {
        /// <summary>
        /// Get a 4 byte pointer at the pointerStartIndex
        /// </summary>
        /// <param name="pointerStartIndex">The start address of the pointer in decimal</param>
        /// <param name="pointerDecimalAddress">The decimal address that the pointer points to</param>
        /// <returns>The hex address and decimal value that the pointer points to in little endian</returns>
        public static string[] GetPointer(int pointerStartIndex, out int pointerDecimalAddress)
        {
            string[] pointerBigEndian = Domain.DomainData[pointerStartIndex..(pointerStartIndex + 4)];
            string[] pointerLittleEndian = pointerBigEndian.Reverse().ToArray();
            pointerDecimalAddress = Int32.Parse(string.Join("", pointerLittleEndian), NumberStyles.HexNumber);
            return pointerLittleEndian;
        }

        public static List<string[]> ReadBytesToDelimiter(int pointerStartIndex, int dataSegmentLength, string delimiter = "FF")
        {
            int delimiterIndex = Array.IndexOf(Domain.DomainData, delimiter, pointerStartIndex);
            string[] data = Domain.DomainData[pointerStartIndex..delimiterIndex];

            List<string[]> allObjectsData = new List<string[]>();
            for (int i = 0; i < data.Length / dataSegmentLength; i++)
            {
                string[] objectData = data[(i * dataSegmentLength)..(i * dataSegmentLength + dataSegmentLength)];
                allObjectsData.Add(objectData);
            }
            return allObjectsData;
        }

        public static Vector2 ReadMapObjectPosition(ref string[] data)
        {
            return new Vector2(int.Parse(data[0], NumberStyles.HexNumber), int.Parse(data[1], NumberStyles.HexNumber));
        }
    }
}
