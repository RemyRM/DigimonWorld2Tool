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

        private static int TextureScaleSize = 1;

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
                else
                {
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No TIM file indentifier was found");
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

            //Color[] palette = ParseTIMCLUT(ref reader, bppCount);

            Color[] palette = null;
            if (DigimonWorld2ToolForm.Main.TextureUseAltClutCheckbox.Checked)
            {
                var readerPos = reader.BaseStream.Position;
                palette = ParseAlternativeCLUT(ref reader, bppCount, CLUTColourCount);

                reader.BaseStream.Position = readerPos;
                ParseTIMCLUT(ref reader, bppCount); // This is temporary to ensure the BaseStream's position is put past the CLUT colour data
                                                    // Should be done by just adding an int based on the CLUT length
            }
            else
            {
                palette = ParseTIMCLUT(ref reader, bppCount);
            }

            DrawCLUTPalette(palette, CLUTColourCount);

            int imageByteCount = reader.ReadInt32(); // This length includes the 12 bytes of header data
            int imageDx = reader.ReadInt16();
            int imageDy = reader.ReadInt16();
            int imageWidth = reader.ReadInt16();
            int imageHeight = reader.ReadInt16();

            CreateImageBMP(ref reader, palette, bppCount, imageWidth, imageHeight, imageDx, imageDy, imageByteCount, CLUTColourCount);

            reader.Close();
            reader.Dispose();
        }

        private static Color[] ParseAlternativeCLUT(ref BinaryReader reader, BPPCount bppCount, int CLUTColourCount)
        {
            switch (bppCount)
            {
                case BPPCount.FourBPP:

                    reader.BaseStream.Position = 16156;
                    Color[] fourBitPalette = new Color[CLUTColourCount];
                    for (int i = 0; i < fourBitPalette.Length; i++)
                    {
                        int colourData = reader.ReadInt16();
                        int colourDataInverted = ~colourData;

                        int r = colourDataInverted & 0x1F;
                        int g = (colourDataInverted & 0x3E0) >> 5;
                        int b = (colourDataInverted & 0x7C00) >> 10;
                        int a = (colourDataInverted & 8000) >> 15 ^ 0x01; // We need to flip the Alpha bit back as it is actually a transparancy bit, that is off or on

                        Color col = Color.FromArgb(a * 255, r * 8, g * 8, b * 8);

                        fourBitPalette[i] = col;
                    }
                    if (DigimonWorld2ToolForm.Main.CLUTFirstColourTransparantCheckbox.Checked)
                    {
                        fourBitPalette[240] = Color.Transparent;

                        fourBitPalette[0] = Color.Transparent;
                    }

                    return fourBitPalette;
                case BPPCount.EightBPP:
                    break;
                default:
                    break;
            }

            return null;
        }

        private static Color[] ParseTIMCLUT(ref BinaryReader reader, BPPCount bppCount)
        {
            switch (bppCount)
            {
                case BPPCount.FourBPP:
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

                    return eightBitPalette;

                default:
                    break;
            }
            return null;
        }

        private static void DrawCLUTPalette(Color[] palette, int CLUTColourCount)
        {
            int PixelSize = 10;
            Bitmap paletteBmp;

            if (DigimonWorld2ToolForm.Main.TextureUseAltClutCheckbox.Checked)
            {
                paletteBmp = new Bitmap(32 * PixelSize + PixelSize, 8 * PixelSize + PixelSize);

                int colID = 0;
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        for (int k = 0; k < PixelSize; k++)
                        {
                            for (int l = 0; l < PixelSize; l++)
                            {
                                paletteBmp.SetPixel((x * PixelSize) + k, (y * PixelSize) + l, palette[colID]);
                            }
                        }
                        colID++;
                    }
                }
            }
            else
            {
                int paletteSizeSqrt = (int)Math.Sqrt(palette.Length);
                paletteBmp = new Bitmap(paletteSizeSqrt * PixelSize + PixelSize, paletteSizeSqrt * PixelSize + PixelSize);

                int colID = 0;
                for (int y = 0; y < paletteSizeSqrt; y++)
                {
                    for (int x = 0; x < paletteSizeSqrt; x++)
                    {
                        for (int k = 0; k < PixelSize; k++)
                        {
                            for (int l = 0; l < PixelSize; l++)
                            {
                                paletteBmp.SetPixel((x * PixelSize) + k, (y * PixelSize) + l, palette[colID]);
                            }
                        }
                        colID++;
                    }
                }
            }
            DigimonWorld2ToolForm.Main.TextureVisualizerPaletteRenderLayer.Image = paletteBmp;
        }

        private static void CreateImageBMP(ref BinaryReader reader, Color[] palette, BPPCount bppCount, int width, int height, int pageX, int pageY, int imageDataCount, int CLUTColourCount)
        {
            TextureScaleSize = DigimonWorld2ToolForm.Main.ScaleTextureToFitCheckbox.Checked ? 2 : 1;

            int texturePageX = pageX / 64; // The file stores the raw offset in VRAM, to translate this to pages we divide by 64. Should be in range 5<>15
            int texturePageY = pageY / 256; // The only spread 2 rows, thus should be either 0 or 1

            if (texturePageX > 15)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageX was greater than 15, or smaller than 0. value: {texturePageX}");

            if (texturePageY > 1)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageY was greater than 1, or smaller than 0. value: {texturePageY}");

            Bitmap imageBmp = null;
            int CLUTOffset = CLUTColourCount == 256 ? 240 : 0;

            switch (bppCount)
            {
                case BPPCount.FourBPP:

                    width *= 4; // in a 4BPP colour depth we only use a quarter of a byte per pixel, so to get the real width the multiply by 4           
                    imageBmp = new Bitmap(width * TextureScaleSize, height * TextureScaleSize);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x += 2) // We skip every 2nd pixel on the x axis, as we render 2 pixels per byte
                        {
                            byte colourValue = reader.ReadByte();

                            for (int i = 0; i < TextureScaleSize; i++)
                            {
                                for (int j = 0; j < TextureScaleSize; j++)
                                {
                                    if (DigimonWorld2ToolForm.Main.TextureUseAltClutCheckbox.Checked)
                                    {
                                        byte rightPixelValue = colourValue.GetRightHalfByte();
                                        imageBmp.SetPixel((x * TextureScaleSize) + i, (y * TextureScaleSize) + j, palette[CLUTOffset + rightPixelValue]); // The left pixel is the right half byte due to endianess

                                        byte leftPixelValue = colourValue.GetLeftHalfByte();
                                        imageBmp.SetPixel((x * TextureScaleSize) + TextureScaleSize + i, (y * TextureScaleSize) + j, palette[CLUTOffset + leftPixelValue]); //+ 1 to the x to render the right pixel                                     
                                    }
                                    else
                                    {
                                        byte rightPixelValue = colourValue.GetRightHalfByte();
                                        imageBmp.SetPixel((x * TextureScaleSize) + i, (y * TextureScaleSize) + j, palette[rightPixelValue]); // The left pixel is the right half byte due to endianess

                                        byte leftPixelValue = colourValue.GetLeftHalfByte();
                                        imageBmp.SetPixel((x * TextureScaleSize) + TextureScaleSize + i, (y * TextureScaleSize) + j, palette[leftPixelValue]); //+ 1 to the x to render the right pixel
                                    }
                                }
                            }
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
            }
            DigimonWorld2ToolForm.Main.SelectedTextureRenderLayer.Image = imageBmp;
        }
    }
}
