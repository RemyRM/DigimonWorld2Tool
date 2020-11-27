using System;
using System.Collections.Generic;
using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.MapObjects;

namespace DigimonWorld2MapVisualizer.Domains
{
    public class DomainTileCombo
    {
        public string TileValueHex;
        public byte TileValueDec;

        public readonly Tile leftTile;
        public readonly Tile rightTile;
        public readonly Vector2 Position;

        public readonly Dictionary<byte, string> TileComboLookup = new Dictionary<byte, string>()
        {
            {0x88, "Empty_Empty"},
            {0x0A, "Water_Room"},
            {0x0B, "Fire_Room"},
            {0x0C, "Nature_Room"},
            {0x0D, "Machine_Room"},
            {0x0E, "Dark_Room"},
            {0x0F, "Empty_Room"},
            {0x00, "Room_Room"},
            {0x01, "Corridor_Room"},
            {0x02, "Water_Room"},
            {0x03, "Fire_Room"},
            {0x04, "Nature_Room"},
            {0x05, "Machine_Room"},
            {0x06, "Dark_Room"},
            {0x07, "Room_Room"},
            {0x08, "Empty_Room"},
            {0x09, "Empty_Room"},
            {0x10, "Room_Corridor"},
            {0x11, "Corridor_Corridor"},
            {0x12, "Water_Corridor"},
            {0x13, "Fire_Corridor"},
            {0x14, "Nature_Corridor"},
            {0x15, "Machine_Corridor"},
            {0x16, "Dark_Corridor"},
            {0x17, "Room_Corridor"},
            {0x18, "Empty_Corridor"},
            {0x19, "Empty_Corridor"},
            {0x1A, "Water_Corridor"},
            {0x1B, "Fire_Corridor"},
            {0x1C, "Nature_Corridor"},
            {0x1D, "Machine_Corridor"},
            {0x1E, "Dark_Corridor"},
            {0x1F, "Room_Corridor"},
            {0x20, "Room_Water"},
            {0x21, "Corridor_Water"},
            {0x22, "Water_Water"},
            {0x23, "Fire_Water"},
            {0x24, "Nature_Water"},
            {0x25, "Machine_Water"},
            {0x26, "Dark_Water"},
            {0x27, "Room_Water"},
            {0x28, "Empty_Water"},
            {0x29, "Empty_Water"},
            {0x2a, "Water_Water"},
            {0x2b, "Fire_Water"},
            {0x2c, "Nature_Water"},
            {0x2d, "Machine_Water"},
            {0x2e, "Dark_Water"},
            {0x2f, "Room_Water"},
            {0x30, "Room_Fire"},
            {0x31, "Corridor_Fire"},
            {0x32, "Water_Fire"},
            {0x33, "Fire_Fire"},
            {0x34, "Nature_Fire"},
            {0x35, "Machine_Fire"},
            {0x36, "Dark_Fire"},
            {0x37, "Room_Fire"},
            {0x38, "Empty_Fire"},
            {0x39, "Empty_Fire"},
            {0x3A, "Water_Fire"},
            {0x3B, "Fire_Fire"},
            {0x3C, "Nature_Fire"},
            {0x3D, "Machine_Fire"},
            {0x3E, "Dark_Fire"},
            {0x3F, "Machine_Fire"},
            {0x40, "Room_Nature"},
            {0x41, "Corridor_Nature"},
            {0x42, "Water_Nature"},
            {0x43, "Fire_Nature"},
            {0x44, "Nature_Nature"},
            {0x45, "Machine_Nature"},
            {0x46, "Dark_Nature"},
            {0x47, "Room_Nature"},
            {0x48, "Empty_Nature"},
            {0x49, "Empty_Nature"},
            {0x4A, "Water_Nature"},
            {0x4B, "Fire_Nature"},
            {0x4C, "Nature_Nature"},
            {0x4D, "Machine_Nature"},
            {0x4E, "Dark_Nature"},
            {0x4F, "Room_Nature"},
            {0x50, "Room_Machine"},
            {0x51, "Corridor_Machine"},
            {0x52, "Water_Machine"},
            {0x53, "Fire_Machine"},
            {0x54, "Nature_Machine"},
            {0x55, "Machine_Machine"},
            {0x56, "Dark_Machine"},
            {0x57, "Room_Machine"},
            {0x58, "Empty_Machine"},
            {0x59, "Empty_Machine"},
            {0x5A, "Water_Machine"},
            {0x5B, "Fire_Machine"},
            {0x5C, "Nature_Machine"},
            {0x5D, "Machine_Machine"},
            {0x5E, "Dark_Machine"},
            {0x5F, "Room_Machine"},
            {0x60, "Room_Dark"},
            {0x61, "Corridor_Dark"},
            {0x62, "Water_Dark"},
            {0x63, "Fire_Dark"},
            {0x64, "Nature_Dark"},
            {0x65, "Machine_Dark"},
            {0x66, "Dark_Dark"},
            {0x67, "Room_Dark"},
            {0x68, "Empty_Dark"},
            {0x69, "Empty_Dark"},
            {0x6A, "Water_Dark"},
            {0x6B, "Fire_Dark"},
            {0x6C, "Nature_Dark"},
            {0x6D, "Machine_Dark"},
            {0x6E, "Dark_Dark"},
            {0x6F, "Room_Dark"},
            {0x70, "Room_Room"},
            {0x71, "Corridor_Room"},
            {0x72, "Water_Room"},
            {0x73, "Fire_Room"},
            {0x74, "Nature_Room"},
            {0x75, "Machine_Room"},
            {0x76, "Dark_Room"},
            {0x77, "Room_Room"},
            {0x78, "Empty_Room"},
            {0x79, "Empty_Room"},
            {0x7A, "Water_Room"},
            {0x7B, "Fire_Room"},
            {0x7C, "Nature_Room"},
            {0x7D, "Machine_Room"},
            {0x7E, "Dark_Room"},
            {0x7F, "Room_Room"},
            {0x80, "Room_Empty"},
            {0x81, "Corridor_Empty"},
            {0x82, "Water_Empty"},
            {0x83, "Fire_Empty"},
            {0x84, "Nature_Empty"},
            {0x85, "Machine_Empty"},
            {0x86, "Dark_Empty"},
            {0x87, "Room_Empty"},
            {0x89, "Empty_Empty"},
            {0x8A, "Water_Empty"},
            {0x8B, "Fire_Empty"},
            {0x8C, "Nature_Empty"},
            {0x8D, "Machine_Empty"},
            {0x8E, "Dark_Empty"},
            {0x8F, "Room_Empty"},
            {0x90, "Room_Empty"},
            {0x91, "Corridor_Empty"},
            {0x92, "Water_Empty"},
            {0x93, "Fire_Empty"},
            {0x94, "Nature_Empty"},
            {0x95, "Machine_Empty"},
            {0x96, "Dark_Empty"},
            {0x97, "Room_Empty"},
            {0x98, "Empty_Empty"},
            {0x99, "Empty_Empty"},
            {0x9A, "Water_Empty"},
            {0x9B, "Fire_Empty"},
            {0x9C, "Nature_Empty"},
            {0x9D, "Machine_Empty"},
            {0x9E, "Dark_Empty"},
            {0x9F, "Room_Empty"},
            {0xA0, "Room_Water"},
            {0xA1, "Corridor_Water"},
            {0xA2, "Water_Water"},
            {0xA3, "Fire_Water"},
            {0xA4, "Nature_Water"},
            {0xA5, "Machine_Water"},
            {0xA6, "Dark_Water"},
            {0xA7, "Room_Water"},
            {0xA8, "Room_Water"},
            {0xA9, "Empty_Water"},
            {0xAA, "Water_Water"},
            {0xAB, "Fire_Water"},
            {0xAC, "Nature_Water"},
            {0xAD, "Machine_Water"},
            {0xAE, "Dark_Water"},
            {0xAF, "Room_Water"},
        };

