using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Textures.Headers;

namespace DigimonWorld2Tool.Textures.Headers
{
    class TextureModelHeader
    {
        public readonly int TimOffset;
        public readonly int[] HeaderPointers;
        public readonly int[] HeaderPointersOrdered;
        public readonly List<IModelTextureSegment> SegmentsInHeader = new List<IModelTextureSegment>();

        public TextureModelHeader(ref BinaryReader reader)
        {
            TimOffset = GetTIMOffset(ref reader);
            reader.BaseStream.Position = 0x04; //Ensure we reset the position of the stream to the model texture
            if (reader.ReadInt32() != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No 0x00 terminator found after TIM pointer.");
                return;
            }
            HeaderPointers = GetHeaderPointers(ref reader);
            HeaderPointersOrdered = HeaderPointers.OrderBy(i => i).ToArray();
            //We start at 1 because the first entry is 0x10 which doesnt seem to be a valid pointer in this context
            for (int i = 1; i < HeaderPointersOrdered.Length; i++)
            {
                SegmentsInHeader.Add(GetSegmentData(i, ref reader));
            }
            WriteToFile();
        }

        /// <summary>
        /// The start of a texture file (.TIM) in DW2 can be either a "normal" TIM file starting with 10 00 00 00
        /// or it can be modified to have a header containing information on how the texture is tiled.
        /// </summary>
        /// <param name="reader">The binreader to read from</param>
        /// <returns>The position of the original TIM header, -1 if no TIM header is found</returns>
        private int GetTIMOffset(ref BinaryReader reader)
        {
            int fileIdentifier = reader.ReadInt32();

            if (fileIdentifier == 0x10)
            {
                DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Found TIM Identifier at start of file");
                return fileIdentifier;
            }
            else
            {
                reader.BaseStream.Position = fileIdentifier;
                fileIdentifier = reader.ReadInt32();
                if (fileIdentifier == 0x10)
                {
                    DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Found TIM Identifier after following pointer 0x{reader.BaseStream.Position - 4:X8}");
                    return (int)reader.BaseStream.Position - 4;
                }
                else
                {
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No TIM file indentifier was found, terminating.");
                    return -1;
                }
            }
        }

        /// <summary>
        /// Get all the pointers that are in the header of the model texture
        /// </summary>
        private int[] GetHeaderPointers(ref BinaryReader reader)
        {
            List<int> pointers = new List<int>();
            bool readingPointers = true;

            while (readingPointers)
            {
                var pointer = reader.ReadInt32();
                if (pointer == 0x00)
                {
                    readingPointers = false;
                    continue;
                }
                pointers.Add(pointer);
            }
            return pointers.ToArray();
        }

