using System;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Warp : IFloorLayoutObject
    {
        public enum WarpType
        {
            Entrance = 0,
            Next = 1,
            Exit = 2,
        }

        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public readonly WarpType Type;
        public Vector2 Position { get; private set; }

        public Warp(IFloorLayoutObject.MapObjectType objectType, string[] data)
        {
            this.ObjectType = objectType;
            Position = ReadMapObjectPosition(ref data);
            Type = GetWarpType(data[2]);
            //Console.Write($"Created: {ToString()}\n");
        }

        private WarpType GetWarpType(string data)
        {
            return (WarpType)Int32.Parse(data);
        }

        public override string ToString()
        {
            return $"Object \"{ObjectType}\" of type \"{Type}\" at position {Position}";
        }
    }
}
