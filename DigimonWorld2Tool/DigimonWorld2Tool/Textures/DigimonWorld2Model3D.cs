using System.IO;
using DigimonWorld2Tool.Textures.Headers;
using System.Threading;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2Model3D
    {
        public TextureModelHeader ModelHeader { get; }
        public readonly DigimonWorld2Texture Texture;

        public DigimonWorld2Model3D(ref BinaryReader reader)
        {
            ModelHeader = new TextureModelHeader(ref reader);
            reader.BaseStream.Position = ModelHeader.TimOffset;
            Texture = new DigimonWorld2Texture(ref reader, false);

            //new Thread(() => { Model3DWindow window = new Model3DWindow(); }).Start();
        }
    }
}
