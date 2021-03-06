﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;

namespace BetterHades.Util
{
    public static class FileHandler
    {
        private const string Title = "BetterHades - ";
        private const string Unnamed = "Unnamed.bhds";
        public static bool HasChanged;
        private static FileInfo _currentFile = new FileInfo(Unnamed);
        public static string FullPath => _currentFile.FullName;
        public static DirectoryInfo CurrentDirectory => _currentFile.Directory;

        public static string CurrentFile
        {
            get => _currentFile.Name.Replace(_currentFile.Extension, "");
            private set
            {
                _currentFile = new FileInfo(value);
                Environment.CurrentDirectory = _currentFile.DirectoryName!;
            }
        }

        // File Handling:
        public static void New()
        {
            CurrentFile = Unnamed;
            Input.Counter = Output.Counter = 1;
            Changed();
        }

        public static void Save(string fileName)
        {
            CurrentFile = fileName;
            Save();
        }

        public static void Save()
        {
            var components = App.MainWindow.GridCanvas.Components;
            using var file = new StreamWriter(_currentFile.FullName);
            foreach (var component in components)
            {
                file.Write($"{component.GetType()}; {component.Pos.X}; {component.Pos.Y}; {component.IsActive}");
                if (component is Input i) file.Write($"; {i.Name}");
                if (component is Output o) file.Write($"; {o.Name}");
                file.WriteLine("");
            }

            file.WriteLine(new string('-', 100));
            foreach (var c in App.MainWindow.GridCanvas.Connections)
            {
                var pts = string.Join("; ", c.Points);
                file.WriteLine($"{c.GetType()}; {components.IndexOf(c.Input)}; {components.IndexOf(c.Output)}; {pts}");
            }

            Changed(false);
        }

        public static void Load(string fileName)
        {
            CurrentFile = fileName;
            var lines = File.ReadAllLines(_currentFile.FullName);
            App.MainWindow.New(null, null, true);
            CurrentFile = fileName;
            LoadComponents(lines.TakeWhile(l => !l.Contains("----------")));
            Dispatcher.UIThread.InvokeAsync
            (
                () => LoadConnections(lines.SkipWhile(l => !l.Contains("----------")).Skip(1)),
                DispatcherPriority.Render
            );
            Changed(false);
        }

        private static void LoadConnections(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var vars = line.Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                App.MainWindow.GridCanvas.Connections.Add
                (
                    (Connection) Activator.CreateInstance(
                        t,
                        App.MainWindow.GridCanvas.Components[int.Parse(vars[1])],
                        (ObservingComponent) App.MainWindow.GridCanvas.Components[int.Parse(vars[2])],
                        new Polyline {Points = vars.Skip(3).Select(Point.Parse).ToList()}
                    ) ?? throw new ComponentNotFoundException(vars[0])
                );
            }

            Changed(false);
        }

        private static void LoadComponents(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var vars = line.Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                var args = new List<object>
                           {new Point(double.Parse(vars[1]), double.Parse(vars[2])), bool.Parse(vars[3])};
                if (vars.Length >= 5) args.Add(vars[4].Trim());
                App.MainWindow.GridCanvas.Components.Add(
                    (Component) Activator.CreateInstance(t, args.ToArray()) ??
                    throw new ComponentNotFoundException(vars[0]));
            }
        }

        // Helper Methods:
        public static void Changed(bool changed = true)
        {
            HasChanged = changed;
            UpdateTitle();
        }

        private static void UpdateTitle() { App.MainWindow.Title = $"{Title}{CurrentFile}{(HasChanged ? "*" : "")}"; }
    }
}