using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2Texture
    {
        public readonly TextureHeader TextureHeader;
        public readonly TIMHeader TimHeader;
        public byte[] TextureData { get; private set; }
        private int TextureDataPosition = 0;

        public DigimonWorld2Texture(ref BinaryReader reader)
        {
            TextureHeader = new TextureHeader(ref reader);
            if (TextureHeader.TimOffset == -1)
                return;

            TimHeader = new TIMHeader(ref reader);
            TextureData = new byte[TimHeader.ImageByteCount - 12];// We need to subtract 12 from the length, as this also includes the header
        }

        public void AddByteToTextureData(byte data)
        {
            TextureData[TextureDataPosition] = data;
            TextureDataPosition++;
        }
    }
}