        /// <summary>
        /// Container class that contains both the left and right tile at this position.
        /// </summary>
        /// <remarks>We need this containe because the game stores the data for two tiles in a single byte.
        /// This makes a single grid entry 2x1, instead of the usual 1x1.</remarks>
        /// <param name="position">The position of this tile combo</param>
        /// <param name="tileValue">The value containing the information for the left and right tile</param>
        public DomainTileCombo(Vector2 position, byte tileValue)
        {
            position = new Vector2(position.x * 2, position.y); // Since the grid is 32x48 but a single "tile" is actually 2x1 in size we have to double the x position
            this.TileValueDec = tileValue;
            this.TileValueHex = tileValue.ToString("X2");
            this.Position = position;

            string[] splitTilesLeftRight = TileComboLookup[TileValueDec].ToString().Split('_');
            var leftTileType = (Tile.DomainTileType)Enum.Parse(typeof(Tile.DomainTileType), splitTilesLeftRight[0]);
            var rightTileType = (Tile.DomainTileType)Enum.Parse(typeof(Tile.DomainTileType), splitTilesLeftRight[1]);

            leftTile = new Tile(position, leftTileType);
            rightTile = new Tile(position + Vector2.Right, rightTileType); //We add 1 to the x position to get the true position of the right tile
        }

