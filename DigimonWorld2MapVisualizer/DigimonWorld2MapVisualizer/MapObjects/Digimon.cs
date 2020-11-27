using System;
using System.Collections.Generic;
using System.Text;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Digimon : IFloorLayoutObject
    {
        public enum DigimonPackLevel : byte
        {
            Rookie = 0,
            Champion = 1,
            Ultimate = 2,
            Mega = 4,
        }

        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly DigimonPackLevel Level;

        public Digimon(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);
            Level = (DigimonPackLevel)(Position.x % 3); // For now we just randomly generate the level based on the x position
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}";
        }
    }
}
