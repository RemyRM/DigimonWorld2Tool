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
        private readonly int BaseMapPlanPointerAddressDecimal;
        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        private readonly List<DomainTile> DomainFloorTiles = new List<DomainTile>();

        public DomainMapPlan(string[] baseMapPlanPointerAddress, int baseMapPlanPointerAddressDecimal)
        {
            this.BaseMapPlanPointerAddress = baseMapPlanPointerAddress;
            this.BaseMapPlanPointerAddressDecimal = baseMapPlanPointerAddressDecimal;

            PrintDomainMapPlanData();

            string[] mapLayoutData = ReadMapPlanLayoutData();
            CreateDomainFloorTiles(ref mapLayoutData);

            Console.WriteLine();
        }

        private void PrintDomainMapPlanData()
        {
            Console.Write("Domain map plan base pointer address: ");
            foreach (var item in BaseMapPlanPointerAddress)
            {
                Console.Write(item);
            }
            Console.WriteLine();
        }

        private string[] ReadMapPlanLayoutData()
        {
            string[] floorPlanStartingAddress = GetPointer(BaseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.FloorPlan, out int floorPlanStartingAddressDecimal);
            string[] mapLayoutData = Domain.DomainData[floorPlanStartingAddressDecimal..(MapLayoutDataLength + floorPlanStartingAddressDecimal)];
            return mapLayoutData;
        }

        private void CreateDomainFloorTiles(ref string[] mapLayoutData)
        {
            for (int i = 0; i < MapLayoutDataLength; i++)
            {
                DomainTile tile = new DomainTile(new Vector2(i % 32, (int)Math.Floor(i / 32d)), mapLayoutData[i]);
                DomainFloorTiles.Add(tile);
            }
        }
    }
}
