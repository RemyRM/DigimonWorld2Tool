using System;
using System.Linq;

namespace DigimonWorld2Tool.FileFormat
{
    public class ENEMYSET
    {
        private const int EnemySetDataEntryLength = 100;
        public byte[] RawFileData { get; private set; }
        public EnemySetHeader[] EnemySets { get; private set; }

        public ENEMYSET()
        {
            RawFileData = BinReader.ReadAllBytesInFile(Settings.Settings.ENEMYSETFilePath);
            EnemySets = new EnemySetHeader[RawFileData.Length / EnemySetDataEntryLength];
            for (int i = 0; i < RawFileData.Length; i += EnemySetDataEntryLength)
            {
                EnemySets[i / EnemySetDataEntryLength] = new EnemySetHeader(RawFileData[i..(i + EnemySetDataEntryLength)]);
            }
        }

        public EnemySetHeader GetSetHeaderByCenterDigiID(byte digID)
        {
            return EnemySets.FirstOrDefault(o => o.ID == digID);
        }
    }

    public class EnemySetHeader
    {
        private const int EnemySetSlotDataLength = 30;

        public byte ID { get; private set; }
        public byte Move { get; private set; }
        public short ModelID { get; private set; }
        public byte GiftType { get; private set; }
        public byte EncounterType { get; private set; }
        public short GiftThreshold { get; private set; }
        public EnemySetSlot[] DigimonInSet { get; private set; } = new EnemySetSlot[3];
        public short Padding { get; private set; }

        public EnemySetHeader(byte[] data)
        {
            ID = data[0];
            Move = data[1];
            ModelID = BitConverter.ToInt16(data[2..4]);
            GiftType = data[4];
            EncounterType = data[5];
            GiftThreshold = BitConverter.ToInt16(data[6..8]);

            for (int i = 0; i < DigimonInSet.Length; i++)
            {
                int startAddr = 8 + i * EnemySetSlotDataLength;
                int endAddr = startAddr + EnemySetSlotDataLength;
                DigimonInSet[i] = new EnemySetSlot(data[startAddr..endAddr]);
            }
            Padding = BitConverter.ToInt16(data[^2..^0]);
        }
    }

    public class EnemySetSlot
    {
        private const int ConditionSkillTargetDataLength = 3;

        public short DigimonID { get; private set; }
        public short HP { get; private set; }
        public short MP { get; private set; }
        public short EXP { get; private set; }
        public short BITS { get; private set; }
        public byte Lv { get; private set; }
        public byte Atk { get; private set; }
        public short Def { get; private set; }
        public byte Spd { get; private set; }
        public byte[] Skills { get; private set; } = new byte[3];

        public EnemySkillData[] ConditionSkillTarget { get; private set; } = new EnemySkillData[4];


        public EnemySetSlot(byte[] data)
        {
            DigimonID = BitConverter.ToInt16(data[0..2]);
            HP = BitConverter.ToInt16(data[2..4]);
            MP = BitConverter.ToInt16(data[4..6]);
            EXP = BitConverter.ToInt16(data[6..8]);
            BITS = BitConverter.ToInt16(data[8..10]);
            Lv = data[10];
            Atk = data[11];
            Def = BitConverter.ToInt16(data[12..14]);
            Spd = data[14];
            Skills = data[15..18];

            for (int i = 0; i < ConditionSkillTarget.Length; i++)
            {
                int startAddr = 18 + ConditionSkillTargetDataLength * i;
                int endAddr = startAddr + ConditionSkillTargetDataLength;
                ConditionSkillTarget[i] = new EnemySkillData(data[startAddr..endAddr]);
            }
        }

        /// <summary>
        /// This contains the <see cref="Condition"/> that needs to be met to use the skill
        /// found in the <see cref="Skills"/> array (<see cref="SkillId"/> is an index into that array)
        /// to attack <see cref="Target"/>
        /// </summary>
        public class EnemySkillData
        {
            public byte Condition { get; private set; }
            public byte SkillId { get; private set; }
            public byte Target { get; private set; }

            public EnemySkillData(byte[] data)
            {
                Condition = data[0];
                SkillId = data[1];
                Target = data[2];
            }
        }
    }
}
