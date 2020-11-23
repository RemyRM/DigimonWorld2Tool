using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigimonWorld2MapVisualizer
{
    public static class BinReader
    {
        /// <summary>
        /// Get a 2 byte pointer at the index as hex and decimal value and convert it to little endian
        /// </summary>
        /// <param name="pointerStartIndex">The start address of the pointer in decimal</param>
        /// <param name="pointerDecimalAddress">The decimal address that the pointer points to</param>
        /// <returns>The hex address that the pointer points to in little endian</returns>
        public static string[] GetPointer(int pointerStartIndex, out int pointerDecimalAddress)
        {
            string[] pointerBigEndian = Domain.DomainData[pointerStartIndex..(pointerStartIndex + 4)];
            string[] pointerLittleEndian = pointerBigEndian.Reverse().ToArray();
            pointerDecimalAddress = Int32.Parse(string.Join("", pointerLittleEndian), System.Globalization.NumberStyles.HexNumber);
            return pointerLittleEndian;
        }
    }
}
