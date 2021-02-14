using System.IO;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Textures.Headers;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2ModelTexture : IDigimonWorld2Texture
    {
        public TextureModelHeader ModelHeader { get; }
        public TIMHeader TimHeader { get; }
        public byte[] TextureData { get; set; }
        public int TextureDataPosition { get; set; } = 0;

        public DigimonWorld2ModelTexture(ref BinaryReader reader)
        {
            ModelHeader = new TextureModelHeader(ref reader);
            reader.BaseStream.Position = ModelHeader.TimOffset;
            TimHeader = new TIMHeader(ref reader);
        }
    }
}
