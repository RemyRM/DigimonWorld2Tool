using System;
using System.Collections.Generic;
using System.Linq;
using DigimonWorld2MapTool.Interfaces;
using DigimonWorld2MapTool.MapObjects;
using DigimonWorld2MapTool.Utility;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2Tool;
using static DigimonWorld2MapTool.BinReader;
using System.Drawing;

namespace DigimonWorld2MapTool.Domains
{
    public class DomainMapLayout
    {
        private enum FloorLayoutHeaderOffsetOld : byte
        {
            FloorPlan = 0,
            Warps = 4,
            Chests = 8,
            Traps = 12,
            Digimon = 16,
        }
        private enum MapObjectDataLengthOld : byte
        {
            Warps = 3,
            Chests = 4,
            Traps = 8,
            Digimon = 4,
        }

        public int OccuranceRate { get; set; }
        public double OccuranceRatePercentage { get; set; }

        private const int MapLayoutDataLength = 1536; //All the layout data for a given map is 1536 bytes long (32x48)

        public readonly int BaseMapPlanPointerAddressDecimal;
        public readonly int BaseMapWarpsPointerAddressDecimal;
        public readonly int BaseMapChestsPointerAddressDecimal;
        public readonly int BaseMapTrapsPointerAddressDecimal;
        public readonly int BaseMapDigimonPointerAddressDecimal;

        public readonly List<DomainTileCombo> FloorLayoutTiles = new List<DomainTileCombo>();
        public readonly List<IFloorLayoutObject> FloorLayoutObjects = new List<IFloorLayoutObject>();

        public DomainMapLayout(int baseMapPlanPointerAddressDecimal)
        {
            this.BaseMapPlanPointerAddressDecimal = baseMapPlanPointerAddressDecimal;

            byte[] mapLayoutData = ReadMapPlanLayoutData();
            CreateDomainFloorTiles(ref mapLayoutData);

            BaseMapWarpsPointerAddressDecimal = GetPointerOld(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffsetOld.Warps);
            CreateDomainLayoutObjects(BaseMapWarpsPointerAddressDecimal, MapObjectDataLengthOld.Warps, IFloorLayoutObject.MapObjectType.Warp);

            BaseMapChestsPointerAddressDecimal = GetPointerOld(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffsetOld.Chests);
            CreateDomainLayoutObjects(BaseMapChestsPointerAddressDecimal, MapObjectDataLengthOld.Chests, IFloorLayoutObject.MapObjectType.Chest);

            BaseMapTrapsPointerAddressDecimal = GetPointerOld(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffsetOld.Traps);
            CreateDomainLayoutObjects(BaseMapTrapsPointerAddressDecimal, MapObjectDataLengthOld.Traps, IFloorLayoutObject.MapObjectType.Trap);

            BaseMapDigimonPointerAddressDecimal = GetPointerOld(baseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffsetOld.Digimon);
            CreateDomainLayoutObjects(BaseMapDigimonPointerAddressDecimal, MapObjectDataLengthOld.Digimon, IFloorLayoutObject.MapObjectType.Digimon);

            //AddFloorLayoutObjectsToTiles();
        }

        /// <summary>
        /// Print the base pointer address of this map layout and the rate of occurance
        /// </summary>
        public void PrintDomainMapPlanData()
        {
            System.Diagnostics.Debug.Write($"\nDomain map plan base pointer address: {BaseMapPlanPointerAddressDecimal.ToString("X8")}");
            System.Diagnostics.Debug.Write($"\nOccurance rate: {OccuranceRatePercentage}%");
        }

