using System;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Trap : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public readonly TrapSlot.TrapType Type;
        public readonly TrapSlot[] TrapSlots = new TrapSlot[4];

        public Vector2 Position { get; private set; }

        public Trap(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);

            // One trap location has 4 trap "slots" that each have a 25% chance to get picked.
            // These trap slots can be either empty, the same, or of a different level.
            // Technically a different trap type can be included, but this is not found in-game I believe.
            for (int i = 0; i < 4; i++)
            {
                TrapSlots[i] = new TrapSlot(data[i + 2]);// We offset the data by 2 to skip over the first 2 bytes which make up the traps position
                this.Type = TrapSlots[i].Type;
            }
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position \"{Position}\"\n{TrapSlots[0]}\n{TrapSlots[1]}\n{TrapSlots[2]}\n{TrapSlots[3]}";
        }

        public class TrapSlot
        {
            public enum TrapType : byte
            {
                None = 0,
                Swamp = 1,
                Spore = 2,
                Rock = 3,
                Mine = 4,
                Bit_Bug = 5,
                Energy_Bug = 6,
                Return_Bug = 7,
                Memory_bug = 8,
            }
            public enum TrapLevel : byte
            {
                Zero = 0,
                One = 1,
                Two = 2,
                Three = 3,
                Four = 4,
                Five = 5
            }

            public readonly TrapType Type = TrapType.None;
            public readonly TrapLevel Level = TrapLevel.Zero;

            public TrapSlot(byte data)
            {
                if (data == 0) return;

                Level = (TrapLevel)data.GetLeftHalfByte();
                Type = (TrapType)data.GetRightHalfByte();
            }

            public override string ToString()
            {
                return $"TrapSlot Type {Type}, Level {Level}";
            }
        }
    }
}
