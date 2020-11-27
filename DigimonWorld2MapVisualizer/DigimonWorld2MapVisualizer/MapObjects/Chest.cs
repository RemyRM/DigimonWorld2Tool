using DigimonWorld2MapVisualizer.Interfaces;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Chest : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly byte Item;
        public readonly byte SpawnChance;

        public Chest(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);
            this.Item = data[2];
            this.SpawnChance = data[3];
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}";
        }
    }
}
