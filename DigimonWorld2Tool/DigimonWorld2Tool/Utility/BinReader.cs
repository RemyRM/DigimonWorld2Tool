using System;
using System.Collections.Generic;
using DigimonWorld2MapVisualizer.Domains;

namespace DigimonWorld2MapVisualizer
{
    public static class BinReader
    {
        /// <summary>
        /// Get a at the pointerStartIndex of length <see cref="pointerSize"/> bytes.
        /// </summary>
        /// <param name="pointerStartIndex">The start address of the pointer in decimal</param>
        /// <param name="pointerSize">The size of the pointer in bytes (Default 4)</param>
        /// <returns>The decimal address that the pointer points to</returns>
        public static int GetPointer(int pointerStartIndex, int pointerSize = 4)
        {
            byte[] pointerBigEndian = Domain.DomainData[pointerStartIndex..(pointerStartIndex + pointerSize)];
            return BitConverter.ToInt32(pointerBigEndian);
        }

        /// <summary>
        /// Read all bytes at <see cref="pointerStartIndex"/> until the first occurance of <see cref="delimiter"/> and returns it as a list in which
        /// each segment is of length <see cref="dataSegmentLength"/>.
        /// </summary>
        /// <param name="pointerStartIndex">The index at which the list starts</param>
        /// <param name="dataSegmentLength">The data length of each entry in the list</param>
        /// <param name="delimiter">The delimiter to which we read. (Standard 0xFF)</param>
        /// <returns>A list containing all entries found</returns>
        public static List<byte[]> ReadBytesToDelimiter(int pointerStartIndex, int dataSegmentLength, byte delimiter = 0xFF)
        {
            try
            {

            int delimiterIndex = Array.IndexOf(Domain.DomainData, delimiter, pointerStartIndex);
            byte[] data = Domain.DomainData[pointerStartIndex..delimiterIndex];

            List<byte[]> allObjectsData = new List<byte[]>();
            for (int i = 0; i < data.Length / dataSegmentLength; i++)
            {
                byte[] objectData = data[(i * dataSegmentLength)..(i * dataSegmentLength + dataSegmentLength)];
                allObjectsData.Add(objectData);
            }
            return allObjectsData;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
