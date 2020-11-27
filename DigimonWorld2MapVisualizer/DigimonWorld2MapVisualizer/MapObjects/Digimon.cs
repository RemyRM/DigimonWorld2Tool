using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.Utility;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Digimon : IFloorLayoutObject
    {
        public enum DigimonPackLevel : byte
        {
            Rookie = 0,
            Champion = 1,
            Ultimate = 2,
            Mega = 4,
        }

        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public readonly DigimonPackLevel Level;
        public Vector2 Position { get; private set; }
        public readonly DigimonPack[] DigimonPacks = new DigimonPack[2];

        public Digimon(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);
            Level = (DigimonPackLevel)(Position.x % 3); // For now we just randomly generate the level based on the x position
            
            // The last 2 bytes of data contains the data for the digimon packs, 1 byte each.
            for (int i = 0; i < 2; i++)
            {
                DigimonPacks[i] = new DigimonPack(data[i + 2]);
            }
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}, First ID 0x{DigimonPacks[0].PackID:X2}, Second ID 0x{DigimonPacks[1].PackID:X2} ";
        }

        public class DigimonPack
        {
            public readonly byte PackID;
            public readonly string ObjectModelDigimonName;
            public readonly byte occuranceRate;

            public DigimonPack(byte data)
            {
                this.PackID = DomainFloor.CurrentDomainFloor.DigimonPacks[data.GetLeftHalfByte() - 1];
                this.occuranceRate = data.GetRightHalfByte();
            }
        }
    }
}
