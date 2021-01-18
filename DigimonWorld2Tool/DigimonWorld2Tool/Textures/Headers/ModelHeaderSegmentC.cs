using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.Textures.Headers
{
    class ModelHeaderSegmentC : IModelTextureSegment
    {
        public int Address { get; }
        public int ArrayLength { get; }
        public int ArrayItemLength { get; } = 20;
        public byte[,] Data { get; }

        public ModelHeaderSegmentC(int Address, int ArrayLength, byte[,] Data)
        {
            this.Address = Address;
            this.ArrayLength = ArrayLength;
            this.Data = Data;
        }
    }
}
