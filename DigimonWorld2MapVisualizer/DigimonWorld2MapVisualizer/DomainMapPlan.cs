using System;
using System.Collections.Generic;
using System.Linq;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer
{
    public class DomainMapPlan
    {
        private enum FloorLayoutHeaderOffset
        {
            FloorPlan = 0,
            Warps = 4,
            Chests = 8,
            Traps = 12,
            Digimon = 16,
        }

        private enum MapObjectDataLength
        {
            Warps = 3,
            Chests = 4,
            Digimon = 4,
            Traps = 6,
        }

        private readonly string[] BaseMapPlanPointerAddress;
        public readonly int BaseMapPlanPointerAddressDecimal;

        private readonly string[] BaseMapWarpsPointerAddress;
        private readonly int BaseMapWarpsPointerAddressDecimal;

        public int OccuranceRate { get; set; }
        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        private readonly List<DomainTile> DomainFloorTiles = new List<DomainTile>();
        private readonly List<IFloorLayoutObject> FloorLayoutObjects = new List<IFloorLayoutObject>();

        public DomainMapPlan(string[] baseMapPlanPointerAddress, int baseMapPlanPointerAddressDecimal)
        {
            this.BaseMapPlanPointerAddress = baseMapPlanPointerAddress;
            this.BaseMapPlanPointerAddressDecimal = baseMapPlanPointerAddressDecimal;

            string[] mapLayoutData = ReadMapPlanLayoutData();
            CreateDomainFloorTiles(ref mapLayoutData);

            BaseMapWarpsPointerAddress = GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Warps, out BaseMapWarpsPointerAddressDecimal);
            CreateDomainLayoutWarps();

            AddFloorLayoutObjectsToTiles();

            DrawMap();

            Console.WriteLine();
        }

        /// <summary>
        /// Print the base pointer address of this map layout and the rate of occurance
        /// </summary>
        public void PrintDomainMapPlanData()
        {
            Console.Write("Domain map plan base pointer address: ");
            foreach (var item in BaseMapPlanPointerAddress)
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
            for (var i = 0; i < MapLayoutDataLength; i++)
            {
                DomainTile tile = new DomainTile(new Vector2(i % 32, (int)Math.Floor(i / 32d)), mapLayoutData[i]);
                DomainFloorTiles.Add(tile);
            }
        }

        /// <summary>
        /// Read the list of warp data and create the warp objects
        /// </summary>
        private void CreateDomainLayoutWarps()
        {
            List<string[]> warps = ReadBytesToDelimiter(BaseMapWarpsPointerAddressDecimal, (int)MapObjectDataLength.Warps);
            foreach (var item in warps)
            {
                IFloorLayoutObject warp = new Warp(IFloorLayoutObject.MapObjectType.Warp, item);
                FloorLayoutObjects.Add(warp);
            }
        }

        private void AddFloorLayoutObjectsToTiles()
        {
            foreach (var item in FloorLayoutObjects)
            {
                // If the position of the object is even we can use the objects position to find the tile and place it on the left tile.
                // However since a grid tile is 2x1 we need to subtract 1 from the object's x axis if it's an odd number, which gives us
                // the domain tile it is on, of which we then take the righTile.
                if (item.Position.x % 2 == 0)
                {
                    Tile tile = DomainFloorTiles.First(o => o.Position == item.Position).leftTile;
                    tile.AddObjectToTile(item);
                }
                else
                {
                    Tile tile = DomainFloorTiles.First(o => o.Position == item.Position - Vector2.Right).rightTile;
                    tile.AddObjectToTile(item);
                }
            }
        }

        /// <summary>
        /// Draw the tiles that make up this map
        /// </summary>
        internal void DrawMap()
        {
            Console.WriteLine("Drawing map");
            foreach (var item in DomainFloorTiles)
            {
                item.Draw();
            }
        }
    }
}
