using System;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Warp : IFloorLayoutObject
    {
        public enum WarpType : byte
        {
            Entrance = 0,
            Next = 1,
            Exit = 2,
        }

        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public readonly WarpType Type;
        public Vector2 Position { get; private set; }

        public Warp(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);
            Type = GetWarpType(data[2]);
        }

        private WarpType GetWarpType(byte data)
        {
            return (WarpType)data;
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" of type \"{Type}\" at position {Position}";
        }
    }
}
