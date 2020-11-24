using System;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Trap : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public readonly TrapSlot.TrapType Type;
        public readonly TrapSlot[] TrapSlots = new TrapSlot[4];
        private IFloorLayoutObject.MapObjectType trap;
        private string[] item;

        public Vector2 Position { get; private set; }

        public Trap(IFloorLayoutObject.MapObjectType objectType, string[] data)
        {
            this.ObjectType = objectType;
            this.Position = ReadMapObjectPosition(ref data);

            for (int i = 0; i < 4; i++)
            {
                TrapSlots[i] = new TrapSlot(data[i + 2]);// We offset the data by 2 to skip over the first 2 bytes which are the trap its position
                this.Type = TrapSlots[i].Type;
            }
        }

        public override string ToString()
        {
            return $"Object \"{ObjectType}\" at position \"{Position}\"\n{TrapSlots[0]}\n{TrapSlots[1]}\n{TrapSlots[2]}\n{TrapSlots[3]}";
        }
    }

    public class TrapSlot
    {
        public enum TrapType
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

        public enum TrapLevel
        {
            Zero = 0,
            One = 10,
            Two = 20,
            Three = 30,
            Four = 40,
            Five = 50
        }

        public readonly TrapType Type;
        public readonly TrapLevel Level;

        public TrapSlot(string data)
        {
            char[] splitData = data.ToCharArray();
            Level = (TrapLevel)(char.GetNumericValue(splitData[0]) * 10);
            Type = (TrapType)char.GetNumericValue(splitData[1]);
        }

        public override string ToString()
        {
            return $"TrapSlot Type {Type}, Level {Level}";
        }
    }
}
