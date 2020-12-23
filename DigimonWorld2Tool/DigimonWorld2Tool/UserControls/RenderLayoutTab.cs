using System;
using System.Linq;
using System.Windows.Forms;
using DigimonWorld2Tool.Rendering;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Interfaces;

namespace DigimonWorld2Tool.UserControls
{
    public partial class RenderLayoutTab : UserControl
    {
        public RenderLayoutTab()
        {
            InitializeComponent();
            MapRenderLayer.Controls.Add(GridRenderLayer);
            GridRenderLayer.Controls.Add(TrapsRenderLayer);
            TrapsRenderLayer.Controls.Add(WarpsRenderLayer);
            WarpsRenderLayer.Controls.Add(ChestsRenderLayer);
            ChestsRenderLayer.Controls.Add(DigimonRenderLayer);


            // This cursor layer always needs to be added as the last layer, so it is on top for receiving mouse events
            DigimonRenderLayer.Controls.Add(CursorLayer);
        }

        private void GetObjectAtGridPosition(Vector2 gridPos)
        {
            IFloorLayoutObject mapObject = DigimonWorld2ToolForm.CurrentMapLayout.FloorLayoutObjects.FirstOrDefault(o => o.Position == gridPos);

            if (mapObject != null)
                DigimonWorld2ToolForm.Main.SetCurrentObjectInformation(mapObject);
        }

        private void CursorLayer_MouseClick(object sender, MouseEventArgs e)
        {
            Vector2 mouseGridPos = new Vector2((int)Math.Floor((double)e.Location.X / LayoutRenderer.tileSize),
                                               (int)Math.Floor((double)e.Location.Y / LayoutRenderer.tileSize));

            GetObjectAtGridPosition(mouseGridPos);
        }
    }
}
