namespace DigimonWorld2MapVisualizer.Domains
{
    public class DomainFloorTreasureData
    {
        public readonly byte ItemID;
        public readonly byte TrapLevel;
        public readonly byte Unknown1;
        public readonly byte Unknown2;

        public DomainFloorTreasureData(byte[] data)
        {
            ItemID = data[0];
            TrapLevel = data[1];
            Unknown1 = data[2];
            Unknown2 = data[3];
        }
    }
}
