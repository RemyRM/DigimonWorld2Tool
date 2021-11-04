using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DigimonWorld2Tool.Domains;
using DigimonWorld2Tool;

namespace DigimonWorld2Tool
{
    public static class BinReader
    {
        /// <summary>
        /// Get a at the pointerStartIndex of length <see cref="pointerSize"/> bytes.
        /// </summary>
        /// <param name="pointerStartIndex">The start address of the pointer in decimal</param>
        /// <param name="pointerSize">The size of the pointer in bytes (Default 4)</param>
        /// <returns>The decimal address that the pointer points to</returns>
        public static int GetPointer(byte[] data, int pointerStartIndex, int pointerSize = 4)
        {
            byte[] pointerBigEndian = data[pointerStartIndex..(pointerStartIndex + pointerSize)];
            return BitConverter.ToInt32(pointerBigEndian);
        }

        /// <summary>
        /// Get a at the pointerStartIndex of length <see cref="pointerSize"/> bytes.
        /// </summary>
        /// <param name="pointerStartIndex">The start address of the pointer in decimal</param>
        /// <param name="pointerSize">The size of the pointer in bytes (Default 4)</param>
        /// <returns>The decimal address that the pointer points to</returns>
        public static int GetPointerOld(int pointerStartIndex, int pointerSize = 4)
        {
            byte[] pointerBigEndian = Domain.DomainData[pointerStartIndex..(pointerStartIndex + pointerSize)];
            return BitConverter.ToInt32(pointerBigEndian);
        }

        /// <summary>
        /// Read the entire contents of a map file
        /// </summary>
        /// <param name="domainFilename">The name of the dungeon file to load</param>
        /// <returns>String array containing all the hex values in the binary</returns>
        /// <remarks>Technically we could use reader.ReadBytes(int.MaxValue), however this may cause an OutOfMemoryException on 32-bit systems.</remarks>
        public static byte[] ReadAllBytesInFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    reader.BaseStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            else
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Error; File {filePath} was not found\nPlease check if the file folder exists.");
                return null;
            }
        }

        /// <summary>
        /// Read all bytes at <see cref="pointerStartIndex"/> until the first occurance of <see cref="delimiter"/> and returns it as a list in which
        /// each segment is of length <see cref="dataSegmentLength"/>.
        /// </summary>
        /// <param name="pointerStartIndex">The index at which the list starts</param>
        /// <param name="delimiter">The delimiter to which we read. (Standard 0xFF)</param>
        /// <returns>A list containing all entries found</returns>
        public static List<byte[]> ReadBytesToDelimiter(byte[] inputData, int pointerStartIndex, int segmentLength, byte[] delimiter)
        {
            try
            {
                List<byte[]> results = new List<byte[]>();

                bool foundDelimiter = false;
                do
                {
                    byte[] found = inputData[pointerStartIndex..(pointerStartIndex + segmentLength)];

                    if (Enumerable.SequenceEqual(found, delimiter))
                    {
                        foundDelimiter = true;
                        continue;
                    }
                    else if (delimiter.Length == 1)
                    {
                        if (found.Contains(delimiter[0]))
                        {
                            foundDelimiter = true;
                            continue;
                        }
                    }

                    results.Add(found);
                    pointerStartIndex += segmentLength;

                } while (!foundDelimiter);

                return results;
            }
            catch (Exception e)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"An unexpected error occurred: {e}");
                return null;
            }
        }

        /// <summary>
        /// Read all bytes at <see cref="pointerStartIndex"/> until the first occurance of <see cref="delimiter"/> and returns it as a list in which
        /// each segment is of length <see cref="dataSegmentLength"/>.
        /// </summary>
        /// <param name="pointerStartIndex">The index at which the list starts</param>
        /// <param name="dataSegmentLength">The data length of each entry in the list</param>
        /// <param name="delimiter">The delimiter to which we read. (Standard 0xFF)</param>
        /// <returns>A list containing all entries found</returns>
        public static List<byte[]> ReadBytesToDelimiterOld(int pointerStartIndex, int dataSegmentLength, byte delimiter = 0xFF)
        {
            try
            {
                int delimiterIndex = Array.IndexOf(Domain.DomainData, delimiter, pointerStartIndex);
                if (delimiterIndex == -1)
                {
                    var floorId = Domain.Main.floorsInThisDomain.Count;
                    var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;

                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Could not find {delimiter:X2} delimiter for object list at floor {floorId} layout {layoutID}");
                    switch (DigimonWorld2ToolForm.ErrorMode)
                    {
                        case DigimonWorld2ToolForm.Strictness.Strict:
                            throw new Exception($"Error mode set to strict, stopping execution");

                        case DigimonWorld2ToolForm.Strictness.Sloppy:
                            return new List<byte[]>();
                    }
                }

                byte[] data = Domain.DomainData[pointerStartIndex..delimiterIndex];

                List<byte[]> allObjectsData = new List<byte[]>();
                for (int i = 0; i < data.Length / dataSegmentLength; i++)
                {
                    byte[] objectData = data[(i * dataSegmentLength)..(i * dataSegmentLength + dataSegmentLength)];
                    allObjectsData.Add(objectData);
                }
                return allObjectsData;
            }
            catch
            {
                var floorId = Domain.Main.floorsInThisDomain.Count;
                var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;

                if (pointerStartIndex > Domain.DomainData.Length)
                {
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Pointer address {pointerStartIndex:X8} is out of bounds at floor {floorId} layout {layoutID}");

                    switch (DigimonWorld2ToolForm.ErrorMode)
                    {
                        case DigimonWorld2ToolForm.Strictness.Strict:
                            throw new Exception($"Error mode set to strict, stopping execution");

                        case DigimonWorld2ToolForm.Strictness.Sloppy:
                            return new List<byte[]>();
                    }
                }
            }
            return new List<byte[]>();
        }
    }
}
