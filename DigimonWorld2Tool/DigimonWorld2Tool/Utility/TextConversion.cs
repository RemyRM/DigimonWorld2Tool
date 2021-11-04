using System;
using System.Collections.Generic;

namespace DigimonWorld2Tool.Utility
{
    public class TextConversion
    {
        private static readonly Dictionary<byte, string> CharacterLookupTable = new Dictionary<byte, string>()
        {
            {0x00, "0"},
            {0x01, "1"},
            {0x02, "2"},
            {0x03, "3"},
            {0x04, "4"},
            {0x05, "5"},
            {0x06, "6"},
            {0x07, "7"},
            {0x08, "8"},
            {0x09, "9"},
            {0x0A, "A"},
            {0x0B, "B"},
            {0x0C, "C"},
            {0x0D, "D"},
            {0x0E, "E"},
            {0x0F, "F"},
            {0x10, "G"},
            {0x11, "H"},
            {0x12, "I"},
            {0x13, "J"},
            {0x14, "K"},
            {0x15, "L"},
            {0x16, "M"},
            {0x17, "N"},
            {0x18, "O"},
            {0x19, "P"},
            {0x1A, "Q"},
            {0x1B, "R"},
            {0x1C, "S"},
            {0x1D, "T"},
            {0x1E, "U"},
            {0x1F, "V"},
            {0x20, "W"},
            {0x21, "X"},
            {0x22, "Y"},
            {0x23, "Z"},
            {0x24, "a"},
            {0x25, "b"},
            {0x26, "c"},
            {0x27, "d"},
            {0x28, "e"},
            {0x29, "f"},
            {0x2A, "g"},
            {0x2B, "h"},
            {0x2C, "i"},
            {0x2D, "j"},
            {0x2E, "k"},
            {0x2F, "l"},
            {0x30, "m"},
            {0x31, "n"},
            {0x32, "o"},
            {0x33, "p"},
            {0x34, "q"},
            {0x35, "r"},
            {0x36, "s"},
            {0x37, "t"},
            {0x38, "u"},
            {0x39, "v"},
            {0x3A, "w"},
            {0x3B, "x"},
            {0x3C, "y"},
            {0x3D, "z"},
            {0x41, "<SQUARE>"},
            {0x44, "?"},
            {0x45, "!"},
            {0x46, "/"},
            {0x49, "-"},
            {0x54, ","},
            {0x55, "."},
            {0x56, ""},
            {0x5B, "PLUS SIGN"},
            {0xFB, "<Input X>"},
            {0xFC, "\n<BOX>"},
            {0xFD, " "},
            {0xFE, "<ENTER>"},
        };

        /// <summary>
        /// Note: All value in this list are prefixed by 0xF0
        /// </summary>
        private static readonly Dictionary<byte, string> CommonWordsLookUpTable = new Dictionary<byte, string>()
        {
            {0x00, "<Akira>"},
            {0x06, "<Digimon>"},
            {0x07, "<you>"},
            {0x08, "<the>"},
            {0x09, "<Digi-Beetle>"},
            {0x0A, "<Domain>"},
            {0x0B, "<Guard>"},
            {0x0C, "<Tamer>"},
            {0x0D, "<here>"},
            {0x0E, "<have>"},
            {0x0F, "<Knights>"},
            {0x10, "<and>"},
            {0x11, "<thing>"},
            {0x12, "<Security>"},
            {0x13, "<that>"},
            {0x14, "<Bertran>"},
            {0x15, "<Tournament>"},
            {0x16, "<Crimson>"},
            {0x18, "<something>"},
            {0x19, "<Item>"},
            {0x1A, "<Falcon>"},
            {0x1B, "<for>"},
            {0x1C, "<That's>"},
            {0x1D, "<Commander>"},
            {0x1E, "<Blood>"},
            {0x1F, "<Leader>"},
            {0x20, "<Attendant>"},
            {0x21, "<Cecilia>"},
            {0x22, "<all>"},
            {0x23, "<mission>"},
            {0x24, "<this>"},
            {0x26, "<Archive>"},
            {0x27, "<Black>"},
            {0x28, "<I'll>"},
            {0x29, "<are>"},
            {0x2A, "<Sword>"},
            {0x2B, "<right>"},
            {0x2C, "<Digivolve>"},
            {0x2D, "<enter>"},
            {0x2E, "<What>"},
            {0x2F, "<will>"},
            {0x30, "<come>"},
            {0x31, "<You>"},
            {0x32, "<Coliseum>"},
            {0x33, "<about>"},
            {0x34, "<don't>"},
            {0x35, "<anything>"},
            {0x37, "<Parts>"},
            {0x38, "<where>"},
            {0x39, "<The>"},
            {0x3A, "<know>"},
            {0x3B, "<Leomon>"},
            {0x3C, "<want>"},
            {0x3D, "<Oldman>"},
            {0x3E, "<like>"},
            {0x3F, "<need>"},
            {0x40, "<Chief>"},
            {0x41, "<with>"},
            {0x42, "<Thank>"},
            {0x43, "<strange>" },
            {0x44, "<Island>"},
            {0x45, "<can>"},
            {0x46, "<really>"},
            {0x47, "<Blue>"},
            {0x48, "<time>"},
        };

