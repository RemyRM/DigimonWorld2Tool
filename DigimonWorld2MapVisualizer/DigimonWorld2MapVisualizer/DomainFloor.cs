using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private const int MapPlansPerFloor = 8;

        private readonly string[] FloorBasePointerAddress;
        private readonly int FloorBasePointerAddressDecimal;
        private readonly string FloorName;
        private readonly string[] UnknownData;

        private readonly List<DomainMapPlan> UniqueDomainMapPlans = new List<DomainMapPlan>();
        private readonly Dictionary<int, int> MapPlanOccuranceRates = new Dictionary<int, int>();

        public DomainFloor(string[] floorBasePointerAddress, int floorBasePointerAddressDecimal)
        {
            this.FloorBasePointerAddress = floorBasePointerAddress;
            this.FloorBasePointerAddressDecimal = floorBasePointerAddressDecimal;

            FloorName = ReadDomainName(FloorBasePointerAddressDecimal);
            UnknownData = ReadUnknownData(floorBasePointerAddressDecimal);

            PrintDomainFloorData();
            CreateMapPlansForFloor();
            AddMapLayoutOccuranceCount();
            DrawUniqueMapLayouts();

            Console.WriteLine();
        }

        /// <summary>
        /// Add the amount of times a given unique map plan can occur in the domain
        /// </summary>
        private void AddMapLayoutOccuranceCount()
        {
            foreach (KeyValuePair<int, int> item in MapPlanOccuranceRates)
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
            foreach (DomainMapPlan item in UniqueDomainMapPlans)
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
            Console.WriteLine(FloorName);
            Console.Write("Unknown data: ");
            foreach (string item in UnknownData)
            {
                Console.Write(item);
            }
            Console.WriteLine();
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
                string[] domainMapPlanPointerAddress = GetPointer(FloorBasePointerAddressDecimal + (int)floorPointerAddressOffset, out int domainMapPlanPointerAddressDecimal);
                if (MapPlanOccuranceRates.ContainsKey(domainMapPlanPointerAddressDecimal))
                {
                    MapPlanOccuranceRates[domainMapPlanPointerAddressDecimal]++;
                }
                else
                {
                    MapPlanOccuranceRates.Add(domainMapPlanPointerAddressDecimal, 1);
                    UniqueDomainMapPlans.Add(new DomainMapPlan(domainMapPlanPointerAddress, domainMapPlanPointerAddressDecimal));
                }
            }
        }

        /// <summary>
        /// Read the domain name from the domain data file
        /// </summary>
        /// <returns>The domain name and floor</returns>
        private string ReadDomainName(int floorHeaderPointerDecimalAddress)
        {
            string[] domainNamePointerAddress = GetPointer(floorHeaderPointerDecimalAddress + (int)DomainDataHeaderOffset.FileName, out int domainNamePointerDecimalAddress);
            string[] domainNameBytes = GetDomainNameBytes(domainNamePointerDecimalAddress);
            return TextConversion.DigiBytesToString(domainNameBytes);
        }

        /// <summary>
        /// Get the name of the current domain floor, this is stored in big endian with 0xFF as the terminating bit.
        /// The text is not stored in ASCII, and needs to be converted using the <see cref="TextConversion.DigiBytesToString(string[])" function/>
        /// </summary>
        /// <param name="pointerStartIndex">The decimal address where the pointer starts</param>
        /// <returns>string array of hex values representing the domain name</returns>
        private string[] GetDomainNameBytes(int pointerStartIndex)
        {
            int delimiterIndex = Array.IndexOf(Domain.DomainData, "FF", pointerStartIndex);
            string[] domainNameBigEndian = Domain.DomainData[pointerStartIndex..delimiterIndex];

            return domainNameBigEndian.ToArray();
        }

        /// <summary>
        /// At the address headerPointer + 4 there is 4 byte of unidentified Data
        /// </summary>
        /// <returns>4 bytes Unknown data</returns>
        private string[] ReadUnknownData(int floorHeaderPointerDecimalAddress)
        {
            string[] unknownData = GetPointer(floorHeaderPointerDecimalAddress + (int)DomainDataHeaderOffset.UnknownValue, out int unknownDataPointerDecimalAddress);
            return unknownData;
        }
    }
}
