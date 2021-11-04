namespace DigimonWorld2Tool.Domains
{
    public class DomainFloorTreasureDataOld
    {
        public readonly byte ItemID;
        public readonly byte TrapLevel;
        public readonly byte Unknown1;
        public readonly byte Unknown2;

        public DomainFloorTreasureDataOld(byte[] data)
        {
            ItemID = data[0];
            TrapLevel = data[1];
            Unknown1 = data[2];
            Unknown2 = data[3];
        }
    }
}