        /// <summary>
        /// The zone is always prefixed by 0xF3
        /// </summary>
        private static readonly Dictionary<byte, string> ZoneLookUpTable = new Dictionary<byte, string>()
        {
            {0x01, "[Main Gate Entrance]"},
            {0x02, "[Main Gate DigiBeetle Room]"},
            {0x03, "[Gold Hawk Entrance]"},
            {0x04, "[Gold Hawk Leader Room]"},
            {0x05, "[Gold Hawk Digivolve Room]"},
            {0x06, "[Gold Hawk Tamer Room]"},
            {0x07, "[Blue Falcon Entrance]"},
            {0x08, "[Blue Falcon Leader Room]"},
            {0x09, "[Blue Falcon Digivolve Room]"},
            {0x0A, "[Blue Falcon Tamer Room]"},
            {0x0B, "[Black Sword Entrance]"},
            {0x0C, "[Black Sword Leader Room]"},
            {0x0D, "[Black Sword Digivolve Room]"},
            {0x0E, "[Black Sword Tamer Room]"},
            {0x0F, "[Tamers Club]"},
            {0x10, "[Digimon Center]"},
            {0x11, "[Coliseum Entrance]"},
            {0x12, "[Coliseum Lobby]"},
            {0x13, "[Device Dome Entrance]"},
            {0x14, "[Device Dome Main Hall]"},
            {0x15, "[Device Dome Digivolve Room]"},
            {0x16, "[Device Dome DigiBeetle Room]"},
            {0x17, "[Meditation Dome Entrance]"},
            {0x18, "[Meditation Dome Angemon Area]"},
            {0x19, "[Meditation Dome Left Area]"},
            {0x1A, "[Meditation Dome Right Area]"},
            {0x1B, "[Archive Ship Shutdown Teleport Entrance]"},
            {0x1C, "[Archive Ship Shutdown Teleport Room]"},
            {0x1D, "[Archive Port Teleport Room]"},
            {0x1E, "[Shuttle Port Shutdown Teleport Entrance]"},
            {0x1F, "[Shuttle Point Shutdown Teleport Room]"},
            {0x20, "[Shuttle Port Teleport Entrance]"},
            {0x21, "[Master Gate]"},
            {0x22, "[DigiBeetle Factory]"},
            {0x23, "[File City Item Shop]"},
            {0x24, "[Jijimon House]"},
            {0x25, "[Archive Ship Teleport Entrance]"},
            {0x26, "[Archive Ship Teleport Room]"},
            {0x27, "[Shuttle Point Teleport Room]"},
            {0x28, "[Shuttle Port Unused Teleport Entrance]"},
            {0x29, "[Debug NPC]"},
            {0x2A, "[Directory Continent]"},
            {0x2B, "[File Island]"},
            {0x2C, "[Kernel Zone]"},
            {0x2D, "[Digital City]"},
            {0x2E, "[File City]"},
            {0x2F, "[Menu DNA Digivolve]"},
            {0x30, "[Menu Item Shop]"},
            {0x31, "[Menu Ammo Shop]"},
            {0x33, "[Menu DigiBeetle Upgrade]"},
            {0xFC, "[Menu PocketStation]"},
            {0xFD, "[Trigger Battle]"},
            {0xFE, "[Vídeo FileIsland]"},
            {0xFF, "[Vídeo KernelZone]"},
        };

        /// <summary>
        /// These colour changes are always prefixed by 0xF4
        /// </summary>
        private static readonly Dictionary<byte, string> SpecialEventsLookUpTable = new Dictionary<byte, string>()
        {
            {0x00, "[Stop anim]"},
            {0x01, "[Walk anim]"},
            {0x02, "[Conversation anim]"},
            {0x03, "[Paralyzed anim]"},
            {0x04, "[Greetings anim]"},
            {0x05, "[Agree anim]"},
            {0x06, "[Disagree anim]"},
            {0x07, "[Running anim]"},
            {0x08, "[Scared anim]"},
            {0x10, "[Walk down talk anim]"},
            {0x11, "[Walk left talk anim]"},
            {0x12, "[Walk up talk anim]"},
            {0x13, "[Walk right talk anim]"},

            {0x20, "[Rename Digimon]" },
            {0x21, "[Rename Character]" },
            {0x22, "[Rename Digi-Beetle]" },

            {0x30, "[Text White]" },
            {0x31, "[Text Grey]" },
            {0x32, "[Text Red]" },
            {0x33, "[Text Deep Red]" },
            {0x34, "[Text Yellow]" },
            {0x35, "[Text Pink]" },

            {0x40, "[Sound item]" },
            {0x41, "[Sound key item]" },
            {0x42, "[Sound Elevator]" },
            {0x43, "[Sound Level up]" },
            {0x44, "[Sound Earthquake]" },
            {0x45, "[Sound Archive Ship]" },
            {0x46, "[Sound auto pilot]" },
            {0x47, "[Sound unknown]" },
            {0x48, "[Sound Bits]" },
        };

