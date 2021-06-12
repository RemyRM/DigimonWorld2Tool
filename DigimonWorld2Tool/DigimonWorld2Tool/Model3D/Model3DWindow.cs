using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DigimonWorld2Tool.Model3D
{
    public class Model3DWindow 
    {
        public Model3DWindow()
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(500, 500),
                Title = "Digimon World 2 Model viewer",
            };

            using(TKWindow window = new TKWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }

    public class TKWindow : GameWindow
    {
        public TKWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            // Check if the Escape button is currently being pressed.
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                // If it is, close the window.
                Close();
            }

            base.OnUpdateFrame(e);
        }
    }
}
