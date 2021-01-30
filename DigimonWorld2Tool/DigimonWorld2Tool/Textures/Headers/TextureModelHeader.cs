using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2Tool.Textures.Headers
{
    class TextureModelHeader
    {
        public readonly int TimOffset;
        public readonly int[] HeaderPointers;
        public readonly List<int> HeaderPointersOrdered = new List<int>();

        public readonly int BonesCount;
        public readonly int[] Bones;
        
        public Vector2[] EyeTextureAnimationOffsets;

        public List<IModelTextureSegment> SegmentsInHeader { get; private set; } = new List<IModelTextureSegment>();

        public TextureModelHeader(ref BinaryReader reader)
        {
            TimOffset = GetTIMOffset(ref reader);
            reader.BaseStream.Position = 0x04; //Ensure we reset the position of the stream to the model texture
            if (reader.ReadInt32() != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No 0x00 terminator found after TIM pointer.");
                return;
            }

            BonesCount = reader.ReadInt32();

            HeaderPointers = GetHeaderPointers(ref reader);
            HeaderPointersOrdered = HeaderPointers.OrderBy(i => i).ToList();

            Bones = GetBones(ref reader);
            EyeTextureAnimationOffsets = GetEyeTextureAnimationOffsets(ref reader);

            for (int i = 0; i < HeaderPointersOrdered.Count; i++)
            {
                GetSegmentData(i, ref reader);
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
        /// Get all the pointers that are in the header of the model texture.
        /// This always seems to be 3x the amount of bones found in the model.
        /// </summary>
        private int[] GetHeaderPointers(ref BinaryReader reader)
        {
            List<int> pointers = new List<int>();

            for (int i = 0; i < BonesCount * 3; i++)
            {
                pointers.Add(reader.ReadInt32());
            }

            return pointers.ToArray();
        }

        /// <summary>
        /// Get the values linked to the bones of the texture.
        /// This seems to be some kind of hierarchy indicating how the bones join together, following a structure of (taken from agumon):
        /// - Upper body (0)
        ///   - Head (1)
        ///     - Jaw (2)
        ///   - upper arms (1)
        ///     - lower arms (2)
        ///       - hands (3)
        /// - Lower body (0)
        ///   - Upper legs (1)
        ///     - Lower legs (2)
        ///       - Feet (3)
        /// </summary>
        /// <returns>The int value for each bone</returns>
        private int[] GetBones(ref BinaryReader reader)
        {
            int[] tmp = new int[BonesCount];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = reader.ReadInt32();
            }
            return tmp;
        }

        /// <summary>
        /// In every model file there are 24 bytes dedicated to what seems to be the offset in the texture for each step in the eye blinking animation.
        /// Most of these only seem to be using the first 10 bytes, and the last 4 bytes are delimiter/padding, and inbetween 0xFF filler.
        /// The first 4 bytes are quite unknown to me, and changing it will spawns eyes all over (agumons) body.
        /// the next 6 bytes are 2 bytes each dedicated to 1 step in the blinking animation
        /// bytes 4..5 X and Y offset of the open eye
        /// bytes 6..7 X and Y offset of the half open eye
        /// bytes 8..9 X and Y offset of the closed eye
        /// </summary>
        /// <returns>A vector 2 containing the X and Y value of the eye offset in the texture</returns>
        private Vector2[] GetEyeTextureAnimationOffsets(ref BinaryReader reader)
        {
            Vector2[] tmp = new Vector2[12];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i].x = reader.ReadByte();
                tmp[i].y = reader.ReadByte();
            }
            return tmp;
        }

        private void GetSegmentData(int index, ref BinaryReader reader)
        {
            reader.BaseStream.Position = HeaderPointersOrdered[index];
            int currentOffset = (int)reader.BaseStream.Position;

            int itemsInSegmentCount = reader.ReadInt32();
            //Sometimes it seems like the segment starts with 0x00 rather than end it
            if (itemsInSegmentCount == 0x00)
                itemsInSegmentCount = reader.ReadInt32();

            int dataStartIndex = (int)reader.BaseStream.Position;

            int nextHeaderOffset;
            if (index + 1 >= HeaderPointersOrdered.Count)
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
                    HeaderPointersOrdered.Insert(index + 1, (int)reader.BaseStream.Position);
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

            CreateHeaderSegment(index, itemsInSegmentCount, itemLength, ref reader);
        }

        private void CreateHeaderSegment(int index, int itemsInSegmentCount, float itemLength, ref BinaryReader reader)
        {
            byte[,] data = new byte[itemsInSegmentCount, (int)itemLength];
            IModelTextureSegment itemsInSegment = null;

            switch (itemLength)
            {
                case 6:
                    //It appears that the 6 length array always starts of with 2x 0x00, unsure if this is some kind of padding
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
            SegmentsInHeader.Add(itemsInSegment);
        }

        private void WriteToFile()
        {
            string filePath = DigimonWorld2ToolForm.Main.SelectedTextureLabel.Text;
            string fileName = filePath.Substring(filePath.LastIndexOf("\\"), filePath.Length - filePath.LastIndexOf("\\"));
            string target = @"D:\Dev\C#\DigimonWorld2MapVisualizer\DigimonWorld2Tool\DigimonWorld2Tool\bin\Debug\netcoreapp3.1\Output";

            using (StreamWriter writer = new StreamWriter($"{target}{fileName}_ParsedHeader.txt"))
            {
                writer.WriteLine($"Header for: {DigimonWorld2ToolForm.FilePathToSelectedTexture}");

                writer.Write(Environment.NewLine);
                WriteIndex(writer);
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Pointer to TIM header]");
                PrintIntAsFourByteHex(TimOffset, writer);
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Likely terminator]");
                writer.WriteLine($"00 00 00 00");
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Bone count]");
                PrintIntAsFourByteHex(BonesCount, writer);
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Pointers list] //This seems to always be 3x the bones count");
                PrintIntArrayAsFourByteHex(HeaderPointers, writer);
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Bones]");
                PrintIntArrayAsFourByteHex(Bones, writer);
                writer.Write(Environment.NewLine);

                writer.WriteLine($"[Eye blinking animation]//This appears to be the offset in the TIM for each step of the blinking animation");
                for (int i = 0; i < EyeTextureAnimationOffsets.Length; i++)
                {
                    writer.Write(EyeTextureAnimationOffsets[i].ToStringHex());
                    writer.Write(" ");

                    if (i % 2 == 1)
                        writer.Write(" ");
                    if (i % 4 == 3)
                        writer.Write(" ");
                    if (i % 8 == 7)
                        writer.Write(Environment.NewLine);
                }
                writer.Write(Environment.NewLine);
                writer.Write(Environment.NewLine);

                //Write each header segment in its hex representation, formatted to what is likely the length of each item
                int segmentIndex = 1;
                foreach (var segment in SegmentsInHeader)
                {
                    writer.WriteLine($"[Address: 0x{segment.Address:X6}]");
                    writer.WriteLine($"[Length: 0x{segment.ArrayLength:X6} ({segment.ArrayLength})]");
                    // The [00 00] bytes added here should be in the data array, but arn't. 
                    if (segment.ArrayItemLength == 6)
                        writer.WriteLine("[00 00] //Unsure if this is some kind of padding or the actual start of the array");
                    if (segment.ArrayItemLength == 16)
                        writer.WriteLine("//Note: This array has no entry in the pointer list, and might be a nested array in the above header segment");

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
                    segmentIndex++;
                    writer.Write(Environment.NewLine);
                }
            }
        }

        private void PrintIntAsFourByteHex(int value, StreamWriter writer, bool newLine = true)
        {
            string hexValueBigEndian = $"{value:X8}";
            string printValue = "";
            for (int j = hexValueBigEndian.Length; j > 0; j -= 2)
            {
                printValue += hexValueBigEndian.Substring(j - 2, 1);
                printValue += hexValueBigEndian.Substring(j - 1, 1);
                printValue += " ";
            }
            if (newLine)
                writer.WriteLine(printValue);
            else
                writer.Write(printValue);
        }

        private void PrintIntArrayAsFourByteHex(int[] intArray, StreamWriter writer)
        {
            for (int i = 0; i < intArray.Length; i++)
            {
                string hexValueBigEndian = $"{intArray[i]:X8}";
                string printValue = "";
                for (int j = hexValueBigEndian.Length; j > 0; j -= 2)
                {
                    printValue += hexValueBigEndian.Substring(j - 2, 1);
                    printValue += hexValueBigEndian.Substring(j - 1, 1);
                    printValue += " ";
                }
                writer.Write(printValue);
                writer.Write(" ");

                if (i % 2 == 1)
                    writer.Write(" ");
                if (i % 4 == 3)
                    writer.Write(Environment.NewLine);
            }
        }

        private void WriteIndex(StreamWriter writer)
        {
            writer.WriteLine("Values are annotated with what I think they are used for, indicated by the [ ].");
            writer.WriteLine("If the start of a (group of) values can be found by following a pointer, the hex value of that pointer will be shown.");
            writer.WriteLine("Hex values will always be prefixed by \'0x\', decimal values will be inbetween ( ).");
        }
    }
}
