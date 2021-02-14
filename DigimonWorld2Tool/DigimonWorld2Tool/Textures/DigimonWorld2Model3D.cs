using System.IO;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Textures.Headers;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2Model3D
    {
        public TextureModelHeader ModelHeader { get; }
        //public TIMHeader TimHeader { get; }
        //public byte[] TextureData { get; set; }
        //public int TextureDataPosition { get; set; } = 0;

        public readonly DigimonWorld2Texture Texture;

        public DigimonWorld2Model3D(ref BinaryReader reader)
        {
            ModelHeader = new TextureModelHeader(ref reader);
            reader.BaseStream.Position = ModelHeader.TimOffset;
            //TimHeader = new TIMHeader(ref reader);
            Texture = new DigimonWorld2Texture(ref reader, false);
        }
    }
}
