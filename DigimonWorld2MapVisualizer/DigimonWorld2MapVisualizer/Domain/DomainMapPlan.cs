using System;
using System.Collections.Generic;
using System.Linq;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.MapObjects;
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
            Traps = 8,
            Digimon = 4,
        }

        private readonly string[] BaseMapPlanPointerAddress;
        public readonly int BaseMapPlanPointerAddressDecimal;

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

            GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Warps, out int BaseMapWarpsPointerAddressDecimal);
            CreateDomainLayoutObjects(BaseMapWarpsPointerAddressDecimal, MapObjectDataLength.Warps, IFloorLayoutObject.MapObjectType.Warp);

            GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Chests, out int BaseMapChestsPointerAddressDecimal);
            CreateDomainLayoutObjects(BaseMapChestsPointerAddressDecimal, MapObjectDataLength.Chests, IFloorLayoutObject.MapObjectType.Chest);

            GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Traps, out int BaseMapTrapsPointerAddressDecimal);
            CreateDomainLayoutObjects(BaseMapTrapsPointerAddressDecimal, MapObjectDataLength.Traps, IFloorLayoutObject.MapObjectType.Trap);

            GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Digimon, out int BaseMapDigimonPointerAddressDecimal);
            CreateDomainLayoutObjects(BaseMapDigimonPointerAddressDecimal, MapObjectDataLength.Digimon, IFloorLayoutObject.MapObjectType.Digimon);

            AddFloorLayoutObjectsToTiles();
        }

        /// <summary>
        /// Print the base pointer address of this map layout and the rate of occurance
        /// </summary>
        public void PrintDomainMapPlanData()
        {
            Console.Write("\nDomain map plan base pointer address: ");
            foreach (var item in BaseMapPlanPointerAddress)
            {
                Console.Write(item);
            }
            var occuranceRatePercentage = (OccuranceRate / 8d) * 100;
            Console.Write($"\nOccurance rate: {occuranceRatePercentage}%");
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
        /// Create a domain layout object of the given type and add it to the <see cref="FloorLayoutObjects"/>
        /// </summary>
        /// <param name="baseStartOfObjectListPointerAddressDecimal">The decimal starting value of the objects list</param>
        /// <param name="dataLength">The length of a single object entry in bytes</param>
        /// <param name="objectType">The type of the object we are creating</param>
        private void CreateDomainLayoutObjects(int baseStartOfObjectListPointerAddressDecimal, MapObjectDataLength dataLength, IFloorLayoutObject.MapObjectType objectType)
        {
            List<string[]> layoutObject = ReadBytesToDelimiter(baseStartOfObjectListPointerAddressDecimal, (int)dataLength);
            foreach (var item in layoutObject)
            {
                switch (objectType)
                {
                    case IFloorLayoutObject.MapObjectType.Warp:
                        FloorLayoutObjects.Add(new Warp(IFloorLayoutObject.MapObjectType.Warp, item));
                        break;
                    case IFloorLayoutObject.MapObjectType.Chest:
                        FloorLayoutObjects.Add(new Chest(IFloorLayoutObject.MapObjectType.Chest, item));
                        break;
                    case IFloorLayoutObject.MapObjectType.Trap:
                        FloorLayoutObjects.Add(new Trap(IFloorLayoutObject.MapObjectType.Trap, item));
                        break;
                    case IFloorLayoutObject.MapObjectType.Digimon:
                        FloorLayoutObjects.Add(new Digimon(IFloorLayoutObject.MapObjectType.Digimon, item));
                        break;
                    default:
                        Console.Write($"\n Error; could not parse MapObjectType {objectType}");
                        break;
                }
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
            Console.Write(Environment.NewLine);
            foreach (var item in DomainFloorTiles)
            {
                item.Draw();
            }
            //Console.Write(Environment.NewLine);
        }
    }
}
