using DigimonWorld2Tool.Utility;
using System.Drawing;

namespace DigimonWorld2Tool.Interfaces
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
