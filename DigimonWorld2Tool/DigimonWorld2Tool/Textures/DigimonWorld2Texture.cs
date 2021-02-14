using DigimonWorld2Tool.Interfaces;
using System.IO;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2Texture
    {
        private readonly int HeaderSize = 12;
        public readonly GeneralTextureSegmentHeader TextureHeader;

        public TIMHeader TimHeader { get; }
        public byte[] TextureData { get; set; }
        public int TextureDataPosition { get; set; }

        public DigimonWorld2Texture(ref BinaryReader reader, bool hasSegmentHeader = true)
        {
            if (hasSegmentHeader)
            {
                TextureHeader = new GeneralTextureSegmentHeader(ref reader);
                if (TextureHeader.TimOffset == -1)
                    return;
            }
            

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
