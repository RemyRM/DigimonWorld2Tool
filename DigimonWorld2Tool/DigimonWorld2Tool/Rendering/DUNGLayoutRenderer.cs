using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

using DigimonWorld2Tool.Views;
using DigimonWorld2Tool.Utility;
using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.FileInterpreter;

namespace DigimonWorld2Tool.Rendering
{
    class DUNGLayoutRenderer
    {
        public static DUNGLayoutRenderer Instance { get; private set; }

        public const int MAP_WIDTH = 64;
        public const int MAP_HEIGHT = 48;

        private Bitmap CurrentDrawnMapLayoutBitmap { get; set; }

        public int TileSizeWidth { get; private set; }
        public int TileSizeHeight { get; private set; }

        private PictureBox FloorLayoutPictureBox { get; }

        private DungFloorLayoutHeader floorLayoutToDraw;
        public DungFloorLayoutHeader FloorLayoutToDraw
        {
            get => floorLayoutToDraw;
            private set => floorLayoutToDraw = value;
        }

        public enum TileType : byte
        {
            Room,
            Corridor,
            Water,
            Fire,
            Nature,
            Machine,
            Dark,
            Override,
            Empty,
            None = 0x99
        }

        public static readonly Dictionary<TileType, Color> TileTypeColour = new Dictionary<TileType, Color>
        {
            {TileType.Empty, Color.Black},
            {TileType.Room, Color.Gray},
            {TileType.Corridor, Color.DarkGray},
            {TileType.Water, Color.DarkBlue},
            {TileType.Fire, Color.DarkRed},
            {TileType.Nature, Color.DarkGreen},
            {TileType.Machine, Color.FromArgb(255, 184, 165, 24)},
            {TileType.Dark, Color.DarkMagenta},
            {TileType.Override, Color.Magenta }
        };

        public DUNGLayoutRenderer(PictureBox floorLayoutPictureBox)
        {
            Instance = this;
            this.FloorLayoutPictureBox = floorLayoutPictureBox;
        }

        public void SetupFloorLayoutToDraw(DungFloorLayoutHeader floorLayoutToDraw)
        {
            this.FloorLayoutToDraw = floorLayoutToDraw;
        }

        public void SetupDungFloorBitmap()
        {
            CurrentDrawnMapLayoutBitmap = new Bitmap(FloorLayoutPictureBox.Width, FloorLayoutPictureBox.Height);
            TileSizeWidth = CurrentDrawnMapLayoutBitmap.Width / MAP_WIDTH;
            TileSizeHeight = CurrentDrawnMapLayoutBitmap.Height / MAP_HEIGHT;
        }

        public void DrawDungFloorLayout()
        {
            DrawFloorLayoutToBitmap();
            DrawFloorTrapsToBitmap();
            DrawFloorWarpsToBitmap();
            DrawFloorChestsToBitmap();
            DrawFloorDigimonToBitmap();
            DrawGridLayout();

            ApplyFloorBitmapToPictureBox();
        }

        private void DrawFloorLayoutToBitmap()
        {
            for (int i = 0; i < FloorLayoutToDraw.FloorLayoutData.Length; i++)
            {
                var xId = i % (MAP_WIDTH / 2);
                var yId = (int)Math.Floor(i / (MAP_WIDTH / 2d));

                var rightTileType = (TileType)FloorLayoutToDraw.FloorLayoutData[i].GetLefNiblet();
                var leftTileType = (TileType)FloorLayoutToDraw.FloorLayoutData[i].GetRightNiblet();

                if (rightTileType == TileType.Override)
                    rightTileType = (TileType)DungWindow.Instance.LoadedDungFloorHeader.FloorTypeOverride;

                if (leftTileType == TileType.Override)
                    leftTileType = (TileType)DungWindow.Instance.LoadedDungFloorHeader.FloorTypeOverride;

                for (int x = 0; x < TileSizeWidth; x++)
                {
                    for (int y = 0; y < TileSizeHeight; y++)
                    {
                        var leftColour = TileTypeColour.ContainsKey(leftTileType) ? TileTypeColour[leftTileType] : Color.Magenta;
                        var rightColour = TileTypeColour.ContainsKey(rightTileType) ? TileTypeColour[rightTileType] : Color.Magenta;

                        CurrentDrawnMapLayoutBitmap.SetPixel(xId * 2 * TileSizeWidth + x, yId * TileSizeHeight + y, leftColour); //Left tile
                        CurrentDrawnMapLayoutBitmap.SetPixel((xId * 2 + 1) * TileSizeWidth + x, yId * TileSizeHeight + y, rightColour); //Right tile
                    }
                }
            }
        }

