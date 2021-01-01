using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.Textures
{
    class DigimonWorld2Texture
    {
        public readonly TextureHeader textureHeader;
        public readonly TIMHeader timHeader;

        public DigimonWorld2Texture(ref BinaryReader reader)
        {
            textureHeader = new TextureHeader(ref reader);
            timHeader = new TIMHeader(ref reader);
        }
    }
}
