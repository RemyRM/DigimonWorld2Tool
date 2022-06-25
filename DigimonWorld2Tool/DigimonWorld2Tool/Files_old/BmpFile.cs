using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.Files
{
    class BmpFile
    {
        private const int FILEHEADERSIZE = 14;

        internal BmpFileHeader FileHeader { get; }
        internal DIBHeader DetailInfoHeader { get; }
        internal byte[] ColourTableData { get; }
        internal Color[] Clut { get; }
        internal byte[] PixelData { get; }

        public BmpFile(byte[] data)
        {
            using (BinaryReader bReader = new BinaryReader(new MemoryStream(data)))
            {
                FileHeader = new BmpFileHeader(data[0..FILEHEADERSIZE]);
                bReader.BaseStream.Position = FILEHEADERSIZE;

                uint DibHeaderSize = bReader.ReadUInt32();
                DetailInfoHeader = new DIBHeader(data[(FILEHEADERSIZE + 4)..(int)(FILEHEADERSIZE + DibHeaderSize)], DibHeaderSize);
                bReader.BaseStream.Position = FILEHEADERSIZE + DibHeaderSize;

                ColourTableData = new byte[(2 << DetailInfoHeader.bV4BitCount) * 2];
                for (int i = 0; i < ColourTableData.Length; i += 4)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        ColourTableData[i + j] = bReader.ReadByte();
                    }
                }

                Clut = new Color[DetailInfoHeader.bV4BitCount * 4];
                for (int i = 0; i < Clut.Length; i++)
                {
                    int dataIndex = i * 4;
                    Color c = (Color)new ColorConverter().ConvertFromString($"#{"ff"}{ColourTableData[dataIndex + 2]:X2}{ColourTableData[dataIndex + 1]:X2}{ColourTableData[dataIndex]:X2}");

                    Clut[i] = c;
                }
                Clut[0] = Color.Transparent;


                PixelData = bReader.ReadBytes(FileHeader.BmpSize - FileHeader.ImageDataOffset);
            }
        }
    }

    /// <summary>
    /// Bitmap File Header
    /// </summary>
    class BmpFileHeader
    {
        internal short HeaderField { get; }
        internal int BmpSize { get; }
        internal short ReservedA { get; }
        internal short ReservedB { get; }
        internal int ImageDataOffset { get; }

        public BmpFileHeader(byte[] data)
        {
            using (BinaryReader bReader = new BinaryReader(new MemoryStream(data)))
            {
                HeaderField = bReader.ReadInt16();
                // HeaderField should be BM, but can be BA, CI, CP, IC, PT
                if (HeaderField != 0x4d42)
                {
                    if (HeaderField != 0x4142 || HeaderField != 0x4943 || HeaderField != 0x5043 || HeaderField != 0x4349 || HeaderField != 0x5450)
                    {
                        Debug.WriteLine("HeaderField did not match any valid values");
                        return;
                    }
                }
                BmpSize = bReader.ReadInt32();
                ReservedA = bReader.ReadInt16();
                ReservedB = bReader.ReadInt16();
                ImageDataOffset = bReader.ReadInt32();
            }
        }
    }

    /// <summary>
    /// Bitmap information header
    /// </summary>
    class DIBHeader
    {
        internal enum HeaderType
        {
            BITMAPCOREHEADER = 12,
            OS22XBITMAPHEADER = 64,
            BITMAPINFOHEADER = 40,
            BITMAPV2INFOHEADER = 52,
            BITMAPV3INFOHEADER = 56,
            BITMAPV4HEADER = 108,
            BITMAPV5HEADER = 124
        }

        internal enum CompressionMethod
        {
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            BI_JPEG = 4,
            BI_PNG = 5,
            BI_ALPHABITFIELDS = 6,
            BI_CMYK = 11,
            BI_CMYKRLE8 = 12,
            BI_CNYJRKE4 = 13
        }

        internal HeaderType bV4Size { get; }
        internal int bV4Width { get; }
        internal int bV4Height { get; }
        internal ushort bV4Planes { get; }
        internal ushort bV4BitCount { get; } // Bits per pixel
        internal CompressionMethod bV4V4Compression { get; }
        internal uint bV4SizeImage { get; }
        internal int bV4XPelsPerMeter { get; }
        internal int bV4YPelsPerMeter { get; }
        internal uint bV4ClrUsed { get; } // Actual used colours
        internal uint bV4ClrImportant { get; }
        internal uint bV4RedMask { get; }
        internal uint bV4GreenMask { get; }
        internal uint bV4BlueMask { get; }
        internal uint bV4AlphaMask { get; }
        internal uint bV4CSType { get; }
        internal byte[] bV4Endpoints { get; } = new byte[36]; //36 bytes?
        internal uint bV4GammaRed { get; }
        internal uint bV4GammaGreen { get; }
        internal uint bV4GammaBlue { get; }

        public DIBHeader(byte[] data, uint headerSize)
        {
            using (BinaryReader bReader = new BinaryReader(new MemoryStream(data)))
            {
                bV4Size = (HeaderType)headerSize;
                if ((int)bV4Size != 108)
                {
                    Debug.WriteLine($"Bmp isn't of type BITMAPV4HEADER, found type {bV4Size} which is not supported");
                    return;
                }

                bV4Width = bReader.ReadInt32();
                bV4Height = bReader.ReadInt32();
                bV4Planes = bReader.ReadUInt16();
                bV4BitCount = bReader.ReadUInt16();
                bV4V4Compression = (CompressionMethod)bReader.ReadUInt32();
                bV4SizeImage = bReader.ReadUInt32();
                bV4XPelsPerMeter = bReader.ReadInt32();
                bV4YPelsPerMeter = bReader.ReadInt32();
                bV4ClrUsed = bReader.ReadUInt32();
                bV4ClrImportant = bReader.ReadUInt32();
                bV4RedMask = bReader.ReadUInt32();
                bV4GreenMask = bReader.ReadUInt32();
                bV4BlueMask = bReader.ReadUInt32();
                bV4AlphaMask = bReader.ReadUInt32();
                bV4CSType = bReader.ReadUInt32();
                bV4Endpoints = bReader.ReadBytes(36);
                bV4GammaRed = bReader.ReadUInt32();
                bV4GammaGreen = bReader.ReadUInt32();
                bV4GammaBlue = bReader.ReadUInt32();
            }
        }
    }
}
