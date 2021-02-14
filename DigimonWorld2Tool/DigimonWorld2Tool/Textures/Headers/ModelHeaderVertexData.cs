using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Textures.Headers
{
    class ModelHeaderVertexData : IModelTextureSegment
    {
        public int Address {get;}
        public int ArrayLength { get; }
        public int ArrayItemLength { get; } = 6;
        public byte[,] Data { get; }
        public readonly Vector3[] VertexData;

        public ModelHeaderVertexData(int Address, int ArrayLength, byte[,] Data)
        {
            this.Address = Address;
            this.ArrayLength = ArrayLength;
            this.Data = Data;

            VertexData = new Vector3[ArrayLength / 3];
            for (int i = 0; i < ArrayLength; i += 2)
            {
                //byte x = Data[i];
            }
        }
    }
}