        /// <summary>
        /// Read the binary data that makes up this map's layout
        /// </summary>
        /// <returns>List of bytes that makes up the map layout data</returns>
        private byte[] ReadMapPlanLayoutData()
        {
            var floorPlanStartingAddressDecimal = GetPointerOld(BaseMapPlanPointerAddressDecimal + (int)FloorLayoutHeaderOffsetOld.FloorPlan);
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
        private void CreateDomainLayoutObjects(int baseStartOfObjectListPointerAddressDecimal, MapObjectDataLengthOld dataLength, IFloorLayoutObject.MapObjectType objectType)
        {
            List<byte[]> layoutObject = ReadBytesToDelimiterOld(baseStartOfObjectListPointerAddressDecimal, (int)dataLength);

            foreach (var data in layoutObject)
            {
                switch (objectType)
                {
                    case IFloorLayoutObject.MapObjectType.Warp:
                        FloorLayoutObjects.Add(new Warp(data));
                        break;
                    case IFloorLayoutObject.MapObjectType.Chest:
                        FloorLayoutObjects.Add(new Chest(data));
                        break;
                    case IFloorLayoutObject.MapObjectType.Trap:
                        FloorLayoutObjects.Add(new Trap(data));
                        break;
                    case IFloorLayoutObject.MapObjectType.Digimon:
                        FloorLayoutObjects.Add(new Digimon(data));
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
                if (item.Position.x >= 64 || item.Position.y >= 48)
                {
                    var floorId = Domain.Main.floorsInThisDomain.Count + 1;
                    var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;

                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"{item.ObjectType} is out of bounds {item.Position} on floor {floorId} layout {layoutID}");
                    if (DigimonWorld2ToolForm.ErrorMode == DigimonWorld2ToolForm.Strictness.Strict)
                    {
                        return;
                    }
                    else if (DigimonWorld2ToolForm.ErrorMode == DigimonWorld2ToolForm.Strictness.Sloppy)
                    {
                        continue;
                    }
                }

                // If the position of the object is even we can use the objects position to find the tile and place it on the left tile.
                // However since a grid tile is 2x1 we need to subtract 1 from the object's x axis if it's an odd number, which gives us
                // the domain tile it is on, of which we then take the righTile.
                //if (item.Position.x % 2 == 0)
                //{
                //    Tile tile = FloorLayoutTiles.FirstOrDefault(o => o.Position == item.Position).leftTile;
                //    if (tile != null)
                //        tile.AddObjectToTile(item);
                //}
                //else
                //{
                //    Tile tile = FloorLayoutTiles.FirstOrDefault(o => o.Position == item.Position - Vector2.Right).rightTile;
                //    if (tile != null)
                //        tile.AddObjectToTile(item);
                //}
            }
        }

        /// <summary>
        /// Draw the tiles that make up this map
        /// </summary>
        internal void DrawLayout()
        {
            LayoutRenderer.DrawTiles(FloorLayoutTiles);

            LayoutRenderer.DrawMapObjects(FloorLayoutObjects,
                                          IFloorLayoutObject.MapObjectType.Warp,
                                          bitmap =>
                                          {
                                              LayoutRenderer.warpsLayer = bitmap;

                                              if (DigimonWorld2ToolForm.Main.ShowWarpsCheckbox.Checked)
                                                  DigimonWorld2ToolForm.CurrentLayoutRenderTab.WarpsRenderLayer.Image = LayoutRenderer.warpsLayer = bitmap;
                                          });

            LayoutRenderer.DrawMapObjects(FloorLayoutObjects,
                                          IFloorLayoutObject.MapObjectType.Trap,
                                          bitmap =>
                                          {
                                              LayoutRenderer.trapsLayer = bitmap;
                                              if (DigimonWorld2ToolForm.Main.ShowTrapsCheckbox.Checked)
                                                  DigimonWorld2ToolForm.CurrentLayoutRenderTab.TrapsRenderLayer.Image = LayoutRenderer.trapsLayer = bitmap;
                                          });

            LayoutRenderer.DrawMapObjects(FloorLayoutObjects,
                                          IFloorLayoutObject.MapObjectType.Chest,
                                          bitmap =>
                                          {
                                              LayoutRenderer.chestsLayer = bitmap;
                                              if (DigimonWorld2ToolForm.Main.ShowChestsCheckbox.Checked)
                                                  DigimonWorld2ToolForm.CurrentLayoutRenderTab.ChestsRenderLayer.Image = LayoutRenderer.chestsLayer = bitmap;
                                          });

            LayoutRenderer.DrawMapObjects(FloorLayoutObjects,
                                          IFloorLayoutObject.MapObjectType.Digimon,
                                          bitmap =>
                                          {
                                              LayoutRenderer.digimonLayer = bitmap;
                                              if (DigimonWorld2ToolForm.Main.ShowDigimonCheckbox.Checked)
                                                  DigimonWorld2ToolForm.CurrentLayoutRenderTab.DigimonRenderLayer.Image = LayoutRenderer.digimonLayer = bitmap;
                                          });
        }
    }
}
