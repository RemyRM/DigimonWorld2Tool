using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Domains;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DigimonWorld2Tool.Rendering
{
    class LayoutRenderer
    {
        private static readonly Vector2 GridSize = new Vector2(32, 48);
        public static int tileSize = 10;
        //public static PictureBox CurrentTargetRenderer { get; private set; }
        private static float TextPadding { get { return tileSize / 10; } }

        private static Bitmap floorLayoutLayer;
        private static Bitmap gridLayer;

        public static void SetupFloorLayerBitmap()
        {
            floorLayoutLayer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.MapRenderLayer.Size = floorLayoutLayer.Size;
        }

        public static void DrawTile(DomainTileCombo tileCombo)
        {
            for (int i = 0; i < tileSize; i++)
            {
                for (int j = 0; j < tileSize; j++)
                {
                    floorLayoutLayer.SetPixel((tileCombo.leftTile.Position.x * tileSize) + i, (tileCombo.leftTile.Position.y * tileSize) + j, tileCombo.leftTile.TileColour);
                    floorLayoutLayer.SetPixel((tileCombo.rightTile.Position.x * tileSize) + i, (tileCombo.rightTile.Position.y * tileSize) + j, tileCombo.rightTile.TileColour);
                }
            }
            CheckIfTileHasText(tileCombo);
            DigimonWorld2ToolForm.CurrentLayoutRenderTab.MapRenderLayer.Image = floorLayoutLayer;
        }

        private static void CheckIfTileHasText(DomainTileCombo tileCombo)
        {

            if (tileCombo.leftTile.FloorObjectText != "")
            {
                AddText(tileCombo.leftTile.Position * tileSize, tileCombo.leftTile.FloorObjectText);
            }
            if (tileCombo.rightTile.FloorObjectText != "")
            {
                AddText(tileCombo.rightTile.Position * tileSize, tileCombo.rightTile.FloorObjectText);
            }

        }

        private static void AddText(Vector2 pos, string text)
        {
            RectangleF rectf = new RectangleF(pos.x + (tileSize / 10), pos.y + (tileSize / 10), tileSize, tileSize + TextPadding);

            Graphics g = Graphics.FromImage(floorLayoutLayer);
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
    }
}
