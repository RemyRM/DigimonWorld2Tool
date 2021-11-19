using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.FileFormat;
using System.Linq;

namespace DigimonWorld2Tool.FileInterpreter
{
    public enum WarpType : byte
    {
        Entrance = 0,
        Next = 1,
        Exit = 2,
    }

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

    public class DUNGInterpreter
    {
        private static DUNG dungFileToInterpret;
        private static DUNG DungFileToInterpret
        {
            get => dungFileToInterpret;
            set => dungFileToInterpret = value;
        }

        private static DungFloorHeader dungFloorToInterpret;
        private static DungFloorHeader DungFloorToInterpret
        {
            get => dungFloorToInterpret;
            set => dungFloorToInterpret = value;
        }

        public DUNGInterpreter(DUNG fileToInterpret, DungFloorHeader floorToInterpret)
        {
            DungFileToInterpret = fileToInterpret;
            DungFloorToInterpret = floorToInterpret;
        }

        public void UpdateDungFloor(DungFloorHeader floorToInterpret)
        {
            DungFloorToInterpret = floorToInterpret;
        }

        public static string GetFloorName(byte[] data)
        {
            return TextConversion.DigiStringToASCII(data);
        }

        public static EnemySetHeader[] GetDigimonSetHeaders(byte[] possibleSetID)
        {
            EnemySetHeader[] EnemySetHeaders = new EnemySetHeader[4];
            for (int i = 0; i < possibleSetID.Length; i++)
            {
                int id = possibleSetID[i] - 1;
                if (id < 0)
                {
                    EnemySetHeaders[i] = null;
                    continue;
                }

                var enemySetID = DungFloorToInterpret.DigimonEncounterTable[id];
                EnemySetHeaders[i] = Settings.Settings.ENEMYSETFile.EnemySets.FirstOrDefault(o => o.ID == enemySetID);
                if(EnemySetHeaders[i] == null)
                    System.Diagnostics.Debug.WriteLine("Enemyset was somehow null");
            }

            return EnemySetHeaders;
        }

        public static TrapTypeAndLevel GetTrapTypeAndLevelFromData(byte data)
        {
            return new TrapTypeAndLevel(data);
        }

        public static WarpType GetWarpType(byte data)
        {
            return (WarpType)data;
        }

        public static TrapType GetTrapType(byte data)
        {
            return (TrapType)data;
        }

        public static TrapLevel GetTrapLevel(byte data)
        {
            return (TrapLevel)data;
        }
    }

    public class TrapTypeAndLevel
    {
        public byte Type { get; private set; }
        public byte Level { get; private set; }

        public TrapTypeAndLevel(byte data)
        {
            Type = data.GetRightNiblet();
            Level = data.GetLeftNiblet();
        }
    }
}
