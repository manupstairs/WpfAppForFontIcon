using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace IcoMoonReader
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().ReadSvgFile();
        }

        private void ReadSvgFile()
        {
            using (var stream = new FileStream("rcc-fonticon-ribbon-v2.svg", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    var pattern = "unicode(\\S)*\\sglyph-name(\\S)*\"";
                    var input = reader.ReadToEnd();
                    foreach (Match match in Regex.Matches(input, pattern))
                    {
                        pattern = "\"\\S*\"";
                        var list = new List<string>();
                        foreach (var result in Regex.Matches(match.Value, pattern))
                        {
                            list.Insert(0, result.ToString());
                        }
                        var name = list[0].Replace("\"", "").Replace("-","_");
                        var code = list[1].Replace("&#x", "\\u").Replace(";", "");
                        Console.WriteLine($"public static string {name} {{ get; }} = {code};");
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
