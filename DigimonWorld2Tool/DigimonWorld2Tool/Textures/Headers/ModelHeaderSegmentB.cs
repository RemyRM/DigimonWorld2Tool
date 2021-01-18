using DigimonWorld2Tool.Interfaces;

namespace DigimonWorld2Tool.Textures.Headers
{
    class ModelHeaderSegmentB : IModelTextureSegment
    {
        public int Address { get; }
        public int ArrayLength { get; }
        public int ArrayItemLength { get; } = 16;
        public byte[,] Data { get; }

        public ModelHeaderSegmentB(int Address, int ArrayLength, byte[,] Data)
        {
            this.Address = Address;
            this.ArrayLength = ArrayLength;
            this.Data = Data;
        }
    }
}
