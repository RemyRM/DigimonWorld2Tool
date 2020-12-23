using DigimonWorld2MapVisualizer.Utility;
using System.Drawing;

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
        public Color ObjectColour { get; }
        public string ObjectText { get;}
    }
}