        /// <summary>
        /// Prefixed by F6
        /// </summary>
        private static readonly Dictionary<byte, string> TartgetModelEvent = new Dictionary<byte, string>()
        {
            {0x03, "[Target Model ID]" }
        };

        /// <summary>
        /// Prefixed by 0xF8
        /// </summary>
        private static readonly Dictionary<byte, string> OptionsLookupTable = new Dictionary<byte, string>()
        {
            {0x00, "[End options]" },
            {0x01, "[Option 1]" },
            {0x02, "[Option 2]" },
        };

        /// <summary>
        /// These portraits are always prefixed by 0xF9
        /// </summary>
        private static readonly Dictionary<byte, string> PortraitLookupTable = new Dictionary<byte, string>()
        {
            {0x00, "[Portrait right]" },
            {0x01, "[Get sprite]" },
            {0x02, "[Portrait Left]" },
            {0x03, "[Sound window closing]" },
        };


        /// <summary>
        /// Prefixed by 0xFA
        /// </summary>
        private static readonly Dictionary<byte, string> TextBoxEvent = new Dictionary<byte, string>()
        {
            {0x00, "[Open text]" },
            {0x01, "[Close text]\n" },
        };

        public static string DigiStringToASCII(byte[] input)
        {
            if (input == null)
                return "No input data given";
            string converted = "";
            foreach (var item in input)
                converted += GetReplacementChar(item, CharacterLookupTable, "");
            return converted;
        }

        /// <summary>
        /// Convert the hex value of DW2 text and events found in the MESS files to their ASCII representation 
        /// </summary>
        /// <param name="input">The bytes that need to be converted</param>
        /// <returns>Input bytes converted to ASCII text</returns>
        public static string MessageFileToString(byte[] input)
        {
            string converted = "";
            bool skipByte = false;

            converted += $"[{input[0]:X2} {input[1]:X2} {input[2]:X2} {input[3]:X2}]\n";
            converted += $"[{input[4]:X2} {input[5]:X2} {input[6]:X2} {input[7]:X2}]\n";
            converted += $"[{input[8]:X2} {input[9]:X2} {input[10]:X2} {input[11]:X2}]\n";

            for (int i = 12; i < input.Length; i++)
            {
                if (skipByte)
                {
                    skipByte = false;
                    continue;
                }

                switch (input[i])
                {
                    case 0xF0:
                        converted += GetReplacementChar(input[i + 1], CommonWordsLookUpTable, "F0");
                        skipByte = true;
                        continue;

                    case 0xF3:
                        converted += GetReplacementChar(input[i + 1], ZoneLookUpTable, "F3");
                        skipByte = true;
                        continue;

                    case 0xF4:
                        converted += GetReplacementChar(input[i + 1], SpecialEventsLookUpTable, "F4");
                        skipByte = true;
                        continue;

                    case 0xF6:
                        converted += GetReplacementChar(input[i + 1], TartgetModelEvent, "F6");
                        //The next 2 bytes indicate the model to target, so we skip these in the next loop and print their hex values
                        converted += $"{input[i + 2]:X2}";
                        converted += $"{input[i + 3]:X2}";

                        i++;
                        i++;
                        converted += " ";

                        skipByte = true;
                        continue;

                    case 0xF8:
                        converted += GetReplacementChar(input[i + 1], OptionsLookupTable, "F8");
                        skipByte = true;
                        continue;

                    case 0xF9:
                        converted += GetReplacementChar(input[i + 1], PortraitLookupTable, "F9");
                        skipByte = true;
                        continue;

                    case 0xFA:
                        converted += GetReplacementChar(input[i + 1], TextBoxEvent, "FA");
                        skipByte = true;
                        continue;


                    default:
                        if (CharacterLookupTable.ContainsKey(input[i]))
                            converted += $"{CharacterLookupTable[input[i]]}";
                        else
                            converted += $"[{input[i]:X2}]";
                        break;
                }
            }

            return converted;
        }

        private static string GetReplacementChar(byte index, Dictionary<byte, string> lookupDict, string modifier)
        {
            if (lookupDict.ContainsKey(index))
                return lookupDict[index];
            else
                return $"[{modifier} Unknown]";
        }

        public static string ByteArrayToHexString(byte[] data, char seperator = ' ')
        {
            string result = "";
            foreach (var item in data)
            {
                result += $"{item:X2}{seperator}";
            }
            return result;
        }
    }
}
