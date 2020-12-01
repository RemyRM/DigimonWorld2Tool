using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2MapVisualizer.Utility;
using System.Linq;
using DigimonWorld2MapVisualizer.Domains;

namespace DigimonWorld2Tool.UserControls
{
    public partial class RenderLayoutTab : UserControl
    {
        public RenderLayoutTab()
        {
            InitializeComponent();
            MapRenderLayer.Controls.Add(GridRenderLayer);
        }

        private void GridRenderLayer_MouseClick(object sender, MouseEventArgs e)
        {
            Vector2 mouseGridPos = new Vector2((int)Math.Floor((double)e.Location.X / LayoutRenderer.tileSize),
                                               (int)Math.Floor((double)e.Location.Y / LayoutRenderer.tileSize));

            GetObjectAtGridPosition(mouseGridPos);
        }

        private void GetObjectAtGridPosition(Vector2 gridPos)
        {
            Tile tile = null;
            if (gridPos.x % 2 == 0)
            {
                tile = DigimonWorld2ToolForm.CurrentMapLayout.FloorLayoutTiles.FirstOrDefault(o => o.Position == gridPos).leftTile;
            }
            else
            {
                tile = DigimonWorld2ToolForm.CurrentMapLayout.FloorLayoutTiles.FirstOrDefault(o => o.Position == gridPos - Vector2.Right).rightTile;
            }

            if (tile.FloorObject == null)
                return;

            DigimonWorld2ToolForm.Main.SetCurrentObjectInformation(tile);
        }
    }
}
