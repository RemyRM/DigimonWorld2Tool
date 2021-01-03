using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.Textures
{
    /// <summary>
    /// The TextureHeader is the entire header that prefixes the TIM header, and contains the information on all texture segments
    /// </summary>
    class TextureHeader
    {
        public readonly int TimOffset;
        public readonly int[] TextureSectionsOffsets;
        public readonly TextureSegmentInformation[] TextureSegments;

        public TextureHeader(ref BinaryReader reader)
        {
            TimOffset = GetTIMOffset(ref reader);
            if (TimOffset == -1 || TimOffset == 4)
                return;

            reader.BaseStream.Position = 4; //Make sure we continue with reading the Texture header, the position will get set to the TIM header otherwise.

            //Gotta check here if the Tim offset doesn't point directly to position 4, as this would indicate a non existing texture header

            TextureSectionsOffsets = GetTextureSectionsOffsets(ref reader);
            if (TextureSectionsOffsets == null)
            {
                reader.BaseStream.Position = TimOffset;
                return;
            }

            DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.Items.Clear();
            TextureSegments = new TextureSegmentInformation[TextureSectionsOffsets.Length];
            for (int i = 0; i < TextureSegments.Length; i++)
            {
                TextureSegments[i] = new TextureSegmentInformation(ref reader, TextureSectionsOffsets[i]);
                DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.Items.Add(i);
            }
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
        /// Get the pointers to all the texture segments contained within this texture map
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>A list of pointers to the texture segments</returns>
        private int[] GetTextureSectionsOffsets(ref BinaryReader reader)
        {
            int firstSectionAddress = reader.ReadInt32();

            //The first pointer is offset by 4 due to the TIM header pointer, and points to the first texture information, all data inbetween is texture pointers
            int sectionsCount = (firstSectionAddress - 4) / 4;

            if (sectionsCount < 0)
                return null;

            int[] sectionsOffsets = new int[sectionsCount];
            sectionsOffsets[0] = firstSectionAddress;

            for (int i = 1; i < sectionsCount; i++)
            {
                sectionsOffsets[i] = reader.ReadInt32();
            }
            DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Found {sectionsCount} sections");

            return sectionsOffsets;
        }
    }

    /// <summary>
    /// Contains information on each individual texture segment, one segment can exist of multiple <see cref="TextureSegmentLayer"/>
    /// </summary>
    class TextureSegmentInformation
    {
        public readonly int SegmentOffset;
        public readonly List<TextureSegmentLayer> Layers = new List<TextureSegmentLayer>();

        public TextureSegmentInformation(ref BinaryReader reader, int offset)
        {
            SegmentOffset = offset;
            reader.BaseStream.Position = SegmentOffset;

            while(reader.ReadInt32() != 0x0000FFFF)
            {
                reader.BaseStream.Position -= 4; // Since we read 4 bytes ahead to check for the delimiter we need to set the position 4 back here
                Layers.Add(new TextureSegmentLayer(ref reader));
            }
        }
    }

    /// <summary>
    /// Contains the information on which part of the texture sheet is used, the position of the texture ons creen, and colour information
    /// </summary>
    class TextureSegmentLayer
    {
        public readonly byte OffsetX;
        public readonly byte OffsetY;
        public readonly short PositionX;
        public readonly short PositionY;
        public readonly short UnknownValue;
        public readonly byte Width;
        public readonly byte Height;
        public readonly short UnknownColour;
        private readonly int Delimiter;

        public TextureSegmentLayer(ref BinaryReader reader)
        {
            OffsetX = reader.ReadByte();
            OffsetY = reader.ReadByte();

            PositionX = reader.ReadInt16();
            PositionY = reader.ReadInt16();

            UnknownValue = reader.ReadInt16();

            Width = reader.ReadByte();
            Height = reader.ReadByte();

            UnknownColour = reader.ReadInt16();

            Delimiter = reader.ReadInt32();
        }

        public void DrawInformationToInformationBox(int offsetPointer)
        {
            DigimonWorld2ToolForm.Main.SetSegmentInformationText(offsetPointer, PositionX, PositionY, OffsetX, OffsetY, UnknownValue, Width, Height, UnknownColour);
        }
    }
}