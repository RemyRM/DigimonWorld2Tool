using DigimonWorld2Tool.Domains;
using DigimonWorld2Tool.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DigimonWorld2Tool.Rendering
{
    class EditorLayoutRenderer
    {
        private static readonly Vector2 GridSize = new Vector2(32, 48);
        public static int tileSize = 10;

        private static Bitmap floorLayoutLayer;
        private static Bitmap gridLayer;

        public static DomainTileCombo[] tiles;

        public static Vector2 GetGridSizeScaled()
        {
            return new Vector2(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
        }

        public static void SetupFloorLayerBitmap()
        {
            //combinedLayer = null;
            if (tiles == null)
                InitializeEmptyFloor();

            Vector2 scaledGridSize = GetGridSizeScaled();
            floorLayoutLayer = new Bitmap(scaledGridSize.x, scaledGridSize.y);

            floorLayoutLayer.MakeTransparent();
            DigimonWorld2ToolForm.EditorLayoutRenderTab.MapRenderLayer.Size = floorLayoutLayer.Size;

            DrawTiles(tiles.ToList());
        }

        private static void InitializeEmptyFloor()
        {
            tiles = new DomainTileCombo[1536];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new DomainTileCombo(new Vector2(i % GridSize.x, (int)Math.Floor((double)i / GridSize.x)), 0x88);
            }
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
            DigimonWorld2ToolForm.EditorLayoutRenderTab.MapRenderLayer.Image = floorLayoutLayer;
            //AddToCombinedLayer(floorLayoutLayer);
        }

        public static void UpdateTile(Vector2 position, Tile.DomainTileTypeOld tileType)
        {
            Color tileColour = Tile.TileTypeColourOld[tileType];
            //TODO: This needs to update the actual tile too, not just the displayed pixel
            DomainTileCombo selectedCombo = tiles.FirstOrDefault(o => o.leftTile.Position == position);
            Tile selectedTile = null;
            if (selectedCombo == null)
            {
                selectedCombo = tiles.FirstOrDefault(o => o.rightTile.Position == position);
                selectedTile = selectedCombo.rightTile;
            }
            else
            {
                selectedTile = selectedCombo.leftTile;
            }

            byte currentByteValue = selectedCombo.TileValueDec;
            if (selectedTile.Position.x % 2 == 0)
            {
                //we've got the left tile, meaning we need to set the right nibblet
                currentByteValue = (byte)((currentByteValue & 0xF0) | (byte)selectedTile.GetTileTypeBasedOnColour(tileColour));
            }
            else
            {
                currentByteValue = (byte)((currentByteValue & 0x0F) | ((byte)selectedTile.GetTileTypeBasedOnColour(tileColour) << 0x04));
                //With the right tile we need to edit the right nibblet
            }
            selectedCombo.TileValueDec = currentByteValue;
            selectedCombo.TileValueHex = currentByteValue.ToString("X2");

            //byte b = 0x11;
            //var nibbleValue = 0x02;
            //b = (byte)((b & 0xF0) | nibbleValue);
            //b = (byte)((b & 0x0F) | (nibbleValue << 0x04));

            for (int i = 0; i < tileSize; i++)
            {
                for (int j = 0; j < tileSize; j++)
                {
                    //floorLayoutLayer.SetPixel((position.x * tileSize) + i, (position.y * tileSize) + j, tileColour);
                    //floorLayoutLayer.SetPixel((position.x * tileSize) + i, (position.y * tileSize) + j, tileColour);
                    floorLayoutLayer.SetPixel((position.x * tileSize) + i, (position.y * tileSize) + j, tileColour);
                }
            }


            DigimonWorld2ToolForm.EditorLayoutRenderTab.MapRenderLayer.Image = floorLayoutLayer;
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
                                if (DigimonWorld2ToolForm.Main.EditorShowGridCheckbox.Checked)
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
            DigimonWorld2ToolForm.EditorLayoutRenderTab.GridRenderLayer.Image = gridLayer;
        }

        public static void HideGrid()
        {
            gridLayer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            gridLayer.MakeTransparent();
            DigimonWorld2ToolForm.EditorLayoutRenderTab.GridRenderLayer.Image = gridLayer;
        }
    }
}
