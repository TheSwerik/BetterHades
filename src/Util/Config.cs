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
        private static Queue<string> _fileHistory;
        private static readonly string[] Headers = {"Better Hades Config File\n", "[FileHistory]\n"};
        public static IEnumerable<string> FileHistory => _fileHistory.Reverse();

        public static void Init()
        {
            _fileHistory = new Queue<string>();
            _fullPath = FileHandler.CurrentDirectory.Parent + "\\" + FileName;
            if (!File.Exists(_fullPath)) File.WriteAllLines(_fullPath, Headers);
            foreach (var file in ReadProperty("FileHistory").Reverse())
                if (File.Exists(file))
                    _fileHistory.Enqueue(file);
            Save();
        }

        public static void AddFileToHistory(string file)
        {
            if (_fileHistory.Contains(file))
            {
                var list = _fileHistory.ToList();
                list[list.IndexOf(file)] = list[^1];
                list[^1] = file;
                _fileHistory.Clear();
                foreach (var f in list) _fileHistory.Enqueue(f);
            }
            else
            {
                if (_fileHistory.Count >= 5) _fileHistory.Dequeue();
                _fileHistory.Enqueue(file);
            }

            App.MainWindow.UpdateFileHistory();
        }

        public static void Save()
        {
            File.WriteAllLines(_fullPath, WriteProperty("FileHistory", FileHistory.ToArray()));
        }

        private static IEnumerable<string> ReadProperty(string property)
        {
            if (!property.Contains("[")) property = "[" + property + "]";
            var lines = File.ReadLines(_fullPath);
            return lines.SkipWhile(l => !l.Contains(property, StringComparison.InvariantCultureIgnoreCase))
                        .Skip(1)
                        .TakeWhile(l => !Regex.IsMatch(l, "\\[.*\\]"));
        }

        private static IEnumerable<string> WriteProperty(string property, IReadOnlyList<string> writeLines)
        {
            if (!property.Contains("[")) property = "[" + property + "]";
            var lines = File.ReadLines(_fullPath).ToList();
            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].Contains(property)) continue;
                i++;
                while (i < lines.Count && !Regex.IsMatch(lines[i], @"\[.*\]")) lines.RemoveAt(i);
                lines.InsertRange(i, writeLines);
                break;
            }

            return lines;
        }
    }
}