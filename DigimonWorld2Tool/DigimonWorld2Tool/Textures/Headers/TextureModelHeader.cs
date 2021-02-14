using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DigimonWorld2MapTool.Utility;

namespace DigimonWorld2Tool.Textures.Headers
{
    class TextureModelHeader
    {
        public readonly int TimOffset;
        public readonly int timOffsetTerminator;
        public readonly int[] HeaderPointers;
        public readonly int[] HeaderPointersOrdered;

        public readonly int BonesCount;
        public readonly int[] Bones;

        public readonly bool OrderDataByPointerValue = true;

        public Vector2[] EyeTextureAnimationOffsets;

        public readonly ModelBodyPartHeader[] BodyPartsHeader;

        public TextureModelHeader(ref BinaryReader reader)
        {
            TimOffset = GetTIMOffset(ref reader);
            reader.BaseStream.Position = 0x04; //Ensure we reset the position of the stream to the model texture
            timOffsetTerminator = reader.ReadInt32();
            if (timOffsetTerminator != 0x00)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No 0x00 terminator found after TIM pointer.");
                return;
            }

            BonesCount = reader.ReadInt32();

            HeaderPointers = GetHeaderPointers(ref reader);
            HeaderPointersOrdered = HeaderPointers.OrderBy(i => i).ToArray();

            Bones = GetBones(ref reader);
            EyeTextureAnimationOffsets = GetEyeTextureAnimationOffsets(ref reader);

            BodyPartsHeader = new ModelBodyPartHeader[Bones.Length];
            for (int i = 0; i < BodyPartsHeader.Length; i++)
            {
                BodyPartsHeader[i] = new ModelBodyPartHeader(ref reader);
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

        private void WriteToFile()
        {
            string filePath = DigimonWorld2ToolForm.Main.SelectedTextureLabel.Text;
            string fileName = filePath.Substring(filePath.LastIndexOf("\\"), filePath.Length - filePath.LastIndexOf("\\"));
            string target = @"D:\Dev\C#\DigimonWorld2MapVisualizer\DigimonWorld2Tool\DigimonWorld2Tool\bin\Debug\netcoreapp3.1\Output";
            DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Creating formatted model file: {target}{fileName}_ParsedHeader.txt");

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
                PrintIntAsFourByteHex(timOffsetTerminator, writer);
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

                int[] pointerArray = OrderDataByPointerValue ? HeaderPointersOrdered : HeaderPointers;

                //Write each header segment in its hex representation
                for (int i = 0; i < BodyPartsHeader.Length; i++)
                {
                    var item = BodyPartsHeader[i];
                    writer.WriteLine($"[Address: 0x{pointerArray[i]:X6}]");
                    PrintIntAsFourByteHex(item.VerticalFacesNullByte, writer);
                    PrintIntAsFourByteHex(item.VerticalFaceCount, writer, false);
                    writer.Write($"//Array length");
                    writer.Write(Environment.NewLine);


                    //Write the vertical face data
                    foreach (var faceData in item.VerticalFacesData)
                    {
                        //VertexID[]
                        for (int j = 0; j < faceData.VertexIDs.Length; j++)
                            writer.Write($"{faceData.VertexIDs[j]:X2} ");

                        writer.Write(" ");

                        //VertexColour[]
                        for (int j = 0; j < faceData.VertexColour.Length; j++)
                            writer.Write($"{faceData.VertexColour[j]:X2} ");

                        writer.Write(" ");

                        //TexturePlaneOffset[]
                        for (int j = 0; j < faceData.TexturePlaneOffset.Length; j++)
                        {
                            writer.Write($"{faceData.TexturePlaneOffset[j].x:X2} ");
                            writer.Write($"{faceData.TexturePlaneOffset[j].y:X2} ");
                            if (j == 1)
                                writer.Write(" ");
                        }

                        writer.Write(" ");

                        //Unknowns
                        writer.Write($"{faceData.Unknown1:X2} ");
                        writer.Write($"{faceData.Unknown2:X2} ");
                        writer.Write($"{faceData.Unknown3:X2} ");
                        writer.Write($"{faceData.Unknown4:X2} ");

                        writer.Write(Environment.NewLine);
                    }
                    writer.Write(Environment.NewLine);


                    //Write the horizontal face data
                    PrintIntAsFourByteHex(item.HorizontalFacesNullByte, writer);
                    PrintIntAsFourByteHex(item.HorizontalFaceCount, writer, false);
                    writer.Write($"//Array length");
                    writer.Write(Environment.NewLine);

                    foreach (var faceData in item.HorizontalFacesData)
                    {
                        //VertexID[]
                        for (int j = 0; j < faceData.VertexIDs.Length; j++)
                            writer.Write($"{faceData.VertexIDs[j]:X2} ");

                        writer.Write(" ");

                        //VertexColour[]
                        for (int j = 0; j < faceData.VertexColour.Length; j++)
                            writer.Write($"{faceData.VertexColour[j]:X2} ");

                        writer.Write("  ");

                        //TexturePlaneOffset[]
                        for (int j = 0; j < faceData.TexturePlaneOffset.Length; j++)
                        {
                            writer.Write($"{faceData.TexturePlaneOffset[j].x:X2} ");
                            writer.Write($"{faceData.TexturePlaneOffset[j].y:X2} ");
                            writer.Write(" ");
                        }

                        writer.Write(" ");

                        //Unknowns
                        writer.Write($"{faceData.Unknown1:X2} ");
                        writer.Write($"{faceData.Unknown2:X2} ");
                        writer.Write($"{faceData.Unknown3:X2} ");
                        writer.Write($"{faceData.Unknown4:X2} ");

                        writer.Write(Environment.NewLine);
                    }
                    writer.Write(Environment.NewLine);


                    //Vertical vertex data
                    WriteVertexDataToFile(writer, pointerArray, i, item);

                    //horizontal vertex data
                    WriteVertexDataToFile(writer, pointerArray, i, item);

                    writer.Write(Environment.NewLine);
                }
                writer.Write(Environment.NewLine);
                writer.Write(Environment.NewLine);
            }
        }

        private void WriteVertexDataToFile(StreamWriter writer, int[] pointerArray, int i, ModelBodyPartHeader item)
        {
            writer.WriteLine($"[Address: 0x{pointerArray[i + 1]:X6}]");
            PrintShortAsFourByteHex(item.VerticalVertexAllignmentByte, writer, false);
            writer.Write($"// Vertex allignment byte");
            writer.Write(Environment.NewLine);

            foreach (var vertex in item.VerticalFaceVertexData)
            {
                PrintShortAsFourByteHex((short)vertex.x, writer, false);
                writer.Write(" ");

                PrintShortAsFourByteHex((short)vertex.y, writer, false);
                writer.Write(" ");

                PrintShortAsFourByteHex((short)vertex.z, writer, false);
                writer.Write(Environment.NewLine);
            }

            if (item.VerticalVertexPaddingBytes != null)
            {
                foreach (var padding in item.VerticalVertexPaddingBytes)
                    writer.Write(padding);

                writer.Write($" //Word padding bytes");
                writer.Write(Environment.NewLine);
            }
            writer.Write(Environment.NewLine);
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

        private void PrintShortAsFourByteHex(short value, StreamWriter writer, bool newLine = true)
        {
            string hexValueBigEndian = $"{value:X4}";
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
