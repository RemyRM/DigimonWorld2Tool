using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace DigimonWorld2Tool.FileFormats
{
    public class SLUS
    {
        public byte[] RawFileData { get; }
        private int[] LBA { get; set; }
        private short[] LBASize { get; set; }

        public SLUS()
        {
            RawFileData = BinReader.ReadAllBytesInFile(Settings.Settings.SLUSFilePath);
            Stream stream = new MemoryStream(RawFileData);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                ReadLBATable(reader);
                ReadLBASizeTable(reader);
            }
        }

        private void ReadLBATable(BinaryReader reader)
        {
            //Right now its all hard coded adresses, would be nice to find what points to this so we can do it dynamically
            reader.BaseStream.Position = 0x33F94;
            LBA = new int[0xE5B];

            int i = 0;
            while (reader.BaseStream.Position < 0x378FF)
            {
                LBA[i] = reader.ReadInt32();
                i++;
            }
        }


        private void ReadLBASizeTable(BinaryReader reader)
        {
            //Right now its all hard coded adresses, would be nice to find what points to this so we can do it dynamically
            reader.BaseStream.Position = 0x37900;

            LBASize = new short[0xE5B];
            int i = 0;
            while (reader.BaseStream.Position < 0x395B5)
            {
                LBASize[i] = reader.ReadInt16();
                i++;
            }
        }

        public long GetAddressAtLBAIndex(int index)
        {
            return LBA[index] * 2352;
        }
    }
}
