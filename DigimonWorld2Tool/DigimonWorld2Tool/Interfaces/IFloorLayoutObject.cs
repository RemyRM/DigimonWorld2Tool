using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2MapVisualizer.Interfaces
{
    public interface IFloorLayoutObject
    {
        public enum MapObjectType : byte
        {
            Warp,
            Chest,
            Trap,
            Digimon
        }

        public MapObjectType ObjectType { get; }
        public Vector2 Position { get; }
    }
}
