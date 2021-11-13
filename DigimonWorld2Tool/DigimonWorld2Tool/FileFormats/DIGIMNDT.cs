using DigimonWorld2Tool;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.FileFormat
{
    public class DIGIMNDT
    {
        private const int DigiDataEntryLength = 18;
        public byte[] RawFileData { get; private set; }

        public DigimonData[] DigimonDataArr { get; private set; }

        public DIGIMNDT()
        {
            RawFileData = BinReader.ReadAllBytesInFile(Settings.Settings.DIGIMNDTFilePath);
            DigimonDataArr = new DigimonData[RawFileData.Length / DigiDataEntryLength];
            for (int i = 0; i < RawFileData.Length; i += DigiDataEntryLength)
            {
                DigimonDataArr[i / DigiDataEntryLength] = new DigimonData(RawFileData[i..(i + DigiDataEntryLength)]);
            }
        }
    }

    public class DigimonData
    {
        public short ID { get; set; }
        public byte Skill { get; private set; }
        public byte Family { get; private set; }
        public byte LvType { get; private set; }
        public byte HpSpecScaling { get; private set; }
        public byte AtkMpScaling { get; private set; }
        public byte SpdDefScaling { get; private set; }
        public byte DP1 { get; private set; }
        public byte DP2 { get; private set; }
        public byte DP3 { get; private set; }
        public byte DP4 { get; private set; }
        public byte DP5 { get; private set; }
        public byte Evolution1 { get; private set; }
        public byte Evolution2 { get; private set; }
        public byte Evolution3 { get; private set; }
        public byte Evolution4 { get; private set; }
        public byte Evolution5 { get; private set; }

        public DigimonData(byte[] data)
        {
            ID = BitConverter.ToInt16(data[0..2]);
            Skill = data[2];
            Family = data[3];
            LvType = data[4];
            HpSpecScaling = data[5];
            AtkMpScaling = data[6];
            SpdDefScaling = data[7];
            DP1 = data[8];
            DP2 = data[9];
            DP3 = data[10];
            DP4 = data[11];
            DP5 = data[12];
            Evolution1 = data[13];
            Evolution2 = data[14];
            Evolution3 = data[15];
            Evolution4 = data[16];
            Evolution5 = data[17];
        }

        public override string ToString()
        {
            return $"{ID:X4} {Skill:X2} {Family:X2} {LvType:X2} {HpSpecScaling:X2} {AtkMpScaling:X2} {SpdDefScaling:X2} {DP1:X2} {DP2:X2} {DP3:X2} {DP4:X2} {DP5:X2} {Evolution1:X2} {Evolution2:X2} {Evolution3:X2} {Evolution4:X2} {Evolution5:X2} ";
        }
    }
}
