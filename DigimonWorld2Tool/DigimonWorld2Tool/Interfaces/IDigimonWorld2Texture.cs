using DigimonWorld2Tool.Textures;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.Interfaces
{
    interface IDigimonWorld2Texture
    {
        public TIMHeader TimHeader { get;  }
        public byte[] TextureData { get; set; }
        public int TextureDataPosition { get; set; }
    }
}
