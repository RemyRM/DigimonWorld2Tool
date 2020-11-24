using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Chest : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly string Item;
        public readonly string SpawnChance;

        public Chest(IFloorLayoutObject.MapObjectType objectType, string[] data)
        {
            this.ObjectType = objectType;
            this.Position = ReadMapObjectPosition(ref data);
            this.Item = data[2];
            this.SpawnChance = data[3];
        }

        public override string ToString()
        {
            return $"Object \"{ObjectType}\" at position {Position}";
        }
    }
}
