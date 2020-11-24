using System;
using System.Collections.Generic;
using System.Text;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
{
    public class Chest : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public string Item { get; private set; }
        public string SpawnChance { get; private set; }

        public Chest(IFloorLayoutObject.MapObjectType objectType, string[] data)
        {
            this.ObjectType = objectType;
            this.Position = ReadMapObjectPosition(ref data);
            this.Item = data[2];
            this.SpawnChance = data[3];
            Console.Write($"\n{ToString()}");
        }

        public override string ToString()
        {
            return $"Object \"{ObjectType}\" at position {Position}";
        }
    }
}
