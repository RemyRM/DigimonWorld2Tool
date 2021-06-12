using DigimonWorld2MapTool.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DigimonWorld2Tool.Rendering
{
    class EditorLayoutRenderer
    {
        private static readonly Vector2 GridSize = new Vector2(32, 48);
        public static int tileSize = 10;

        private static Bitmap floorLayoutLayer;
        private static Bitmap gridLayer;

        public static Vector2 GetGridSizeScaled()
        {
            return new Vector2(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
        }

        public static void SetupFloorLayerBitmap()
        {
            //combinedLayer = null;

            Vector2 scaledGridSize = GetGridSizeScaled();
            floorLayoutLayer = new Bitmap(scaledGridSize.x, scaledGridSize.y);

            floorLayoutLayer.MakeTransparent();
            DigimonWorld2ToolForm.EditorLayoutRenderTab.MapRenderLayer.Size = floorLayoutLayer.Size;
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
