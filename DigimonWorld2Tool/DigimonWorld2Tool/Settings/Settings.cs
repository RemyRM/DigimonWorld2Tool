using DigimonWorld2Tool.FileFormat;
using DigimonWorld2Tool.FileFormats;
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

        public static string DIGIMNDTFilePath{ get => $"{BaseDllDirectory}/Resources/DataFiles/DIGIMNDT.BIN"; }
        private static DIGIMNDT _DIGIMNDTFile;
        public static DIGIMNDT DIGIMNDTFile
        {
            get
            {
                if (_DIGIMNDTFile == null)
                    _DIGIMNDTFile = new DIGIMNDT();

                return _DIGIMNDTFile;
            }
        }

        public static string ENEMYSETFilePath { get => $"{BaseDllDirectory}/Resources/DataFiles/ENEMYSET.BIN"; }
        private static ENEMYSET _ENEMYSETFile;
        public static ENEMYSET ENEMYSETFile
        {
            get
            {
                if (_ENEMYSETFile == null)
                    _ENEMYSETFile = new ENEMYSET();

                return _ENEMYSETFile;
            }
        }

        public static string MODELDT0FilePath { get => $"{BaseDllDirectory}/Resources/DataFiles/MODELDT0.BIN"; }
        private static MODELDT0 _MODELDT0File;
        public static MODELDT0 MODELDT0File
        {
            get
            {
                if (_MODELDT0File == null)
                    _MODELDT0File = new MODELDT0();

                return _MODELDT0File;
            }
        }

        public static string ITEMDATAFilePath { get => $"{BaseDllDirectory}/Resources/DataFiles/ITEMDATA.BIN"; }
        private static ITEMDATA _ITEMDATAFile;
        public static ITEMDATA ITEMDATAFile
        {
            get
            {
                if (_ITEMDATAFile == null)
                    _ITEMDATAFile = new ITEMDATA();

                return _ITEMDATAFile;
            }
        }

        public static string SLUSFilePath { get => $"{BaseDllDirectory}/Resources/DataFiles/SLUS_011.93"; }
        private static SLUS _SLUSFile;
        public static SLUS SLUSFile
        {
            get
            {
                if (_SLUSFile == null)
                    _SLUSFile = new SLUS();

                return _SLUSFile;
            }
        }

        private static bool isUsingDarkModeIsSet;
        private static bool isUsingDarkMode;
        public static bool IsUsingDarkMode
        {
            get
            {
                if (!isUsingDarkModeIsSet)
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
                if (backgroundColour == null)
                    backgroundColour = IsUsingDarkMode ? Color.FromArgb(18, 18, 18) : Color.FromArgb(230, 230, 230);

                return backgroundColour;
            }
        }

        private static Color? buttonBackgroundColour = null;
        public static Color? ButtonBackgroundColour
        {
            get
            {
                if (buttonBackgroundColour == null)
                    buttonBackgroundColour = IsUsingDarkMode ? Color.FromArgb(24, 24, 24) : Color.FromArgb(210, 210, 210);

                return buttonBackgroundColour;
            }
        }

        private static Color? buttonSelectedBackgroundColour = null;
        public static Color? ButtonSelectedBackgroundColour
        {
            get
            {
                if (buttonSelectedBackgroundColour == null)
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
                    panelBackgroundColour = IsUsingDarkMode ? Color.FromArgb(24, 24, 24) : Color.FromArgb(255, 255, 255);

                return panelBackgroundColour;
            }
        }

        private static Color? textColour = null;
        public static Color? TextColour
        {
            get
            {
                if (textColour == null)
                    textColour = IsUsingDarkMode ? Color.FromArgb(255, 255, 255) : Color.FromArgb(0, 0, 0);

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

        private static string valueTextFormat = null;
        public static string ValueTextFormat
        {
            get
            {
                if(valueTextFormat == null)
                    valueTextFormat = (bool)Properties.Settings.Default["ShowValuesAsHex"] ? "X2" : "D2";

                return valueTextFormat;
            }
            set => valueTextFormat = value;
        }

        private static bool removeIDCap = false;
        public static bool RemoveIDCap
        {
            get => removeIDCap;
            set => removeIDCap = value;
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
