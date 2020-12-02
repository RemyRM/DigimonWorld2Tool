using System;
using System.Collections.Generic;

namespace DigimonWorld2MapVisualizer.Utility
{
    public class TextConversion
    {
        private static readonly Dictionary<string, string> ConversionLookupTable = new Dictionary<string, string>()
        {
            {"00", "0"},
            {"01", "1"},
            {"02", "2"},
            {"03", "3"},
            {"04", "4"},
            {"05", "5"},
            {"06", "6"},
            {"07", "7"},
            {"08", "8"},
            {"09", "9"},
            {"0A", "A"},
            {"0B", "B"},
            {"0C", "C"},
            {"0D", "D"},
            {"0E", "E"},
            {"0F", "F"},
            {"10", "G"},
            {"11", "H"},
            {"12", "I"},
            {"13", "J"},
            {"14", "K"},
            {"15", "L"},
            {"16", "M"},
            {"17", "N"},
            {"18", "O"},
            {"19", "P"},
            {"1A", "Q"},
            {"1B", "R"},
            {"1C", "S"},
            {"1D", "T"},
            {"1E", "U"},
            {"1F", "V"},
            {"20", "W"},
            {"21", "X"},
            {"22", "Y"},
            {"23", "Z"},
            {"24", "a"},
            {"25", "b"},
            {"26", "c"},
            {"27", "d"},
            {"28", "e"},
            {"29", "f"},
            {"2A", "g"},
            {"2B", "h"},
            {"2C", "i"},
            {"2D", "j"},
            {"2E", "k"},
            {"2F", "l"},
            {"30", "m"},
            {"31", "n"},
            {"32", "o"},
            {"33", "p"},
            {"34", "q"},
            {"35", "r"},
            {"36", "s"},
            {"37", "t"},
            {"38", "u"},
            {"39", "v"},
            {"3A", "w"},
            {"3B", "x"},
            {"3C", "y"},
            {"3D", "z"},
            {"41", "<SQUARE>"},
            {"44", "?"},
            {"45", "!"},
            {"46", "/"},
            {"49", "-"},
            {"54", ","},
            {"55", "."},
            {"56", ""},
            {"5B", "PLUS SIGN"},
            {"FB", "<X>"},
            {"FC", "<NEW BOX>"},
            {"FD", " "},
            {"FE", "<ENTER>"},
            {"FF", "\n" },
            {"F000", "Akira"},
            {"F006", "Digimon"},
            {"F007", "you"},
            {"F008", "the"},
            {"F009", "Digi-Beetle"},
            {"F00A", "Domain"},
            {"F00B", "Guard"},
            {"F00C", "Tamer"},
            {"F00D", "here"},
            {"F00E", "have"},
            {"F00F", "Knights"},
            {"F010", "and"},
            {"F011", "thing"},
            {"F012", "Security"},
            {"F013", "that"},
            {"F014", "Bertran"},
            {"F015", "Tournament"},
            {"F016", "Crimson"},
            {"F018", "something"},
            {"F019", "Item"},
            {"F01A", "Falcon"},
            {"F01B", "for"},
            {"F01C", "That's"},
            {"F01D", "Commander"},
            {"F01E", "Blood"},
            {"F01F", "Leader"},
            {"F020", "Attendant"},
            {"F021", "Cecilia"},
            {"F022", "all"},
            {"F023", "mission"},
            {"F024", "this"},
            {"F026", "Archive"},
            {"F027", "Black"},
            {"F028", "I'll"},
            {"F029", "are"},
            {"F02A", "Sword"},
            {"F02B", "right"},
            {"F02C", "Digivolve"},
            {"F02D", "enter"},
            {"F02E", "What"},
            {"F02F", "will"},
            {"F030", "come"},
            {"F031", "You"},
            {"F032", "Coliseum"},
            {"F033", "about"},
            {"F034", "don't"},
            {"F035", "anything"},
            {"F037", "Parts"},
            {"F038", "where"},
            {"F039", "The"},
            {"F03A", "know"},
            {"F03B", "Leomon"},
            {"F03C", "want"},
            {"F03D", "Oldman"},
            {"F03E", "like"},
            {"F03F", "need"},
            {"F040", "Chief"},
            {"F041", "with"},
            {"F042", "Thank"},
            {"F044", "Island"},
            {"F045", "can"},
            {"F046", "really"},
            {"F047", "Blue"},
            {"F048", "time"},
        };

        /// <summary>
        /// Convert the hex value of DW2 text to the ASCII representation using the text map found here https://docs.google.com/spreadsheets/d/1UiDU4MsSfxO1vhpK6err1KsLRZM53JUOuYqYhfEFp8o/edit#gid=1279970913
        /// </summary>
        /// <param name="input">The bytes that need to be converted</param>
        /// <returns>Input bytes converted to ASCII text</returns>
        public static string DigiBytesToString(byte[] input)
        {
            string converted = "";
            foreach (var item in input)
            {
                if (ConversionLookupTable.ContainsKey(item.ToString("X2")))
                    converted += ConversionLookupTable[item.ToString("X2")];
                else
                    converted += item.ToString("X2");
            }
            return converted;
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
