using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Domains;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Rendering
{
    class LayoutRenderer
    {
        private static readonly Vector2 GridSize = new Vector2(32, 48);
        private static readonly int tileSize = 10;
        public static PictureBox currentTargetRenderer { get; private set; }
        private static Bitmap bmp;

        public static void SetRenderTarget(PictureBox target)
        {
            currentTargetRenderer = target;
            bmp = new Bitmap(GridSize.x * 2 * tileSize, GridSize.y * tileSize);
            currentTargetRenderer.Size = bmp.Size;
        }

        public static void DrawTile(DomainTileCombo tileCombo)
        {
            for (int i = 0; i < tileSize; i++)
            {
                for (int j = 0; j < tileSize; j++)
                {
                    bmp.SetPixel((tileCombo.leftTile.Position.x * tileSize) + i, (tileCombo.leftTile.Position.y * tileSize) + j, tileCombo.leftTile.TileColour);
                    bmp.SetPixel((tileCombo.rightTile.Position.x * tileSize) + i, (tileCombo.rightTile.Position.y * tileSize) + j, tileCombo.rightTile.TileColour);
                }
            }
            currentTargetRenderer.Image = bmp;
        }
    }
}
