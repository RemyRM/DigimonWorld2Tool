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
                if (item == null)
                    continue;
                item.NameData = item.GetNameData(RawFileData);
            }
        }

        /// <summary>
        /// Get the digimon mapping from the MODELDT0 file, which includes the name of the digimon and its model/animation files.
        /// We need to have a big dirty switch statement in here because there are a lot of entries skipped in the DIGIMNDT file
        /// If we do not add these missing ID's into this array as NULL we get a massive mis-alignment when looking up a digimon's name by its
        /// DIGIMNDT ID
        /// </summary>
        private void GetDigimonModelMappings(byte[] data)
        {
            //we add 3 because Agumon starts at 0x03 in DIGIMNDT
            //DigimonModelMappings = new DigimonModelFilesMapping[(data.Length / DigimonModelFilesMappingDataLength) + 3];
            //for (int i = 0; i < data.Length; i += DigimonModelFilesMappingDataLength)
            //{
            //    DigimonModelMappings[i / DigimonModelFilesMappingDataLength + 3] = new DigimonModelFilesMapping(data[i..(i + DigimonModelFilesMappingDataLength)]);
            //}

            DigimonModelMappings = new DigimonModelFilesMapping[255];
            int digimonMappingEntryBaseAddress = 0;
            for (int j = 0; j < DigimonModelMappings.Length; j++)
            {
                switch (j)
                {
                    case 0x00:
                    case 0x01:
                    case 0x02:
                    case 0x0F:
                    case 0x10:
                    case 0x1D:
                    case 0x1E:
                    case 0x2B:
                    case 0x2C:
                    case 0x3E:
                    //case 0x52: //There is a model for this, but no DIGIMNDT
                    //case 0x53: //There is a model for this, but no DIGIMNDT
                    //case 0x56: //There is a model for this, but no DIGIMNDT
                    //case 0x57: //There is a model for this, but no DIGIMNDT
                    //case 0x59: //There is a model for this, but no DIGIMNDT
                    //case 0x5D: //There is a model for this, but no DIGIMNDT
                    //case 0x61: //There is a model for this, but no DIGIMNDT
                    //case 0x63: //There is a model for this, but no DIGIMNDT
                    case 0x6C:
                    case 0x72:
                    case 0x74:
                    case 0x76:
                    case 0x78:
                    case 0x7D:
                    case 0x7E:
                    case 0x7F:
                    case 0x81:
                    case 0x82:
                    case 0x98:
                    case 0x99:
                    case 0x9A:
                    case 0x9B:
                    case 0x9C:
                    case 0x9D:
                    case 0x9E:
                    case 0x9F:
                    case 0xA0:
                    case 0xA1:
                    case 0xA2:
                    case 0xA3:
                    case 0xA4:
                    case 0xA5:
                    case 0xA6:
                    case 0xA7:
                    case 0xA8:
                    case 0xA9:
                    case 0xAA:
                    case 0xAB:
                    case 0xAC:
                    case 0xAD:
                    case 0xAE:
                    case 0xAF:
                    case 0xB0:
                    case 0xB1:
                    case 0xB2:
                    case 0xB3:
                    case 0XEF:
                    case 0XF1:
                    case 0XF2:
                    case 0XF3:
                        DigimonModelMappings[j] = null;
                        continue;
                }

                DigimonModelMappings[j] = new DigimonModelFilesMapping(data[digimonMappingEntryBaseAddress..(digimonMappingEntryBaseAddress + DigimonModelFilesMappingDataLength)]);
                digimonMappingEntryBaseAddress += DigimonModelFilesMappingDataLength;
            }
        }
    }

    public class DigimonModelFilesMapping
    {
        public int NamePointer { get; set; }
        public short Unknown1 { get; set; }
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
            Unknown1 = BitConverter.ToInt16(data[4..6]);
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
    }
}