        public void UpdateTileAtPosition(int xPos, int yPos, byte tileToDraw)
        {
            xPos /= 2;

            var rightTileType = (TileType)tileToDraw.GetLefNiblet();
            var leftTileType = (TileType)tileToDraw.GetRightNiblet();

            if (rightTileType == TileType.Override)
                rightTileType = (TileType)DungWindow.Instance.LoadedDungFloorHeader.FloorTypeOverride;

            if (leftTileType == TileType.Override)
                leftTileType = (TileType)DungWindow.Instance.LoadedDungFloorHeader.FloorTypeOverride;

            for (int x = 0; x < TileSizeWidth; x++)
            {
                for (int y = 0; y < TileSizeHeight; y++)
                {
                    var leftColour = TileTypeColour.ContainsKey(leftTileType) ? TileTypeColour[leftTileType] : Color.Magenta;
                    var rightColour = TileTypeColour.ContainsKey(rightTileType) ? TileTypeColour[rightTileType] : Color.Magenta;

                    CurrentDrawnMapLayoutBitmap.SetPixel(xPos * 2 * TileSizeWidth + x, yPos * TileSizeHeight + y, leftColour);
                    CurrentDrawnMapLayoutBitmap.SetPixel((xPos * 2 + 1) * TileSizeWidth + x, yPos * TileSizeHeight + y, rightColour);
                }
            }

            var targetRect = Rectangle.FromLTRB(
                xPos * 2 * TileSizeWidth,
                yPos * TileSizeHeight,
                xPos * 2 * TileSizeWidth + TileSizeWidth,
                yPos * TileSizeHeight + TileSizeHeight
                );

            var targetRect2 = Rectangle.FromLTRB(
                (xPos * 2 + 1) * TileSizeWidth,
                yPos * TileSizeHeight,
                (xPos * 2 + 1) * TileSizeWidth + TileSizeWidth,
                yPos * TileSizeHeight + TileSizeHeight
                );

            //FloorLayoutPictureBox.Invalidate(targetRect);
            //FloorLayoutPictureBox.Invalidate(targetRect2);

            FloorLayoutPictureBox.Invalidate();
            FloorLayoutPictureBox.Update();
        }

        private void DrawFloorWarpsToBitmap()
        {
            foreach (var warp in FloorLayoutToDraw.FloorLayoutWarps)
            {
                for (int x = 0; x < TileSizeWidth; x++)
                {
                    for (int y = 0; y < TileSizeHeight; y++)
                    {
                        CurrentDrawnMapLayoutBitmap.SetPixel(warp.X * TileSizeWidth + x, warp.Y * TileSizeHeight + y, Color.Cyan);
                        string warpTypeChar = "w";
                        switch ((WarpType)warp.Type)
                        {
                            case WarpType.Entrance:
                                warpTypeChar = "E";
                                break;
                            case WarpType.Next:
                                warpTypeChar = "N";
                                break;
                            case WarpType.Exit:
                                warpTypeChar = "X";
                                break;
                            default:
                                break;
                        }
                        AddText(new Vector2(warp.X, warp.Y), warpTypeChar);
                    }
                }
            }
        }

