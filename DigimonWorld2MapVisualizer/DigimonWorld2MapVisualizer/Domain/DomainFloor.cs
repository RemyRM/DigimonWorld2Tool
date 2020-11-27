using System;
using System.Collections.Generic;
using System.Linq;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
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

        private readonly int FloorBasePointerAddressDecimal;
        private readonly string FloorName;
        private readonly int UnknownDataDecimal;

        private readonly List<DomainMapPlan> UniqueDomainMapPlans = new List<DomainMapPlan>();
        private readonly Dictionary<int, int> MapPlanOccuranceRates = new Dictionary<int, int>();

        public DomainFloor(int floorBasePointerAddressDecimal)
        {
            this.FloorBasePointerAddressDecimal = floorBasePointerAddressDecimal;

            FloorName = ReadDomainName(FloorBasePointerAddressDecimal);
            UnknownDataDecimal = ReadUnknownData(floorBasePointerAddressDecimal);

            PrintDomainFloorData();
            CreateMapPlansForFloor();
            AddMapLayoutOccuranceCount();
            DrawUniqueMapLayouts();
        }

        /// <summary>
        /// Add the amount of times a given unique map plan can occur in the domain
        /// </summary>
        private void AddMapLayoutOccuranceCount()
        {
            foreach (var item in MapPlanOccuranceRates)
            {
                DomainMapPlan domainMapPlan = UniqueDomainMapPlans.FirstOrDefault(o => o.BaseMapPlanPointerAddressDecimal == item.Key);
                if (domainMapPlan != null)
                    domainMapPlan.OccuranceRate = item.Value;
            }
        }

        /// <summary>
        /// Print the data and draw a unique map plan 
        /// </summary>
        private void DrawUniqueMapLayouts()
        {
            foreach (var item in UniqueDomainMapPlans)
            {
                item.PrintDomainMapPlanData();
                item.DrawMap();
            }
        }

        /// <summary>
        /// Print the anme and the unkown data of this floor
        /// </summary>
        private void PrintDomainFloorData()
        {
            Console.Write($"\nFloor name: {FloorName}");
            Console.Write($"\nFloor base pointer addres: {FloorBasePointerAddressDecimal:X8}");
            Console.Write($"\nUnknown data: {UnknownDataDecimal:X8}");
            Console.Write($"\n");
        }

        /// <summary>
        /// Create a <see cref="DomainMapPlan"/> for each unique map layout in the domain.
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
                    UniqueDomainMapPlans.Add(new DomainMapPlan(domainMapPlanPointerAddressDecimal));
                }
            }
        }

        /// <summary>
        /// Read the domain name from the domain data file
        /// </summary>
        /// <returns>The domain name and floor</returns>
        private string ReadDomainName(int floorHeaderPointerDecimalAddress)
        {
            int domainNamePointerDecimalAddress = GetPointer(floorHeaderPointerDecimalAddress + (int)DomainDataHeaderOffset.FileName);
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
        /// </summary>
        /// <returns>4 bytes Unknown data</returns>
        private int ReadUnknownData(int floorHeaderPointerDecimalAddress)
        {
            int unknownDataPointerDecimalAddress = GetPointer(floorHeaderPointerDecimalAddress + (int)DomainDataHeaderOffset.UnknownValue);
            return unknownDataPointerDecimalAddress;
        }
    }
}
