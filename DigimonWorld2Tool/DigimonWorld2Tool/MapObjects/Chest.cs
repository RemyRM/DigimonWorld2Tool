using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Chest : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly byte[] Item;

        public Chest(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);
            this.Item = data[2..4];
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}";
        }
    }
}
