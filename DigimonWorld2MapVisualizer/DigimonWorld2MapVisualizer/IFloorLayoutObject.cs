using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
{
    public interface IFloorLayoutObject
    {
        public enum MapObjectType
        {
            Warp,
            Chest,
            Trap,
            Digimon
        }

        public MapObjectType ObjectType { get; set; }
        public Vector2 Position { get; set; }
    }
}
