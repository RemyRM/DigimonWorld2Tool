using DigimonWorld2Tool.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2GeneralTexture : IDigimonWorld2Texture
    {
        private readonly int HeaderSize = 12;
        public readonly GeneralTextureSegmentHeader TextureHeader;

        public TIMHeader TimHeader { get; }
        public byte[] TextureData { get; set; }
        public int TextureDataPosition { get; set; }

        //public TIMHeader TimHeader { get; }
        //public byte[] TextureData { get; private set; }
        //private int TextureDataPosition { get; set; } = 0;

        public DigimonWorld2GeneralTexture(ref BinaryReader reader)
        {
            TextureHeader = new GeneralTextureSegmentHeader(ref reader);
            if (TextureHeader.TimOffset == -1)
                return;

            TimHeader = new TIMHeader(ref reader);
            TextureData = new byte[TimHeader.ImageByteCount - HeaderSize];// We need to subtract 12 from the length, as this also includes the header
        }

        public void AddByteToTextureData(byte data)
        {
            TextureData[TextureDataPosition] = data;
            TextureDataPosition++;
        }
    }
}
