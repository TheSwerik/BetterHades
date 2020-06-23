using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetterHades.Util
{
    public static class Config
    {
        private const string FileName = "config.cfg";
        private static string _fullPath;
        public static List<string> FileHistory;

        private static readonly string[] Headers = {"Better Hades Config File\n", "[FileHistory]\n"};

        public static void Init()
        {
            FileHistory = new List<string>();
            _fullPath = FileHandler.CurrentDirectory.Parent + "\\" + FileName;
            if (!File.Exists(_fullPath)) File.WriteAllLines(_fullPath, Headers);
            FileHistory.AddRange(ReadProperty("FileHistory"));
            Console.WriteLine(string.Join("\n", FileHistory));
        }

        private static IEnumerable<string> ReadProperty(string property)
        {
            if (!property.Contains("[")) property = "[" + property + "]";
            var lines = File.ReadLines(_fullPath);
            return lines.SkipWhile(l => !l.Contains(property, StringComparison.InvariantCultureIgnoreCase))
                        .Skip(1)
                        .TakeWhile(l => !Regex.IsMatch(l, "\\[.*\\]"));
        }
    }
}