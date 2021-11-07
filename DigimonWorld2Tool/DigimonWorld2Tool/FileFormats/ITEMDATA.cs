using System;

namespace DigimonWorld2Tool.FileFormats
{
    public class ITEMDATA
    {
        private const int ItemDataEntryLength = 16;
        private int ItemEntriesPointer { get; set; }
        private byte[] RawFileData { get; set; }
        public ITEMDATAEntry[] ItemData;

        public ITEMDATA()
        {
            RawFileData = BinReader.ReadAllBytesInFile(Settings.Settings.ITEMDATAFilePath);
            ItemEntriesPointer = BinReader.GetPointer(RawFileData, 0, 4);

            GetItems(RawFileData[ItemEntriesPointer..]);

            foreach (var item in ItemData)
            {
                item.NameData = item.GetNameData(RawFileData);
                item.DescriptionData = item.GetDescriptionData(RawFileData);
            }

            System.Diagnostics.Debug.WriteLine("ItemDATA:");
            foreach (var item in ItemData)
            {
                var name = Utility.TextConversion.DigiStringToASCII(item.NameData);
                var description = Utility.TextConversion.DigiStringToASCII(item.DescriptionData);
                System.Diagnostics.Debug.WriteLine($"{name}, {description}");
                System.Diagnostics.Debug.WriteLine($"{item}");
            }
        }

        private void GetItems(byte[] data)
        {
            ItemData = new ITEMDATAEntry[data.Length / ItemDataEntryLength];
            for (int i = 0; i < data.Length; i += ItemDataEntryLength)
            {
                ItemData[i / ItemDataEntryLength] = new ITEMDATAEntry(data[i..(i + ItemDataEntryLength)]);
            }
        }
    }

    public class ITEMDATAEntry
    {
        public short ID { get; private set; }
        public byte ItemType { get; private set; }
        public byte ItemLevel { get; private set; }
        public int Price { get; private set; }
        public byte Unknown2 { get; private set; }
        public int NamePointer { get; private set; }
        public int DescriptionPointer { get; private set; }

        public byte[] NameData { get; set; }
        public byte[] DescriptionData { get; set; }

        public ITEMDATAEntry(byte[] data)
        {
            ID = BitConverter.ToInt16(data[0..2]);
            ItemType = data[2];
            ItemLevel = data[3];
            Price = data[6] << 16 | data[5] << 8 | data[4]; // devs be like "Let's use 3 bytes for the price!"
            Unknown2 = data[7];
            NamePointer = BitConverter.ToInt32(data[8..12]);
            DescriptionPointer = BitConverter.ToInt32(data[12..16]);
        }

        public byte[] GetNameData(byte[] data)
        {
            var nameBytes = BinReader.ReadBytesToDelimiter(data, NamePointer, 1, new byte[] { 0xff });
            byte[] nameBytesArray = new byte[nameBytes.Count];
            for (int i = 0; i < nameBytes.Count; i++)
                nameBytesArray[i] = nameBytes[i][0];

            return nameBytesArray;
        }

        public byte[] GetDescriptionData(byte[] data)
        {
            var descriptionBytes = BinReader.ReadBytesToDelimiter(data, DescriptionPointer, 1, new byte[] { 0xff });
            byte[] descriptionBytesArray = new byte[descriptionBytes.Count];
            for (int i = 0; i < descriptionBytes.Count; i++)
                descriptionBytesArray[i] = descriptionBytes[i][0];

            return descriptionBytesArray;
        }

        public override string ToString()
        {
            //return $"{ID:X2} {Unknown:X2} {Price:X4} {Unknown3:X2} {NamePointer:X4} {Unknown4:X4}";
            return $"{ID:X4} {ItemType:X2} {ItemLevel:X2} {Price:X6} {Unknown2:X2} {NamePointer:X8} {DescriptionPointer:X8}";
        }
    }
}
