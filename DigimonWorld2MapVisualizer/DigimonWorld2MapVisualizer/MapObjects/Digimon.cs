using System;
using System.Collections.Generic;
using System.Text;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Digimon : IFloorLayoutObject
    {
        public enum DigimonPackLevel
        {
            Rookie = 0,
            Champion = 1,
            Ultimate = 2,
            Mega = 4,
        }

        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly DigimonPackLevel Level;

        public Digimon(IFloorLayoutObject.MapObjectType objectType, string[] data)
        {
            this.ObjectType = objectType;
            Position = ReadMapObjectPosition(ref data);
            Level = (DigimonPackLevel)(Position.x % 3);
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}";
        }
    }
}
