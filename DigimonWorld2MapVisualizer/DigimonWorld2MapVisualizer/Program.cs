using System;
using System.Linq;
using DigimonWorld2MapVisualizer.Files;
using System.Reflection;

namespace DigimonWorld2MapVisualizer
{
    internal class Program
    {
        public static bool ShowOriginalValueInMapTile = false;
        private static readonly DungFile[] DungeonFiles = new DungFile[] 
        {
            new DungFile("DUNG4000", "SCSI Domain 1", 0x00, 0x3A),
            new DungFile("DUNG4100", "Video Domain 1", 0x01, 0xE1),
            new DungFile("DUNG4200", "Disk Domain 1", 0x02, 0xE2),
            new DungFile("DUNG4300", "BIOS Domain 1", 0x03, 0x26),
            new DungFile("DUNG4400", "Drive Domain 1", 0x04, 0xE3),
            new DungFile("DUNG4500", "Web Domain 1", 0x05, 0xE4),
            new DungFile("DUNG4600", "Modem Domain 1", 0x06, 0xE5),
            new DungFile("DUNG4700", "SCSI Domain 2", 0x07, 0xE6),
            new DungFile("DUNG4800", "Video Domain 2", 0x08, 0xE7),
            new DungFile("DUNG4900", "Disk Domain 2", 0x09, 0xE8),
            new DungFile("DUNG5000", "BIOS Domain 2", 0x0A, 0xE9),
            new DungFile("DUNG5100", "Drive Domain 2", 0x0B, 0xEA),
            new DungFile("DUNG5200", "Web Domain 2", 0x0C, 0xEB),
            new DungFile("DUNG5300", "Modem Domain 2", 0x0D, 0xEC),
            new DungFile("DUNG5400", "Code Domain", 0x0E, 0xED),
            new DungFile("DUNG5500", "Laser Domain", 0x0F, 0xEE),
            new DungFile("DUNG5600", "Giga Domain", 0x10, 0xEF),
            new DungFile("DUNG5700", "Diode Domain", 0x11, 0xF0),
            new DungFile("DUNG5800", "Port Domain", 0x12, 0xF1),
            new DungFile("DUNG5900", "Scan Domain", 0x13, 0xF2),
            new DungFile("DUNG6000", "Data Domain", 0x14, 0xF3),
            new DungFile("DUNG6100", "Patch Domain", 0x15, 0xF4),
            new DungFile("DUNG6200", "Mega Domain", 0x16, 0xF5),
            new DungFile("DUNG6300", "Soft Domain", 0x17, 0xF6),
            new DungFile("DUNG6400", "Bug Domian", 0x18, 0xF7),
            new DungFile("DUNG6500", "RAM Domian", 0x19, 0xF8),
            new DungFile("DUNG6600", "ROM Domain", 0x1A, 0xF9),
            new DungFile("DUNG6700", "Core Tower", 0x1B, 0xFA),
            new DungFile("DUNG6800", "Chaos Tower", 0x1C, 0xFB),
            new DungFile("DUNG6900", "Boot Domain", 0x1D, 0xFC),
            new DungFile("DUNG7000", "DVD Domain", 0x1E, 0xFD),
            new DungFile("DUNG7100", "Power Domain", 0x1F, 0xFE),
            new DungFile("DUNG7200", "Tera Domain", 0x20, 0xFF),
            new DungFile("DUNG7300", "ABCDE", 0x21, 0x00),
        };
        public static System.Diagnostics.Stopwatch watch;
        private static void Main(string[] args)
        {
            PrintInfo();
            InitConsole();
            PrintDungFileNames();
            DungeonFileSelector();
        }

        /// <summary>
        /// Print a list of all possible dungeons, containing the Domain name, ID, Filename and DATA4000 id. 
        /// </summary>
        public static void DungeonFileSelector()
        {
            Console.Write("\nPlease enter the ID or (full) domain name: ");
            string input = Console.ReadLine();

            if (!ValidateInput(input, out string dungeonFilename))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error; \"{input}\" was not found, please try again!");
                Console.ForegroundColor = ConsoleColor.Gray;
                DungeonFileSelector();
            }