        private IModelTextureSegment GetSegmentData(int index, ref BinaryReader reader)
        {
            reader.BaseStream.Position = HeaderPointersOrdered[index];
            int currentOffset = (int)reader.BaseStream.Position;

            int itemsInSegmentCount = reader.ReadInt32();
            //Sometimes it seems like the segment starts with 0x00 rather than end it
            if (itemsInSegmentCount == 0x00)
                itemsInSegmentCount = reader.ReadInt32();

            int dataStartIndex = (int)reader.BaseStream.Position;

            int nextHeaderOffset;
            if (index + 1 >= HeaderPointersOrdered.Length)
                nextHeaderOffset = TimOffset;
            else
                nextHeaderOffset = HeaderPointersOrdered[index + 1];

            //Sometimes there is a section of data in the header that doens't have a pointer, so to try and catch these segments
            //we look ahead a bit and see if we find any 0x00's that might terminate the current list
            //It seems like the 6 byte arrays can have double 00's in it causing a false positive. We stop this by chacking for the 0xF000 delimiter
            bool checkForNullTerminator = false;
            while (reader.BaseStream.Position < nextHeaderOffset)
            {
                int value = reader.ReadInt32();

                if (value == 0x3F00)
                    checkForNullTerminator = true;
                if (value == 0x00 && checkForNullTerminator)
                {
                    DigimonWorld2ToolForm.Main.AddWarningToLogWindow($"Found 0x00 terminator at index {index} pointer {reader.BaseStream.Position - 4:X6}, setting as new end of list");
                    nextHeaderOffset = (int)(reader.BaseStream.Position - 4);
                }
            }
            reader.BaseStream.Position = dataStartIndex;

            int segmentLength = nextHeaderOffset - dataStartIndex;
            //We cast one of the ints to a float here, since the result might be a float is the calculation is off
            float itemLength = (float)segmentLength / itemsInSegmentCount;

            if (Math.Abs(itemLength % 1) >= (Double.Epsilon * 100))
            {
                DigimonWorld2ToolForm.Main.AddWarningToLogWindow($"itemLength {itemLength} was not a round number at index {index}, address {HeaderPointersOrdered[index]:X6}. Rounding to 6");
                if ((int)itemLength == 6)
                {
                    itemLength = (int)itemLength;
                }
            }


            byte[,] data = new byte[itemsInSegmentCount, (int)itemLength];
            IModelTextureSegment itemsInSegment = null;

            switch (itemLength)
            {
                case 6:
                    //It appears that the 6 lenght array always starts of with 2x 0x00, unsure if this is some kind of padding
                    //Or those arrays just always start with double 00's... Both are equally likely right now.
                    reader.ReadByte();
                    reader.ReadByte();

                    for (int i = 0; i < itemsInSegmentCount; i++)
                    {
                        for (int j = 0; j < (int)itemLength; j++)
                        {
                            data[i, j] = reader.ReadByte();
                        }
                    }
                    itemsInSegment = new ModelHeaderSegmentA(HeaderPointersOrdered[index], itemsInSegmentCount, data);
                    break;

                case 16:
                    for (int i = 0; i < itemsInSegmentCount; i++)
                    {
                        for (int j = 0; j < (int)itemLength; j++)
                        {
                            data[i, j] = reader.ReadByte();
                        }
                    }
                    itemsInSegment = new ModelHeaderSegmentB(HeaderPointersOrdered[index], itemsInSegmentCount, data);
                    break;
                case 20:
                    for (int i = 0; i < itemsInSegmentCount; i++)
                    {
                        for (int j = 0; j < (int)itemLength; j++)
                        {
                            data[i, j] = reader.ReadByte();
                        }
                    }
                    itemsInSegment = new ModelHeaderSegmentC(HeaderPointersOrdered[index], itemsInSegmentCount, data);
                    break;

                default:
                    DigimonWorld2ToolForm.Main.AddWarningToLogWindow($"Unhandled case for Model header, itemLength {itemLength}");
                    break;
            }

            return itemsInSegment;
        }

        private void WriteToFile()
        {
            string filePath = DigimonWorld2ToolForm.Main.SelectedTextureLabel.Text;
            string fileName = filePath.Substring(filePath.LastIndexOf("\\"), filePath.Length - filePath.LastIndexOf("\\"));
            string target = @"D:\Dev\C#\DigimonWorld2MapVisualizer\DigimonWorld2Tool\DigimonWorld2Tool\bin\Debug\netcoreapp3.1\Output";
            using (StreamWriter writer = new StreamWriter($"{target}{fileName}_ParsedHeader.txt"))
            {
                writer.WriteLine($"Header for: {DigimonWorld2ToolForm.FilePathToSelectedTexture}");
                int index = 1;

                foreach (var segment in SegmentsInHeader)
                {
                    writer.WriteLine();
                    writer.WriteLine($"[Address: 0x{segment.Address:X4}]");
                    writer.WriteLine($"[Length: 0x{segment.ArrayLength:X4} ({segment.ArrayLength})]");
                    if (segment.ArrayItemLength == 6)
                        writer.WriteLine("[00 00] //Unsure if this is some kind of padding or the actual start of the array");

                    for (int i = 0; i < segment.Data.GetLength(0); i++)
                    {
                        for (int j = 0; j < segment.Data.GetLength(1); j++)
                        {
                            if (j > 0)
                            {
                                switch (segment.ArrayItemLength)
                                {
                                    case 6:
                                        if (j % 2 == 0)
                                            writer.Write(" ");
                                        break;

                                    case 16:
                                    case 20:
                                        if (j % 4 == 0)
                                            writer.Write(" ");
                                        if (j % 8 == 0)
                                            writer.Write(" ");
                                        break;

                                    default:
                                        DigimonWorld2ToolForm.Main.AddErrorToLogWindow("Default case hit while writing segments");
                                        break;
                                }
                            }
                            writer.Write($"{segment.Data[i, j]:X2} ");
                        }
                        writer.Write(Environment.NewLine);
                    }
                    index++;
                }
            }
        }
    }
}
