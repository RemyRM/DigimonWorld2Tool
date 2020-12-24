using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

using DigimonWorld2Tool;
using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2Tool.Textures
{
    class TIMParser
    {
        public enum BPPCount
        {
            FourBPPNoCLUT = 0,
            EightBPPNoClut = 1,
            SixteenBPPNoClut = 2,
            TwentyFourBPPNoClut = 3,
            FourBPP = 8,
            EightBPP = 9
        }

        public static void CheckForTIMHeader(string filePath)
        {
            DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Attempting to parse texture {filePath}");

            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open));
            int fileIdentifier = reader.ReadInt32();
            if (fileIdentifier == 0x10)
            {
                DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Found TIM Identifier");
                ParseTIMHeader(ref reader);
            }
            else
            {
                int pointerToTIMFile = fileIdentifier;
                reader.BaseStream.Position = pointerToTIMFile;
                fileIdentifier = reader.ReadInt32();
                if (fileIdentifier == 0x10)
                {
                    DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Found TIM Identifier after following pointer 0x{pointerToTIMFile:X8}");
                    ParseTIMHeader(ref reader);
                }
            }
        }

        private static void ParseTIMHeader(ref BinaryReader reader)
        {
            BPPCount bppCount = (BPPCount)reader.ReadInt32();
            int clutLength = reader.ReadInt32();
            int clutDx = reader.ReadInt16();
            int clutDy = reader.ReadInt16();
            int CLUTColourCount = reader.ReadInt16();
            int CLUTPages = reader.ReadInt16();

            Color[] palette = ParseCLUT(ref reader, bppCount);

            int imageByteCount = reader.ReadInt32();
            int imageDx = reader.ReadInt16();
            int imageDy = reader.ReadInt16();
            int imageWidth = reader.ReadInt16();
            int imageHeight = reader.ReadInt16();

            CreateImageBMP(ref reader, palette, bppCount, imageWidth, imageHeight, imageDx, imageDy, imageByteCount);

            reader.Close();
            reader.Dispose();
        }

        private static Color[] ParseCLUT(ref BinaryReader reader, BPPCount bppCount)
        {
            switch (bppCount)
            {
                case BPPCount.FourBPP:
                    Color[] FourBitPallete = new Color[16];
                    for (int i = 0; i < FourBitPallete.Length; i++)
                    {
                        int colorData = reader.ReadInt16();
                        int r = colorData & 0x1F;
                        int g = (colorData & 0x3E0) >> 5;
                        int b = (colorData & 0x7C00) >> 10;
                        int a = (colorData & 8000) >> 15 ^ 0x01; // We need to flip the Alpha bit as it is actually a transparancy bit, that is off or on

                        Color col = Color.FromArgb(a * 255, r * 8, g * 8, b * 8);
                        FourBitPallete[i] = col;
                    }
                    DrawCLUTPalette(FourBitPallete);
                    return FourBitPallete;

                case BPPCount.EightBPP:
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
                    DrawCLUTPalette(eightBitPalette);
                    return eightBitPalette;

                case BPPCount.FourBPPNoCLUT:
                    break;
                case BPPCount.EightBPPNoClut:
                    break;
                case BPPCount.SixteenBPPNoClut:
                    break;
                case BPPCount.TwentyFourBPPNoClut:
                    break;

                default:
                    break;
            }
            return null;
        }

        private static void DrawCLUTPalette(Color[] palette)
        {
            int CLUTColourSize = 10;
            int paletteSizeSqrt = (int)Math.Sqrt(palette.Length);
            Bitmap paletteBmp = new Bitmap(paletteSizeSqrt * CLUTColourSize + CLUTColourSize, paletteSizeSqrt * CLUTColourSize+ CLUTColourSize);

            int colID = 0;
            for (int i = 0; i < paletteSizeSqrt; i++)
            {
                for (int j = 0; j < paletteSizeSqrt; j++)
                {
                    for (int k = 0; k < CLUTColourSize; k++)
                    {
                        for (int l = 0; l < CLUTColourSize; l++)
                        {
                            paletteBmp.SetPixel((i * CLUTColourSize) + k, (j * CLUTColourSize) + l, palette[colID]);
                        }
                    }
                    colID++;
                }
            }
            DigimonWorld2ToolForm.Main.TextureVisualizerPaletteRenderLayer.Image = paletteBmp;
        }

        private static void CreateImageBMP(ref BinaryReader reader, Color[] palette, BPPCount bppCount, int width, int height, int pageX, int pageY, int imageDataCount)
        {
            int texturePageX = pageX / 64; // The file stores the raw offset in VRAM, to translate this to pages we divide by 64. Should be in range 5<>15
            int texturePageY = pageY / 256; // The only spread 2 rows, thus should be either 0 or 1

            if (texturePageX > 15)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageX was greater than 15, or smaller than 0. value: {texturePageX}");

            if (texturePageY > 1)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageY was greater than 1, or smaller than 0. value: {texturePageY}");

            Bitmap imageBmp = null;

            switch (bppCount)
            {
                case BPPCount.FourBPP:
                    width *= 4; // in a 4BPP colour depth we only use a quarter of a byte per pixel, so to get the real width the multiply by 4           

                    imageBmp = new Bitmap(width, height);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x += 2) // We skip every 2nd pixel on the x axis, as we render 2 pixels per byte
                        {
                            byte colourValue = reader.ReadByte();

                            byte leftPixelValue = colourValue.GetLeftHalfByte();
                            imageBmp.SetPixel(x, y, palette[leftPixelValue]);

                            byte rightPixelValue = colourValue.GetRightHalfByte();
                            imageBmp.SetPixel(x + 1, y, palette[rightPixelValue]); //+ 1 to the x to render the right pixel
                        }
                    }
                    break;

                case BPPCount.EightBPP:
                    width *= 2; // Same as with 4, but only half a byte, so we multiply by 2

                    imageBmp = new Bitmap(width, height);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            byte colourValue = reader.ReadByte();
                            imageBmp.SetPixel(x, y, palette[colourValue]);
                        }
                    }
                    break;

                case BPPCount.FourBPPNoCLUT:
                    break;
                case BPPCount.EightBPPNoClut:
                    break;
                case BPPCount.SixteenBPPNoClut:
                    break;
                case BPPCount.TwentyFourBPPNoClut:
                    break;
                default:
                    break;
            }
            DigimonWorld2ToolForm.Main.SelectedTextureRenderLayer.Image = imageBmp;
        }
    }
}
