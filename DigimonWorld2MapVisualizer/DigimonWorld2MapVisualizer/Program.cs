using System;
using System.Diagnostics;

namespace DigimonWorld2MapVisualizer
{
    internal class Program
    {
        private static string mapFileName = "";/*"DUNG7000.BIN";*/ // 4000 SCSI, 4900 has coloured floors, 7000 is DVD Domain which has 1 floor only
        public static bool ShowOriginalValueInMapTile = false;

        private static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Console.WriteLine("Starting Digimon World 2 domain visualizer\n");
            if(mapFileName.Equals(""))
            {
                Console.Write("Please enter the map number you want to load in: ");
                mapFileName = $"DUNG{Console.ReadLine()}.BIN";
            }
            
            InitConsole();
            PrintIndex();

            Domain dom = new Domain(mapFileName);

            Console.WriteLine("\n\nEnd of visualisation, press any key to exit.");

            watch.Stop();
            Console.WriteLine($"Visualizing took {watch.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        private static void InitConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.SetBufferSize(Console.BufferWidth, 32766);
            Console.WindowWidth = Console.LargestWindowWidth - 100;
            Console.WindowHeight = Console.LargestWindowHeight;
        }

        private static void PrintIndex()
        {
            Console.WriteLine($"Colour index: " +
                              $"\nEmpty    - Black" +
                              $"\nRoom     - White" +
                              $"\nCorridor - Grey" +
                              $"\nWater    - Dark Blue" +
                              $"\nFire     - Dark Red" +
                              $"\nNature   - Dark Green" +
                              $"\nMachine  - Dark Yellow" +
                              $"\nDark     - Dark Magenta");
            Console.WriteLine();
        }
    }
}
