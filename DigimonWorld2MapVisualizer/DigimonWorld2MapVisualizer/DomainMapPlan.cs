using System;
using System.Collections.Generic;
using System.Text;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
{
    class DomainMapPlan
    {
        private enum FloorLayoutHeaderOffset
        {
            FloorPlan = 0,
            Warps = 4,
            Chests = 8,
            Traps = 12,
            Digimon = 16,
        }

        private readonly string[] BaseMapPlanPointerAddress;
        public readonly int BaseMapPlanPointerAddressDecimal;
        public int OccuranceRate { get; set; }
        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        private readonly List<DomainTile> domainFloorTiles = new List<DomainTile>();

        public DomainMapPlan(string[] baseMapPlanPointerAddress, int baseMapPlanPointerAddressDecimal)
        {
            this.BaseMapPlanPointerAddress = baseMapPlanPointerAddress;
            this.BaseMapPlanPointerAddressDecimal = baseMapPlanPointerAddressDecimal;

            string[] mapLayoutData = ReadMapPlanLayoutData();
            CreateDomainFloorTiles(ref mapLayoutData);

            Console.WriteLine();
        }

        /// <summary>
        /// Print the base pointer address of this map layout and the rate of occurance
        /// </summary>
        public void PrintDomainMapPlanData()
        {
            Console.Write("Domain map plan base pointer address: ");
            foreach (string item in BaseMapPlanPointerAddress)
            {
                Console.Write(item);
            }
            var occuranceRatePercentage = (OccuranceRate / 8d) * 100;
            Console.WriteLine($"\nOccurance rate: {occuranceRatePercentage}%");
            Console.WriteLine();
        }

        /// <summary>
        /// Read the binary data that makes up this map's layout
        /// </summary>
        /// <returns>The map layout in hex</returns>
        private string[] ReadMapPlanLayoutData()
        {
            string[] floorPlanStartingAddress = GetPointer(BaseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.FloorPlan, out int floorPlanStartingAddressDecimal);
            string[] mapLayoutData = Domain.DomainData[floorPlanStartingAddressDecimal..(MapLayoutDataLength + floorPlanStartingAddressDecimal)];
            return mapLayoutData;
        }

        /// <summary>
        /// Create the floor tiles that make up this floor layout
        /// </summary>
        /// <param name="mapLayoutData">Hex representation of this floor</param>
        private void CreateDomainFloorTiles(ref string[] mapLayoutData)
        {
            for (int i = 0; i < MapLayoutDataLength; i++)
            {
                DomainTile tile = new DomainTile(new Vector2(i % 32, (int)Math.Floor(i / 32d)), mapLayoutData[i]);
                domainFloorTiles.Add(tile);
            }
        }

        /// <summary>
        /// Draw the tiles that make up this map
        /// </summary>
        internal void DrawMap()
        {
            foreach (DomainTile item in domainFloorTiles)
            {
                item.Draw();
            }
        }
    }
}
