using System;
using System.Collections.Generic;
using System.Linq;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.Domains
{
    public class DomainFloor
    {
        private enum DomainDataHeaderOffset
        {
            FileName = 0,
            UnknownValue = 4,
            FloorLayout0 = 8,
            FloorLayout1 = 12,
            FloorLayout2 = 16,
            FloorLayout3 = 20,
            FloorLayout4 = 24,
            FloorLayout5 = 28,
            FloorLayout6 = 32,
            FloorLayout7 = 36,
            UnknownValue2 = 40,
            TrapLevel = 44,
            DigimonTable = 48
        }

        private const int MapPlansPerFloor = 8;

        public static DomainFloor CurrentDomainFloor;

        private readonly int FloorBasePointerAddressDecimal;
        public readonly string FloorName;
        private readonly int UnknownDataDecimal;
        private readonly int UnknownData2Decimal;
        private readonly byte[] TrapLevel;
        public readonly byte[] DigimonPacks = new byte[4];

        public readonly List<DomainMapLayout> UniqueDomainMapLayouts = new List<DomainMapLayout>();
        private readonly Dictionary<int, int> MapPlanOccuranceRates = new Dictionary<int, int>();

        public DomainFloor(int floorBasePointerAddressDecimal)
        {
            CurrentDomainFloor = this;

            this.FloorBasePointerAddressDecimal = floorBasePointerAddressDecimal;

            FloorName = ReadDomainName();
            UnknownDataDecimal = ReadUnknownData();
            UnknownData2Decimal = ReadUnknownData2();
            TrapLevel = ReadTrapLevel();
            DigimonPacks = ReadDigimonPacks();

            //PrintDomainFloorData();
            CreateMapPlansForFloor();
            AddMapLayoutOccuranceCount();
        }

        /// <summary>
        /// Add the amount of times a given unique map plan can occur in the domain
        /// </summary>
        private void AddMapLayoutOccuranceCount()
        {
            foreach (var item in MapPlanOccuranceRates)
            {
                DomainMapLayout domainMapPlan = UniqueDomainMapLayouts.FirstOrDefault(o => o.BaseMapPlanPointerAddressDecimal == item.Key);
                if (domainMapPlan != null)
                {
                    domainMapPlan.OccuranceRate = item.Value;
                    domainMapPlan.OccuranceRatePercentage = (domainMapPlan.OccuranceRate / 8d) * 100; // There are always 8 possible layouts per floor
                }
            }
        }

        /// <summary>
        /// Print the anme and the unkown data of this floor
        /// </summary>
        private void PrintDomainFloorData()
        {
            System.Diagnostics.Debug.Write($"\nFloor name: {FloorName}");
            System.Diagnostics.Debug.Write($"\nFloor base pointer addres: {FloorBasePointerAddressDecimal:X8}");
            System.Diagnostics.Debug.Write($"\nUnknown data: {UnknownDataDecimal:X8}");
            System.Diagnostics.Debug.Write($"\n");
        }

        /// <summary>
        /// Create a <see cref="DomainMapLayout"/> for each unique map layout in the domain.
        /// Keep track of the amount of occurances per unique domain layout
        /// </summary>
        private void CreateMapPlansForFloor()
        {
            for (int i = 0; i < MapPlansPerFloor; i++)
            {
                DomainDataHeaderOffset floorPointerAddressOffset = (DomainDataHeaderOffset)Enum.Parse(typeof(DomainDataHeaderOffset), $"FloorLayout{i}");
                int domainMapPlanPointerAddressDecimal = GetPointer(FloorBasePointerAddressDecimal + (int)floorPointerAddressOffset);
                if (MapPlanOccuranceRates.ContainsKey(domainMapPlanPointerAddressDecimal))
                {
                    MapPlanOccuranceRates[domainMapPlanPointerAddressDecimal]++;
                }
                else
                {
                    MapPlanOccuranceRates.Add(domainMapPlanPointerAddressDecimal, 1);
                    UniqueDomainMapLayouts.Add(new DomainMapLayout(domainMapPlanPointerAddressDecimal));
                }
            }
        }

        /// <summary>
        /// Read the domain name from the domain data file
        /// </summary>
        /// <returns>The domain name and floor</returns>
        private string ReadDomainName()
        {
            int domainNamePointerDecimalAddress = GetPointer(FloorBasePointerAddressDecimal + (int)DomainDataHeaderOffset.FileName);
            byte[] domainNameBytes = GetDomainNameBytes(domainNamePointerDecimalAddress);
            return TextConversion.DigiBytesToString(domainNameBytes);
        }

        /// <summary>
        /// Get the name of the current domain floor, this is stored in big endian with 0xFF as the terminating byte.
        /// The text is not stored in ASCII, and needs to be converted using the <see cref="TextConversion.DigiBytesToString(string[])" function/>
        /// </summary>
        /// <param name="pointerStartIndex">The decimal address where the pointer starts</param>
        /// <returns>string array of hex values representing the domain name</returns>
        private byte[] GetDomainNameBytes(int pointerStartIndex)
        {
            int delimiterIndex = Array.IndexOf(Domain.DomainData, (byte)0xFF, pointerStartIndex);
            return Domain.DomainData[pointerStartIndex..delimiterIndex];
        }

        /// <summary>
        /// At the address headerPointer + 4 there is 4 byte of unidentified Data
        /// We think this data refers to scripts/battle music?
        /// </summary>
        /// <returns>4 bytes Unknown data</returns>
        private int ReadUnknownData()
        {
            int unknownDataPointerDecimalAddress = GetPointer(FloorBasePointerAddressDecimal + (int)DomainDataHeaderOffset.UnknownValue);
            return unknownDataPointerDecimalAddress;
        }

        /// <summary>
        /// Read in the second set of yet unknown data that consists of 4 bytes
        /// </summary>
        /// <returns>4 bytes of unkown data</returns>
        private int ReadUnknownData2()
        {
            int unknownData2PointerDecimalAddress = GetPointer(FloorBasePointerAddressDecimal + (int)DomainDataHeaderOffset.UnknownValue2);
            return unknownData2PointerDecimalAddress;
        }

        /// <summary>
        /// Read the current trap level for this floor. it appears that the trap level gets changed by setting each byte individually
        /// </summary>
        /// <returns>byte array with the trap level(s)</returns>
        private byte[] ReadTrapLevel()
        {
            byte[] trapLevel = new byte[4];
            for (int i = 0; i < trapLevel.Length; i++)
            {
                trapLevel[i] = Domain.DomainData[FloorBasePointerAddressDecimal + (int)DomainDataHeaderOffset.TrapLevel + i];
            }
            return trapLevel;
        }

        /// <summary>
        /// Get the possible digimon packs for this floor.
        /// Each pack is represented by 1 byte in a 4 byte long array that correlates to an encounter ID
        /// </summary>
        /// <returns>4 bytes array containing an encounter each</returns>
        private byte[] ReadDigimonPacks()
        {
            byte[] digimonPack = new byte[4];
            for (int i = 0; i < digimonPack.Length; i++)
            {
                digimonPack[i] = Domain.DomainData[FloorBasePointerAddressDecimal + (int)DomainDataHeaderOffset.DigimonTable + i];
            }
            return digimonPack;
        }
    }
}