            Console.WriteLine($"Loading {dungeonFilename}");
            PrintIndex();

            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            new Domain(dungeonFilename);
        }

        /// <summary>
        /// Notify the user that the visualization has completed, and asks if he user wants to render another map
        /// </summary>
        public static void FinishUpVisualization()
        {
            Console.WriteLine("\n\nEnd of visualisation, Do you wish to quit? y/n. " +
                              "\nEntering n will take you back to the map selector.");

            string input = Console.ReadLine();
            if(input.ToLower().Equals("y"))
            {
                return;
            }
            else if(input.ToLower().Equals("n"))
            {
                Console.WriteLine();
                PrintDungFileNames();
                DungeonFileSelector();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error; Input unrecognised. Please enter either y or n");
                Console.ForegroundColor = ConsoleColor.Gray;
                FinishUpVisualization();
            }
        }

        /// <summary>
        /// Checks if the user's input is either numerical or textual
        /// - If the input is numerical we search the DUNG files by ID
        /// - If the input is textual we search the DUNG files by domain name
        /// </summary>
        /// <param name="input">THe user inputted text</param>
        /// <param name="dungeonFilename">The Filename that was found</param>
        /// <returns>True if a file was found, false otherwise.</returns>
        private static bool ValidateInput(string input, out string dungeonFilename)
        {
            dungeonFilename = "";
            if(int.TryParse(input, out int result))
            {
                DungFile dungFile = DungeonFiles.FirstOrDefault(o => o.DomainIDDecimal == result);
                if(dungFile != null)
                {
                    dungeonFilename = dungFile.Filename;
                    return true;
                }
            }
            else
            {
                input = input.ToLower();
                DungFile dungFile = DungeonFiles.FirstOrDefault(o => o.DomainName.ToLower() == input || o.DomainName.ToLower().Contains(input));
                if (dungFile != null)
                {
                    dungeonFilename = dungFile.Filename;
                    return true;
                }
            }
            return false;
        }

        private static void InitConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.SetBufferSize(Console.BufferWidth, 32766);
            Console.WindowWidth = Console.LargestWindowWidth - 100;
            Console.WindowHeight = Console.LargestWindowHeight - 10;
        }

        private static void PrintInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Assembly assembly = typeof(Program).Assembly;
            Console.Write($"Starting {assembly.GetName().Name}");
            Console.Write($"\nVersion: {assembly.GetName().Version}");
            Console.Write($"\nMade by Remy_RM");
            Console.Write($"\nFound a bug or got feedback? Please create an issue on GitHub\nhttps://github.com/RemyRM/DigimonWorld2Visualizer");
            Console.Write($"\n\nNotes:" +
                          $"\n- Currently chests and traps are always rendered, regardless of the actual spawn chance." +
                          $"\n- The application looks for a directory called \\Maps\\ that contains the DUNGxxxx.BIN files," +
                          $"\n  this folder needs to be placed in the same folder as the executable." +
                          $"\n  The DUNGxxxx.BIN files can be found in the \\data\\ folder at the root of the repository." +
                          $"\n- This is just a proof of concept build while i'm working on a full version, and subject to change.");
            Console.Write($"\n\nKnown bugs:" +
                          $"\n- Changing the width of the console breaks the maps" +
                          $"\n- The level of the digimon packs is not shown correctly" +
                          $"\n- When a map reached the max width of 64 it bleeds colours to the side");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void PrintDungFileNames()
        {
            Console.WriteLine("\n\nPlease select one of the following domains to load using either its ID or Domain name: ");
            for (int i = 0; i < DungeonFiles.Length; i++)
            {
                Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.White : ConsoleColor.Gray;
                Console.WriteLine(DungeonFiles[i]);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void PrintIndex()
        {
            Console.WriteLine("\n** Index **\n");
            Console.WriteLine($"Tiles colour index: " +
                              $"\nEmpty    - Black" +
                              $"\nRoom     - White" +
                              $"\nCorridor - Grey" +
                              $"\nWater    - Dark Blue" +
                              $"\nFire     - Dark Red" +
                              $"\nNature   - Dark Green" +
                              $"\nMachine  - Dark Yellow" +
                              $"\nDark     - Dark Magenta");
            Console.WriteLine();
            Console.WriteLine($"Objects colour index: " +
                              $"\nWarp Entrance   - Cyan   + WE" +
                              $"\nWarp Next       - Cyan   + WN" +
                              $"\nWarp Exit       - Cyan   + WX" +
                              $"\nTreasure Chest  - Green  + TC" +
                              $"\nTrap Spore      - Yellow + SP" +
                              $"\nTrap Rock       - Yellow + RO" +
                              $"\nTrap Swamp      - Yellow + SW" +
                              $"\nTrap Mine       - Yellow + MI" +
                              $"\nTrap Bit Bug    - Yellow + BB" +
                              $"\nTrap Energy Bug - Yellow + EB" +
                              $"\nTrap Return Bug - Yellow + RB" +
                              $"\nTrap Memory Bug - Yellow + MB" +
                              $"\nDigimon rookie  - Red    + RO" +
                              $"\nDigimon Champ   - Red    + CH" +
                              $"\nDigimon Ulti    - Red    + UL" +
                              $"\nDigimon Mega    - Red    + ME");

            Console.WriteLine();
        }
    }
}