        private void DrawFloorChestsToBitmap()
        {
            foreach (var chest in FloorLayoutToDraw.FloorLayoutChests)
            {
                for (int x = 0; x < TileSizeWidth; x++)
                {
                    for (int y = 0; y < TileSizeHeight; y++)
                    {
                        CurrentDrawnMapLayoutBitmap.SetPixel(chest.X * TileSizeWidth + x, chest.Y * TileSizeHeight + y, Color.FromArgb(255, 0, 255, 0));
                        AddText(new Vector2(chest.X, chest.Y), "T");
                    }
                }
            }
        }

        private void DrawFloorTrapsToBitmap()
        {
            foreach (var trap in FloorLayoutToDraw.FloorLayoutTraps)
            {
                for (int x = 0; x < TileSizeWidth; x++)
                {
                    for (int y = 0; y < TileSizeHeight; y++)
                    {
                        CurrentDrawnMapLayoutBitmap.SetPixel(trap.X * TileSizeWidth + x, trap.Y * TileSizeHeight + y, Color.Yellow);
                        string trapTypeChar;

                        DungFloorTrap.TrapTypeAndLevel selectedTrap = trap.TypeAndLevel.FirstOrDefault(o => (TrapLevel)o.Level != TrapLevel.Zero);
                        if (selectedTrap == null)
                            continue;

                        switch ((TrapType)selectedTrap.Type)
                        {
                            case TrapType.None:
                                trapTypeChar = "-";
                                break;
                            case TrapType.Swamp:
                                trapTypeChar = "A";
                                break;
                            case TrapType.Spore:
                                trapTypeChar = "S";
                                break;
                            case TrapType.Rock:
                                trapTypeChar = "R";
                                break;
                            case TrapType.Mine:
                                trapTypeChar = "M";
                                break;
                            case TrapType.Bit_Bug:
                            case TrapType.Energy_Bug:
                            case TrapType.Return_Bug:
                            case TrapType.Memory_bug:
                                trapTypeChar = "B";
                                break;
                            default:
                                trapTypeChar = "T";
                                break;
                        }

                        AddText(new Vector2(trap.X, trap.Y), trapTypeChar);
                    }
                }
            }
        }

        private void DrawFloorDigimonToBitmap()
        {
            foreach (var digimon in FloorLayoutToDraw.FloorLayoutDigimons)
            {
                for (int x = 0; x < TileSizeWidth; x++)
                {
                    for (int y = 0; y < TileSizeHeight; y++)
                    {
                        CurrentDrawnMapLayoutBitmap.SetPixel(digimon.X * TileSizeWidth + x, digimon.Y * TileSizeHeight + y, Color.FromArgb(255, 255, 100, 100));
                    }
                    AddText(new Vector2(digimon.X, digimon.Y), "D");
                }
            }
        }

        public void DrawGridLayout()
        {
            if (!DungWindow.Instance.DrawGridCheckBox.Checked)
                return;

            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    for (int i = 0; i < TileSizeWidth; i++)
                    {
                        for (int j = 0; j < TileSizeHeight; j++)
                        {
                            if (i == 0 || j == 0)
                            {
                                CurrentDrawnMapLayoutBitmap.SetPixel((x * TileSizeWidth) + i, (y * TileSizeHeight) + j, Color.LightGray);
                                CurrentDrawnMapLayoutBitmap.SetPixel((x * TileSizeWidth) + i, (y * TileSizeHeight) + j, Color.LightGray);
                                continue;
                            }
                        }
                    }
                }
            }
            ApplyFloorBitmapToPictureBox();
        }

        private void AddText(Vector2 pos, string text)
        {
            pos.x *= TileSizeWidth;
            pos.y *= TileSizeHeight;
            RectangleF rectf = new RectangleF(pos.x + (TileSizeWidth / 10), pos.y + (TileSizeHeight / 10), TileSizeWidth, TileSizeHeight + TileSizeHeight / 10);

            Graphics g = Graphics.FromImage(CurrentDrawnMapLayoutBitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, new Font("Tahoma", TileSizeHeight / 1.6f), Brushes.Black, rectf);

            g.Flush();
        }

        private void ApplyFloorBitmapToPictureBox()
        {
            FloorLayoutPictureBox.Image = CurrentDrawnMapLayoutBitmap;
        }


    }
}
