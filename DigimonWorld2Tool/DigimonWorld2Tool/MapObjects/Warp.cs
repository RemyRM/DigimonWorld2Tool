using System;
using System.Drawing;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.Utility;
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

        public IFloorLayoutObject.MapObjectType ObjectType => IFloorLayoutObject.MapObjectType.Warp;
        public readonly WarpType Type;
        public Vector2 Position { get; private set; }

        public Color ObjectColour => Color.Cyan;
        public string ObjectText{ get; private set; }

        public Warp(byte[] data)
        {
            this.Position = new Vector2(data[0], data[1]);
            Type = GetWarpType(data[2]);

            switch (Type)
            {
                case WarpType.Entrance:
                    ObjectText = "E";
                    break;
                case WarpType.Next:
                    ObjectText = "N";
                    break;
                case WarpType.Exit:
                    ObjectText = "X";
                    break;
                default:
                    break;
            }
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
