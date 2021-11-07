using System;
using System.Linq;

namespace DigimonWorld2Tool.FileFormats
{
    public class MODELDT0
    {
        private const int DigimonModelFilesMappingDataLength = 40;
        private byte[] RawFileData { get; set; }
        public DigimonModelFilesMapping[] DigimonModelMappings { get; private set; }

        public int DigimonModelMappingPointer { get; private set; }
        public int UnknownPointer { get; private set; }

        public MODELDT0()
        {
            RawFileData = BinReader.ReadAllBytesInFile(Settings.Settings.MODELDT0FilePath);

            DigimonModelMappingPointer = BinReader.GetPointer(RawFileData, 0, 4);
            UnknownPointer = BinReader.GetPointer(RawFileData, 4, 4);

            GetDigimonModelMappings(RawFileData[DigimonModelMappingPointer..]);

            foreach (var item in DigimonModelMappings)
            {
                item.NameData = item.GetNameData(RawFileData);
            }

            //System.Diagnostics.Debug.WriteLine($"MODELDT0:");
            //int i = 0;
            //foreach (var item in DigimonModelMappings)
            //{
            //    var name = Utility.TextConversion.DigiStringToASCII(item.NameData);
            //    System.Diagnostics.Debug.WriteLine($"{i}: {name}");
            //    System.Diagnostics.Debug.WriteLine($"{item.ToString()}");
            //    i++;
            //}
        }

        /// <summary>
        /// Get the digimon mapping from the MODELDT0 file, which includes the name of the digimon and its model/animation files.
        /// We need to have a big dirty switch statement in here because there are a lot of entries skipped in the DIGIMNDT file
        /// If we do not add these missing ID's into this array as NULL we get a massive mis-alignment when looking up a digimon's name by its
        /// DIGIMNDT ID
        /// </summary>
        private void GetDigimonModelMappings(byte[] data)
        {
            DigimonModelMappings = new DigimonModelFilesMapping[(data.Length / DigimonModelFilesMappingDataLength)];
            for (int i = 0; i < data.Length; i += DigimonModelFilesMappingDataLength)
            {
                DigimonModelMappings[i / DigimonModelFilesMappingDataLength] = new DigimonModelFilesMapping(data[i..(i + DigimonModelFilesMappingDataLength)]);
            }
        }

        public DigimonModelFilesMapping GetDigimonByDigimonID(short digiID)
        {
            return DigimonModelMappings.FirstOrDefault(o => o.DigimonID / 2 == digiID);
        }
    }

    public class DigimonModelFilesMapping
    {
        public int NamePointer { get; set; }
        public short DigimonID { get; set; } //This is actually the DigimonID * 2
        public short MainModel { get; set; }
        public short IdleAnim { get; set; }
        public short GuardingDamageAnim { get; set; }
        public short DamageAnim { get; set; }
        public short CityModel { get; set; }
        public short Unknown2 { get; set; }
        public short Attack1Anim { get; set; }
        public short Attack2Anim { get; set; }
        public short Attack3Anim { get; set; }
        public short VictoryAnim { get; set; }
        public short GettingUpAnim { get; set; }
        public short DeadAnim { get; set; }
        public short Unknown3 { get; set; }
        public short Unknown4 { get; set; }
        public short Unknown5 { get; set; }
        public short Unknown6 { get; set; }
        public short Unknown7 { get; set; }

        public byte[] NameData;

