using System;
using System.Collections.Generic;
using System.Linq;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.MapObjects;
using static DigimonWorld2MapVisualizer.BinReader;

namespace DigimonWorld2MapVisualizer.Domains
{
    public class DomainMapPlan
    {
        private enum FloorLayoutHeaderOffset : byte
        {
            FloorPlan = 0,
            Warps = 4,
            Chests = 8,
            Traps = 12,
            Digimon = 16,
        }
        private enum MapObjectDataLength : byte
        {
            Warps = 3,
            Chests = 4,
            Traps = 8,
            Digimon = 4,
        }

        public readonly int BaseMapPlanPointerAddressDecimal;

        public int OccuranceRate { get; set; }
        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        private readonly List<DomainTileCombo> FloorLayoutTiles = new List<DomainTileCombo>();
        private readonly List<IFloorLayoutObject> FloorLayoutObjects = new List<IFloorLayoutObject>();

        public DomainMapPlan(int baseMapPlanPointerAddressDecimal)
        {
            this.BaseMapPlanPointerAddressDecimal = baseMapPlanPointerAddressDecimal;

            byte[] mapLayoutData = ReadMapPlanLayoutData();
            CreateDomainFloorTiles(ref mapLayoutData);

            var BaseMapWarpsPointerAddressDecimal = GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Warps);
            CreateDomainLayoutObjects(BaseMapWarpsPointerAddressDecimal, MapObjectDataLength.Warps, IFloorLayoutObject.MapObjectType.Warp);

            var BaseMapChestsPointerAddressDecimal = GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Chests);
            CreateDomainLayoutObjects(BaseMapChestsPointerAddressDecimal, MapObjectDataLength.Chests, IFloorLayoutObject.MapObjectType.Chest);

            var BaseMapTrapsPointerAddressDecimal = GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Traps);
            CreateDomainLayoutObjects(BaseMapTrapsPointerAddressDecimal, MapObjectDataLength.Traps, IFloorLayoutObject.MapObjectType.Trap);

            var BaseMapDigimonPointerAddressDecimal = GetPointer(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.Digimon);
            CreateDomainLayoutObjects(BaseMapDigimonPointerAddressDecimal, MapObjectDataLength.Digimon, IFloorLayoutObject.MapObjectType.Digimon);

            AddFloorLayoutObjectsToTiles();
        }

        /// <summary>
        /// Print the base pointer address of this map layout and the rate of occurance
        /// </summary>
        public void PrintDomainMapPlanData()
        {
            var occuranceRatePercentage = (OccuranceRate / 8d) * 100; // There are always 8 possible layouts per floor

            System.Diagnostics.Debug.Write($"\nDomain map plan base pointer address: {BaseMapPlanPointerAddressDecimal.ToString("X8")}");
            System.Diagnostics.Debug.Write($"\nOccurance rate: {occuranceRatePercentage}%");
        }

        /// <summary>
        /// Read the binary data that makes up this map's layout
        /// </summary>
        /// <returns>List of bytes that makes up the map layout data</returns>
        private byte[] ReadMapPlanLayoutData()
        {
            var floorPlanStartingAddressDecimal = GetPointer(BaseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffset.FloorPlan);
            return Domain.DomainData[floorPlanStartingAddressDecimal..(MapLayoutDataLength + floorPlanStartingAddressDecimal)];
        }

        /// <summary>
        /// Create the floor tiles that make up this floor layout
        /// </summary>
        /// <param name="mapLayoutData">List of bytes representing the layout of this floor</param>
        private void CreateDomainFloorTiles(ref byte[] mapLayoutData)
        {
            for (var i = 0; i < MapLayoutDataLength; i++)
            {
                DomainTileCombo tile = new DomainTileCombo(new Vector2(i % 32, (int)Math.Floor(i / 32d)), mapLayoutData[i]);
                FloorLayoutTiles.Add(tile);
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
            List<byte[]> layoutObject = ReadBytesToDelimiter(baseStartOfObjectListPointerAddressDecimal, (int)dataLength);
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
                        System.Diagnostics.Debug.Write($"\n Error; could not parse MapObjectType {objectType}");
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
                    Tile tile = FloorLayoutTiles.First(o => o.Position == item.Position).leftTile;
                    tile.AddObjectToTile(item);
                }
                else
                {
                    Tile tile = FloorLayoutTiles.First(o => o.Position == item.Position - Vector2.Right).rightTile;
                    tile.AddObjectToTile(item);
                }
            }
        }

        /// <summary>
        /// Draw the tiles that make up this map
        /// </summary>
        internal void DrawMap()
        {
            System.Diagnostics.Debug.Write(Environment.NewLine);
            foreach (var item in FloorLayoutTiles)
            {
                item.Draw();
            }
        }
    }
}
