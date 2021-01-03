using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.Textures
{
    class TIMHeader
    {
        public enum BitDepth
        {
            FourNoCLUT = 0,
            EightNoClut = 1,
            SixteenNoClut = 2,
            TwentyFourNoClut = 3,
            Four = 8,
            Eight = 9
        }

        private readonly int TimHeader;
        public readonly BitDepth Bpp;
        private readonly int ClutLength;
        private readonly short ClutDx;
        private readonly short ClutDy;
        private readonly short ClutColourCount;
        private readonly short ClutPages;
        public readonly Color[] TimClutPalette;
        public readonly Color[] AlternativeClutPalette;
        public readonly long ImageByteCount;
        private readonly short ImageDx;
        private readonly short ImageDy;
        public readonly short ImageWidth;
        public readonly short ImageHeight;
        private readonly long TextureDataPosition;
        private readonly short texturePageX;
        private readonly short texturePageY;

        public TIMHeader(ref BinaryReader reader)
        {
            TimHeader = reader.ReadInt32();
            Bpp = (BitDepth)reader.ReadInt32();
            ClutLength = reader.ReadInt32();
            ClutDx = reader.ReadInt16();
            ClutDy = reader.ReadInt16();
            ClutColourCount = reader.ReadInt16();
            ClutPages = reader.ReadInt16();

            TimClutPalette = GetTIMClutPalette(ref reader, Bpp);

            ImageByteCount = reader.ReadInt32(); //This length includes the 12 bytes of header data
            ImageDx = reader.ReadInt16();
            ImageDy = reader.ReadInt16();
            ImageWidth = reader.ReadInt16();
            ImageHeight = reader.ReadInt16();

            texturePageX = (short)(ImageDx / 64); // The file stores the raw offset in VRAM, to translate this to pages we divide by 64. Should be in range 5<>15
            texturePageY = (short)(ImageDy / 256); // The only spread 2 rows, thus should be either 0 or 1

            if (texturePageX > 15)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageX was greater than 15, or smaller than 0. value: {texturePageX}");

            if (texturePageY > 1)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageY was greater than 1, or smaller than 0. value: {texturePageY}");


            TextureDataPosition = reader.BaseStream.Position;

            AlternativeClutPalette = GetAlternativeCLUT(ref reader, Bpp, ClutColourCount);

            reader.BaseStream.Position = TextureDataPosition; // Reset the position of the stream to the start of the data
        }

        /// <summary>
        /// Get the colour palette that is stored in the TIM header, this is always 16 for a 4bpp file, and 256 for an 8bpp file
        /// </summary>
        /// <param name="bppCount">Bitdepth for the TIM file</param>
        /// <returns>Colour palette based on the bit depth</returns>
        private Color[] GetTIMClutPalette(ref BinaryReader reader, BitDepth bppCount)
        {
            switch (bppCount)
            {
                case BitDepth.Four:
                    Color[] FourBitPallete = new Color[16];
                    for (int i = 0; i < FourBitPallete.Length; i++)
                    {
                        int colourData = reader.ReadInt16();
                        int r = colourData & 0x1F;
                        int g = (colourData & 0x3E0) >> 5;
                        int b = (colourData & 0x7C00) >> 10;
                        int a = (colourData & 8000) >> 15 ^ 0x01; // We need to flip the Alpha bit as it is actually a transparancy bit, that is off or on

                        Color col = Color.FromArgb(a * 255, r * 8, g * 8, b * 8);
                        FourBitPallete[i] = col;
                    }

                    return FourBitPallete;

                case BitDepth.Eight:
                    Color[] eightBitPalette = new Color[256];
                    for (int i = 0; i < eightBitPalette.Length; i++)
                    {
                        int colorData = reader.ReadInt16();
                        int r = colorData & 0x1F;
                        int g = (colorData & 0x3E0) >> 5;
                        int b = (colorData & 0x7C00) >> 10;
                        int a = (colorData & 8000) >> 15 ^ 0x01; // We need to flip the Alpha bit as it is actually a transparancy bit, that is off or on

                        Color col = Color.FromArgb(a * 255, r * 8, g * 8, b * 8);
                        eightBitPalette[i] = col;
                    }

                    return eightBitPalette;

                default:
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No BitDepth found for bppCount {bppCount}");
                    break;
            }
            return null;
        }

        private Color[] GetAlternativeCLUT(ref BinaryReader reader, BitDepth bppCount, int CLUTColourCount)
        {
            switch (bppCount)
            {
                case BitDepth.Four:

                    reader.BaseStream.Position = reader.BaseStream.Length - (CLUTColourCount * 2);
                    Color[] fourBitPalette = new Color[CLUTColourCount];
                    for (int i = 0; i < fourBitPalette.Length; i++)
                    {
                        int colourData = reader.ReadInt16();
                        if (DigimonWorld2ToolForm.Main.InvertCLUTColoursCheckbox.Checked)
                            colourData = ~colourData;

                        int r = colourData & 0x1F;
                        int g = (colourData & 0x3E0) >> 5;
                        int b = (colourData & 0x7C00) >> 10;
                        int a = (colourData & 8000) >> 15 ^ 0x01; // We need to flip the Alpha bit back as it is actually a transparancy bit, that is off or on

                        Color col = Color.FromArgb(a * 255, r * 8, g * 8, b * 8);

                        fourBitPalette[i] = col;
                    }
                    if (DigimonWorld2ToolForm.Main.CLUTFirstColourTransparantCheckbox.Checked)
                    {
                        fourBitPalette[(int)DigimonWorld2ToolForm.Main.CLUTOffsetUpDown.Value] = Color.Transparent;

                        fourBitPalette[0] = Color.Transparent;
                    }

                    return fourBitPalette;

                case BitDepth.Eight:
                    DigimonWorld2ToolForm.Main.AddWarningToLogWindow($"Bitdepth is set to 8. Returning the actual TIM CLUT instead as the game doesn't" +
                                                                     $"seem to include an alternative CLUT in this mode.");
                    return TimClutPalette;

                default:
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No BitDepth found for {bppCount}");
                    break;
            }
            return null;
        }
    }
}
