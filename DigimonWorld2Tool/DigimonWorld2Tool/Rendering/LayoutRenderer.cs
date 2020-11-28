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
        public static PictureBox CurrentTargetRenderer { get; private set; }
        private static float TextPadding { get { return tileSize / 10; } }

        private static Bitmap floorLayoutLayer;
        private static Bitmap gridLayer;

        public static void SetRenderTarget(PictureBox target)
        {
            CurrentTargetRenderer = target;
            floorLayoutLayer = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            CurrentTargetRenderer.Size = floorLayoutLayer.Size;
        }

        public static void DrawTile(DomainTileCombo tileCombo)
        {
            for (int i = 0; i < tileSize; i++)
            {
                for (int j = 0; j < tileSize; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        if (DigimonWorld2ToolForm.Main.ShowGridCheckbox.Checked)
                        {
                            floorLayoutLayer.SetPixel((tileCombo.leftTile.Position.x * tileSize) + i, (tileCombo.leftTile.Position.y * tileSize) + j, Color.White);
                            floorLayoutLayer.SetPixel((tileCombo.rightTile.Position.x * tileSize) + i, (tileCombo.rightTile.Position.y * tileSize) + j, Color.White);
                            continue;
                        }
                    }
                    floorLayoutLayer.SetPixel((tileCombo.leftTile.Position.x * tileSize) + i, (tileCombo.leftTile.Position.y * tileSize) + j, tileCombo.leftTile.TileColour);
                    floorLayoutLayer.SetPixel((tileCombo.rightTile.Position.x * tileSize) + i, (tileCombo.rightTile.Position.y * tileSize) + j, tileCombo.rightTile.TileColour);
                }
            }
            if (tileCombo.leftTile.FloorObjectText != "")
            {
                AddText(tileCombo.leftTile.Position * tileSize, tileCombo.leftTile.FloorObjectText);
            }
            if (tileCombo.rightTile.FloorObjectText != "")
            {
                AddText(tileCombo.rightTile.Position * tileSize, tileCombo.rightTile.FloorObjectText);
            }

            CurrentTargetRenderer.Image = floorLayoutLayer;
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
    }
}
