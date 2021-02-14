using System;
using System.Drawing;
using System.IO;
using DigimonWorld2MapTool.Utility;
using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.Textures
{
    class TextureParser
    {
        public enum TextureType
        {
            Generic,
            Model,
        }

        public enum BPPCount
        {
            FourBPPNoCLUT = 0,
            EightBPPNoClut = 1,
            SixteenBPPNoClut = 2,
            TwentyFourBPPNoClut = 3,
            FourBPP = 8,
            EightBPP = 9
        }

        public static DigimonWorld2GeneralTexture CurrentTexture;
        private static Color[] palette;
        private static int TextureScaleSize = 1;


        public static void CheckForTIMHeader(string filePath)
        {
            DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Attempting to parse texture {filePath}");

            if (!File.Exists(filePath))
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No file found at \"{filePath}\"");
                return;
            }

            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open));

            switch ((TextureType)DigimonWorld2ToolForm.Main.TextureTypeComboBox.SelectedIndex)
            {
                case TextureType.Generic:
                    CheckForGenericTexture(ref reader);
                    break;

                case TextureType.Model:
                    CheckForModelTexture(ref reader);
                    reader.Close();
                    reader.Dispose();
                    return;
                    break;

                default:
                    break;
            }

            reader.Close();
            reader.Dispose();

            if (CurrentTexture.TextureHeader.TextureSectionsOffsets != null && CurrentTexture.TextureHeader.TextureSectionsOffsets.Length > 0)
            {
                DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.Enabled = true;
                DigimonWorld2ToolForm.Main.TextureLayerSelectComboBox.Enabled = true;
                DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.SelectedIndex = 0;
            }
            else
            {
                DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.Enabled = false;
                DigimonWorld2ToolForm.Main.TextureLayerSelectComboBox.Enabled = false;
            }
        }

        private static void CheckForGenericTexture(ref BinaryReader reader)
        {
            CurrentTexture = new DigimonWorld2GeneralTexture(ref reader);
            if (CurrentTexture.TimHeader == null)
                return;

            palette = DigimonWorld2ToolForm.Main.TextureUseAltClutCheckbox.Checked ? CurrentTexture.TimHeader.AlternativeClutPalette :
                                                                                             CurrentTexture.TimHeader.TimClutPalette;
            if (palette == null)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No Palette was found, terminating.");
                reader.Close();
                reader.Dispose();
                return;
            }

            DrawCLUTPalette(palette);
            DrawTextureBMP(ref reader, palette, CurrentTexture);
        }

        private static void CheckForModelTexture(ref BinaryReader reader)
        {
            var CurrentTexture = new DigimonWorld2ModelTexture(ref reader);
            if (CurrentTexture.TimHeader == null)
                return;

            palette = DigimonWorld2ToolForm.Main.TextureUseAltClutCheckbox.Checked ? CurrentTexture.TimHeader.AlternativeClutPalette :
                                                                                             CurrentTexture.TimHeader.TimClutPalette;
            if (palette == null)
            {
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"No Palette was found, terminating.");
                reader.Close();
                reader.Dispose();
                return;
            }

            DrawCLUTPalette(palette);
        }

        /// <summary>
        /// Draw the colour palette based on the alternative clut checkbox
        /// </summary>
        /// <param name="palette">The palette to draw</param>
        private static void DrawCLUTPalette(Color[] palette)
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

        /// <summary>
        /// Draw a BMP image of the selected texture based on the raw data given in the parameters
        /// </summary>
        private static void DrawTextureBMP(ref BinaryReader reader, Color[] palette, BPPCount bppCount, int width, int height, int pageX, int pageY, int imageDataCount, int CLUTColourCount)
        {
            TextureScaleSize = DigimonWorld2ToolForm.Main.ScaleTextureToFitCheckbox.Checked ? 2 : 1;

            int texturePageX = pageX / 64; // The file stores the raw offset in VRAM, to translate this to pages we divide by 64. Should be in range 5<>15
            int texturePageY = pageY / 256; // The only spread 2 rows, thus should be either 0 or 1

            if (texturePageX > 15)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageX was greater than 15, or smaller than 0. value: {texturePageX}");

            if (texturePageY > 1)
                DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"TexturePageY was greater than 1, or smaller than 0. value: {texturePageY}");

            Bitmap imageBmp = null;
            int CLUTOffset = (int)DigimonWorld2ToolForm.Main.CLUTOffsetUpDown.Value;

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

        /// <summary>
        /// Draw a BMP image of the selected texture of the given texture
        /// </summary>
        /// <param name="reader">The stream to read the bytes from</param>
        /// <param name="palette">The colour palette to use</param>
        /// <param name="texture">The texture to draw</param>
        private static void DrawTextureBMP(ref BinaryReader reader, Color[] palette, DigimonWorld2GeneralTexture texture)
        {
            TextureScaleSize = DigimonWorld2ToolForm.Main.ScaleTextureToFitCheckbox.Checked ? 2 : 1;

            Bitmap imageBmp = null;
            int CLUTOffset = (int)DigimonWorld2ToolForm.Main.CLUTOffsetUpDown.Value;

            switch (texture.TimHeader.Bpp)
            {
                case TIMHeader.BitDepth.Four:
                    {
                        var width = texture.TimHeader.ImageWidth * 4; // in a 4BPP colour depth we only use a quarter of a byte per pixel, so to get the real width the multiply by 4           
                        var height = texture.TimHeader.ImageHeight;
                        imageBmp = new Bitmap(width * TextureScaleSize, height * TextureScaleSize);

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x += 2) // We skip every 2nd pixel on the x axis, as we render 2 pixels per byte
                            {
                                byte colourValue = reader.ReadByte();
                                CurrentTexture.AddByteToTextureData(colourValue);

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
                    }
                    break;

                case TIMHeader.BitDepth.Eight:
                    {
                        var width = texture.TimHeader.ImageWidth * 2; // in a 8BPP colour depth we only use a half of a byte per pixel, so to get the real width the multiply by 2          
                        var height = texture.TimHeader.ImageHeight;

                        imageBmp = new Bitmap(width, height);

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                byte colourValue = reader.ReadByte();
                                imageBmp.SetPixel(x, y, palette[colourValue]);
                            }
                        }
                    }
                    break;
            }
            DigimonWorld2ToolForm.Main.SelectedTextureRenderLayer.Image = imageBmp;
        }

        /// <summary>
        /// Create a bitmap for a specific segment of the total texture sheet
        /// </summary>
        public static Bitmap CreateTextureSegmentBMP()
        {
            if (CurrentTexture == null)
                return null;

            var segmentLayer = CurrentTexture.TextureHeader.TextureSegments[DigimonWorld2ToolForm.Main.TextureSegmentSelectComboBox.SelectedIndex].Layers[DigimonWorld2ToolForm.Main.TextureLayerSelectComboBox.SelectedIndex];

            var width = segmentLayer.Width;
            var height = segmentLayer.Height;

            // A texture file is always 64 bytes wide, multiply this by the Y offset to get the start ID in the data array
            // We need to add half the offsetX because 1 byte is 2 pixels
            int startSegmentDataID = segmentLayer.OffsetY * 64 + (segmentLayer.OffsetX / 2);
            int endSegmentDataID = startSegmentDataID + (64 * height);

            byte[] textureSegmentData = CurrentTexture.TextureData[startSegmentDataID..endSegmentDataID];
            MemoryStream stream = new MemoryStream(textureSegmentData);
            BinaryReader reader = new BinaryReader(stream);

            Bitmap imageBmp = new Bitmap(width * 2 + 4, height * 2); // * 2 for upscaling the texture, + 4 because... I don't know honestly. But it works ¯\_(ツ)_/¯

            int CLUTOffset = (int)DigimonWorld2ToolForm.Main.CLUTOffsetUpDown.Value;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < 128; x += 2)
                {
                    // The width of the texture is 64 bytes (with 2 pixels per byte), but not all textures take up the full width, 
                    // so we need to throw away excess bytes when we're past the width of the texture segment
                    if (x >= width)
                    {
                        reader.ReadByte();
                        continue;
                    }

                    byte colourValue = reader.ReadByte();

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
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
            return imageBmp;
        }
    }
}