        public void Draw()
        {
            DrawLeftTile();
            DrawRightTile();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            if (leftTile.Position.x == 62)
                System.Diagnostics.Debug.Write(Environment.NewLine);
        }

        private void DrawLeftTile()
        {
            Console.BackgroundColor = leftTile.TileColour;
            //if (Program.ShowOriginalValueInMapTile)
            //{
            //    if (Console.BackgroundColor == ConsoleColor.Gray || Console.BackgroundColor == ConsoleColor.White)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Black;
            //        if (rightTile.FloorObject != null)
            //        {
            //            System.Diagnostics.Debug.Write(rightTile.FloorObjectText);
            //        }
            //        else
            //        {
            //            System.Diagnostics.Debug.Write(TileValueHex);
            //        }
            //        Console.ForegroundColor = ConsoleColor.Gray;
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.Write(TileValueHex);
            //    }
            //}
            //else
            {
                if (leftTile.Position.y == 0)
                {
                    System.Diagnostics.Debug.Write($"{(leftTile.Position.x):00}");
                }
                else if (leftTile.Position.x == 0)
                {
                    System.Diagnostics.Debug.Write($"{leftTile.Position.y:00}");
                }
                else
                {
                    if (leftTile.FloorObject != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        System.Diagnostics.Debug.Write(leftTile.FloorObjectText);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        System.Diagnostics.Debug.Write("  ");
                    }
                }
            }
        }

        private void DrawRightTile()
        {
            Console.BackgroundColor = rightTile.TileColour;
            if (rightTile.FloorObject != null)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                System.Diagnostics.Debug.Write(rightTile.FloorObjectText);
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            else
            {
                System.Diagnostics.Debug.Write("  ");
            }
        }
    }

    public class Tile
    {
        public readonly Dictionary<DomainTileType, ConsoleColor> TileTypeColour = new Dictionary<DomainTileType, ConsoleColor>
        {
            {DomainTileType.Empty, ConsoleColor.Black },
            {DomainTileType.Room, ConsoleColor.Gray },
            {DomainTileType.Corridor, ConsoleColor.DarkGray },
            {DomainTileType.Water, ConsoleColor.DarkBlue },
            {DomainTileType.Fire, ConsoleColor.DarkRed },
            {DomainTileType.Nature, ConsoleColor.DarkGreen },
            {DomainTileType.Machine, ConsoleColor.DarkYellow },
            {DomainTileType.Dark, ConsoleColor.DarkMagenta},
        };
        public readonly Dictionary<IFloorLayoutObject.MapObjectType, ConsoleColor> FloorObjectTypeColour = new Dictionary<IFloorLayoutObject.MapObjectType, ConsoleColor>
        {
            {IFloorLayoutObject.MapObjectType.Chest, ConsoleColor.Green },
            {IFloorLayoutObject.MapObjectType.Digimon, ConsoleColor.Red},
            {IFloorLayoutObject.MapObjectType.Warp, ConsoleColor.Cyan},
            {IFloorLayoutObject.MapObjectType.Trap, ConsoleColor.Yellow},
        };

