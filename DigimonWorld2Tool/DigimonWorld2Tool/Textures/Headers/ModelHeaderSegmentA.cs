using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.Textures.Headers
{
    class ModelHeaderSegmentA : IModelTextureSegment
    {
        public int Address {get;}
        public int ArrayLength { get; }
        public int ArrayItemLength { get; } = 6;
        public byte[,] Data { get; }

        public ModelHeaderSegmentA(int Address, int ArrayLength, byte[,] Data)
        {
            this.Address = Address;
            this.ArrayLength = ArrayLength;
            this.Data = Data;
        }
    }
}
