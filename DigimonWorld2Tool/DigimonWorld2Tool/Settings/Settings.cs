using DigimonWorld2Tool.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DigimonWorld2Tool.Settings
{
    public class Settings
    {
        public static string BaseDllDirectory
        {
            get
            {
                var dllLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                //Trim the first 6 characters off the start of the path to remove "file://"
                return dllLocation[6..];
            }

        }

        private static bool isUsingDarkModeIsSet;
        private static bool isUsingDarkMode;
        public static bool IsUsingDarkMode
        {
            get
            {
                if(!isUsingDarkModeIsSet)
                {
                    isUsingDarkMode = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1) == 0;
                    isUsingDarkModeIsSet = true;
                }

                return isUsingDarkMode;
            }
        }

        private static Color? backgroundColour = null;
        public static Color? BackgroundColour
        {
            get
            {
                if(backgroundColour == null)
                    backgroundColour = IsUsingDarkMode ? Color.FromArgb(18,18,18) : Color.FromArgb(230,230,230); 

                return backgroundColour;
            }
        }

        private static Color? buttonBackgroundColour = null;
        public static Color? ButtonBackgroundColour
        {
            get
            {
                if (buttonBackgroundColour == null)
                    buttonBackgroundColour = IsUsingDarkMode ? Color.FromArgb(24, 24, 24) : Color.FromArgb(240, 240, 240);

                return buttonBackgroundColour;
            }
        }

        private static Color? buttonSelectedBackgroundColour = null;
        public static Color? ButtonSelectedBackgroundColour
        {
            get
            {
                if(buttonSelectedBackgroundColour== null)
                    buttonSelectedBackgroundColour = IsUsingDarkMode ? Color.FromArgb(50, 50, 50) : Color.FromArgb(190, 240, 240);

                return buttonSelectedBackgroundColour;
            }
        }

        private static Color? panelBackgroundColour = null;
        public static Color? PanelBackgroundColour
        {
            get
            {
                if (panelBackgroundColour == null)
                    panelBackgroundColour = IsUsingDarkMode ? Color.FromArgb(24,24,24) : Color.FromArgb(255,255,255);

                return panelBackgroundColour;
            }
        }

        private static Color? textColour = null;
        public static Color? TextColour
        {
            get
            {
                if (textColour == null)
                    textColour = IsUsingDarkMode ? Color.FromArgb(255,255,255) : Color.FromArgb(0,0,0);

                return textColour;
            }
        }

        private static List<DUNGMapping> dungMapping;
        public static List<DUNGMapping> DungMapping
        {
            get
            {
                List<string[]> mapping;
                if (dungMapping == null)
                {
                    mapping = CsvParser.Parse($"{BaseDllDirectory}/Mappings/DUNG.csv");

                    dungMapping = new List<DUNGMapping>();
                    foreach (var item in mapping)
                        dungMapping.Add(new DUNGMapping(item[0], item[1]));
                }
                return dungMapping;
            }
        }
    }

    public class DUNGMapping
    {
        public string Filename { get; private set; }
        public string DomainName { get; private set; }

        public DUNGMapping(string filename, string domainName)
        {
            this.Filename = filename;
            this.DomainName = domainName;
        }

        public override string ToString()
        {
            return $"{Filename} -> {DomainName}";
        }
    }
}