        public enum DomainTileType : byte
        {
            Empty,
            Room,
            Corridor,
            Water,
            Fire,
            Nature,
            Machine,
            Dark,
        }

        public readonly Vector2 Position;
        public readonly DomainTileType TileType;
        public ConsoleColor TileColour { get; private set; }
        public IFloorLayoutObject FloorObject { get; private set; }
        public string FloorObjectText { get; private set; } = "  ";

        public Tile(Vector2 position, DomainTileType tileType)
        {
            this.Position = position;
            this.TileType = tileType;
            this.TileColour = GetConsoleBackgroundColourBasedOnTileType(tileType);
        }

        public ConsoleColor GetConsoleBackgroundColourBasedOnTileType(DomainTileType type)
        {
            return TileTypeColour.GetValueOrDefault(type);
        }

        public void AddObjectToTile(IFloorLayoutObject objectToPlace)
        {
            FloorObject = objectToPlace;
            TileColour = FloorObjectTypeColour[FloorObject.ObjectType];

            if (FloorObject.ObjectType == IFloorLayoutObject.MapObjectType.Warp)
            {
                Warp warp = (Warp)FloorObject;
                switch (warp.Type)
                {
                    case Warp.WarpType.Entrance:
                        FloorObjectText = "WE";
                        break;
                    case Warp.WarpType.Next:
                        FloorObjectText = "WN";
                        break;
                    case Warp.WarpType.Exit:
                        FloorObjectText = "WX";
                        break;
                    default:
                        break;
                }
            }
            else if(FloorObject.ObjectType == IFloorLayoutObject.MapObjectType.Chest)
            {
                FloorObjectText = "TC";
            }
            else if(FloorObject.ObjectType == IFloorLayoutObject.MapObjectType.Trap)
            {
                Trap trap = (Trap)FloorObject;
                switch (trap.Type)
                {
                    case Trap.TrapSlot.TrapType.None:
                        FloorObjectText = "EM";
                        break;
                    case Trap.TrapSlot.TrapType.Swamp:
                        FloorObjectText = "SW";
                        break;
                    case Trap.TrapSlot.TrapType.Spore:
                        FloorObjectText = "SP";
                        break;
                    case Trap.TrapSlot.TrapType.Rock:
                        FloorObjectText = "RO";
                        break;
                    case Trap.TrapSlot.TrapType.Mine:
                        FloorObjectText = "MI";
                        break;
                    case Trap.TrapSlot.TrapType.Bit_Bug:
                        FloorObjectText = "BB";
                        break;
                    case Trap.TrapSlot.TrapType.Energy_Bug:
                        FloorObjectText = "EB";
                        break;
                    case Trap.TrapSlot.TrapType.Return_Bug:
                        FloorObjectText = "RB";
                        break;
                    case Trap.TrapSlot.TrapType.Memory_bug:
                        FloorObjectText = "MB";
                        break;
                    default:
                        break;
                }
            }
            else if(FloorObject.ObjectType == IFloorLayoutObject.MapObjectType.Digimon)
            {
                Digimon digimon = (Digimon)FloorObject;
                switch (digimon.Level)
                {
                    case Digimon.DigimonPackLevel.Rookie:
                        FloorObjectText = "RO";
                        break;
                    case Digimon.DigimonPackLevel.Champion:
                        FloorObjectText = "CH";
                        break;
                    case Digimon.DigimonPackLevel.Ultimate:
                        FloorObjectText = "UL";
                        break;
                    case Digimon.DigimonPackLevel.Mega:
                        FloorObjectText = "ME";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
