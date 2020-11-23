using System;

namespace DigimonWorld2MapVisualizer
{
    class Program
    {
        private static readonly string mapFileName = "DUNG7000.BIN"; // 4000 SCSI, 4900 has coloured floors, 7000 is DVD Domain which has 1 floor only
        public static bool ShowOriginalValueInMapTile = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Digimon World 2 domain visualizer\n");
                
            InitConsole();
            PrintIndex();

            Domain dom = new Domain(mapFileName);

            Console.WriteLine("\n\nEnd of visualisation, press any key to exit.");
            Console.ReadLine();
        }

        static void InitConsole()
        {
            Console.SetBufferSize(Console.BufferWidth, 32766);
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;
        }

        static void PrintIndex()
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
        }
    }
}
