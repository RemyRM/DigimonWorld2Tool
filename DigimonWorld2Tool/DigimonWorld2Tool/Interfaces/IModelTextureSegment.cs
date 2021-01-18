using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.Interfaces
{
    interface IModelTextureSegment
    {
        public int Address { get; }
        public int ArrayLength { get; }
        public int ArrayItemLength { get; }
        public byte[,] Data { get; }
    }
}