        public DigimonModelFilesMapping(byte[] data)
        {
            NamePointer = BitConverter.ToInt32(data[0..4]);
            DigimonID = BitConverter.ToInt16(data[4..6]);
            MainModel = BitConverter.ToInt16(data[6..8]);
            IdleAnim = BitConverter.ToInt16(data[8..10]);
            GuardingDamageAnim = BitConverter.ToInt16(data[10..12]);
            DamageAnim = BitConverter.ToInt16(data[12..14]);
            CityModel = BitConverter.ToInt16(data[14..16]);
            Unknown2 = BitConverter.ToInt16(data[16..18]);
            Attack1Anim = BitConverter.ToInt16(data[18..20]);
            Attack2Anim = BitConverter.ToInt16(data[20..22]);
            Attack3Anim = BitConverter.ToInt16(data[22..24]);
            VictoryAnim = BitConverter.ToInt16(data[24..26]);
            GettingUpAnim = BitConverter.ToInt16(data[26..28]);
            DeadAnim = BitConverter.ToInt16(data[28..30]);
            Unknown3 = BitConverter.ToInt16(data[30..32]);
            Unknown4 = BitConverter.ToInt16(data[32..34]);
            Unknown5 = BitConverter.ToInt16(data[34..36]);
            Unknown6 = BitConverter.ToInt16(data[36..38]);
            Unknown7 = BitConverter.ToInt16(data[38..40]);
        }

        public byte[] GetNameData(byte[] data)
        {
            var nameBytes = BinReader.ReadBytesToDelimiter(data, NamePointer, 1, new byte[] { 0xff });
            byte[] nameBytesArray = new byte[nameBytes.Count];
            for (int i = 0; i < nameBytes.Count; i++)
                nameBytesArray[i] = nameBytes[i][0];

            return nameBytesArray;
        }

        public override string ToString()
        {;
            //return $"{NamePointer:X8}, {DigimonID:X2}, {MainModel:X2}, {IdleAnim:X2}, {GuardingDamageAnim:X2}, {DamageAnim:X2}, {CityModel:X2}, {Unknown2:X2}, {Attack1Anim:X2}, {Attack2Anim:X2}, {Attack3Anim:X2}, {VictoryAnim:X2}, {GettingUpAnim:X2}, {DeadAnim:X2}, {Unknown3:X2}, {Unknown4:X2}, {Unknown5:X2}, {Unknown6:X2}, {Unknown7:X2}";
            return $"{NamePointer:X8}, {(byte)(DigimonID & 0xff):X2}, {(byte)(DigimonID >> 8):X2}, " +
                $"{(byte)(MainModel & 0xff):X2}, {(byte)(MainModel >> 8):X2}, " +
                $"{(byte)(IdleAnim & 0xff):X2}, {(byte)(IdleAnim >> 8):X2}, " +
                $"{(byte)(GuardingDamageAnim & 0xff):X2}, {(byte)(GuardingDamageAnim >> 8):X2}, " +
                $"{(byte)(DamageAnim & 0xff):X2}, {(byte)(DamageAnim >> 8):X2}, " +
                $"{(byte)(CityModel & 0xff):X2}, {(byte)(CityModel >> 8):X2}, " +
                $"{(byte)(Unknown2 & 0xff):X2}, {(byte)(Unknown2 >> 8):X2}, " +
                $"{(byte)(Attack1Anim & 0xff):X2}, {(byte)(Attack1Anim >> 8):X2}, " +
                $"{(byte)(Attack2Anim & 0xff):X2}, {(byte)(Attack2Anim >> 8):X2}, " +
                $"{(byte)(Attack3Anim & 0xff):X2}, {(byte)(Attack3Anim >> 8):X2}, " +
                $"{(byte)(VictoryAnim & 0xff):X2}, {(byte)(VictoryAnim >> 8):X2}, " +
                $"{(byte)(GettingUpAnim & 0xff):X2}, {(byte)(GettingUpAnim >> 8):X2}, " +
                $"{(byte)(DeadAnim & 0xff):X2}, {(byte)(DeadAnim >> 8):X2}, " +
                $"{(byte)(Unknown3 & 0xff):X2}, {(byte)(Unknown3 >> 8):X2}, " +
                $"{(byte)(Unknown4 & 0xff):X2}, {(byte)(Unknown4 >> 8):X2}, " +
                $"{(byte)(Unknown5 & 0xff):X2}, {(byte)(Unknown5 >> 8):X2}, " +
                $"{(byte)(Unknown6 & 0xff):X2}, {(byte)(Unknown6 >> 8):X2}, " +
                $"{(byte)(Unknown7 & 0xff):X2}, {(byte)(Unknown7 >> 8):X2}";
        }
    }
}
