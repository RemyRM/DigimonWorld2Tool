using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.Utility
{
    class CsvParser
    {
        public static List<string[]> Parse(string fileName)
        {
            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.SetDelimiters(",");

                List<string[]> parsedData = new List<string[]>();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    parsedData.Add(fields);
                }
                return parsedData;
            }
        }
    }
}
