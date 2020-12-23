using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Domains;
using DigimonWorld2MapVisualizer.Interfaces;
using static DigimonWorld2MapVisualizer.Interfaces.IFloorLayoutObject;

namespace DigimonWorld2Tool.Rendering
{
    class LayoutRenderer
    {
        private static readonly Vector2 GridSize = new Vector2(32, 48);
        public static int tileSize = 10;

        private static float TextPadding { get { return tileSize / 10; } }

        private static Bitmap floorLayoutLayer;
        private static Bitmap gridLayer;

        public static Bitmap warpsLayer;
        public static Bitmap trapsLayer;
        public static Bitmap chestsLayer;
        public static Bitmap digimonLayer;

        public static Bitmap combinedLayer;

        public static Vector2 GetGridSizeScaled()
        {
            return new Vector2(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
        }

        public static void SetupFloorLayerBitmap()
        {
            combinedLayer = null;

            Vector2 scaledGridSize = GetGridSizeScaled();
            floorLayoutLayer = new Bitmap(scaledGridSize.x, scaledGridSize.y);

            floorLayoutLayer.MakeTransparent();
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.MapRenderLayer.Size = floorLayoutLayer.Size;
        }

        public static void DrawTiles(List<DomainTileCombo> tiles)
        {
            foreach (DomainTileCombo tileCombo in tiles)
            {
                for (int i = 0; i < tileSize; i++)
                {
                    for (int j = 0; j < tileSize; j++)
                    {
                        floorLayoutLayer.SetPixel((tileCombo.leftTile.Position.x * tileSize) + i, (tileCombo.leftTile.Position.y * tileSize) + j, tileCombo.leftTile.TileColour);
                        floorLayoutLayer.SetPixel((tileCombo.rightTile.Position.x * tileSize) + i, (tileCombo.rightTile.Position.y * tileSize) + j, tileCombo.rightTile.TileColour);
                    }
                }
            }
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.MapRenderLayer.Image = floorLayoutLayer;
            AddToCombinedLayer(floorLayoutLayer);
        }

        /// <summary>
        /// Draw the map object of type <see cref="MapObjectType"/> on its own layer, and add the text if applicable
        /// </summary>
        /// <param name="mapObjects">The list of objects to draw</param>
        /// <param name="mapType">The type of object to draw</param>
        /// <param name="callback">The layer to draw it to</param>
        public static void DrawMapObjects(IEnumerable<IFloorLayoutObject> mapObjects, MapObjectType mapType, Action<Bitmap> callback)
        {
            Bitmap layer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            layer.MakeTransparent();

            foreach (IFloorLayoutObject mapObject in mapObjects.Where(o => o.ObjectType == mapType))
            {
                if (mapObject.Position.x >= 64 || mapObject.Position.y >= 48)
                {
                    var floorId = Domain.Main.floorsInThisDomain.Count + 1;
                    var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;

                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"{mapObject.ObjectType} is out of bounds {mapObject.Position} on floor {floorId} layout {layoutID}");
                    if (DigimonWorld2ToolForm.ErrorMode == DigimonWorld2ToolForm.Strictness.Strict)
                    {
                        return;
                    }
                    else if (DigimonWorld2ToolForm.ErrorMode == DigimonWorld2ToolForm.Strictness.Sloppy)
                    {
                        continue;
                    }
                }

                for (int i = 0; i < tileSize; i++)
                {
                    for (int j = 0; j < tileSize; j++)
                    {
                        layer.SetPixel((mapObject.Position.x * tileSize) + i, (mapObject.Position.y * tileSize) + j, mapObject.ObjectColour);
                    }
                }

                if (mapObject.ObjectText != "")
                    AddText(mapObject.Position, mapObject.ObjectText, layer);
            }
            callback?.Invoke(layer);
            AddToCombinedLayer(layer);
        }

        private static void AddText(Vector2 pos, string text, Bitmap bmp)
        {
            pos *= tileSize;
            RectangleF rectf = new RectangleF(pos.x + (tileSize / 10), pos.y + (tileSize / 10), tileSize, tileSize + TextPadding);

            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, new Font("Tahoma", tileSize / 2), Brushes.Black, rectf);

            g.Flush();
        }

        public static void DrawGrid()
        {
            gridLayer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            for (int x = 0; x < GridSize.x * 2; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    for (int i = 0; i < tileSize; i++)
                    {
                        for (int j = 0; j < tileSize; j++)
                        {
                            if (i == 0 || j == 0)
                            {
                                if (DigimonWorld2ToolForm.Main.ShowGridCheckbox.Checked)
                                {
                                    gridLayer.SetPixel((x * tileSize) + i, (y * tileSize) + j, Color.White);
                                    gridLayer.SetPixel((x * tileSize) + i, (y * tileSize) + j, Color.White);
                                    continue;
                                }
                            }
                            gridLayer.SetPixel((x * tileSize) + i, (y * tileSize) + j, Color.Transparent);
                            gridLayer.SetPixel((x * tileSize) + i, (y * tileSize) + j, Color.Transparent);
                        }
                    }
                }
            }
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.GridRenderLayer.Image = gridLayer;
        }

        public static void HideGrid()
        {
            gridLayer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            gridLayer.MakeTransparent();
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.GridRenderLayer.Image = gridLayer;
        }


        private static void AddToCombinedLayer(Bitmap bmpSource)
        {
            if (combinedLayer == null)
            {
                Vector2 scaledGridSize = GetGridSizeScaled();
                combinedLayer = new Bitmap(scaledGridSize.x, scaledGridSize.y);
            }

            using (Graphics g = Graphics.FromImage(combinedLayer))
            {
                g.DrawImage(combinedLayer, Point.Empty);
                g.DrawImage(bmpSource, Point.Empty);
            }
        }
    }
}
