﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Exceptions;

namespace BetterHades.Util
{
    public static class FileHandler
    {
        private const string Title = "BetterHades - ";
        private const string Unnamed = "Unnamed.bhds";
        private static bool _hasChanged = true;
        private static FileInfo _currentFile;

        public static string CurrentFile
        {
            get => _currentFile.Name.Replace(_currentFile.Extension, "");
            set
            {
                _currentFile = new FileInfo(value);
                Environment.CurrentDirectory = _currentFile.DirectoryName!;
            }
        }

        // File Handling:
        public static void New()
        {
            CurrentFile = Unnamed;
            Changed();
        }

        public static void Save(string fileName)
        {
            CurrentFile = fileName;
            Save();
        }

        public static void Save()
        {
            using var file = new StreamWriter(_currentFile.FullName);
            foreach (var c in App.MainWindow.GridCanvas.Components)
                file.WriteLine($"{c.GetType()}; {c.X}; {c.Y}; {c.IsActive}");
            file.WriteLine("--------------------------------------");
            foreach (var c in App.MainWindow.GridCanvas.Connections)
                file.WriteLine(
                    $"{c.GetType()}; {App.MainWindow.GridCanvas.Components.IndexOf(c.Input)}; {App.MainWindow.GridCanvas.Components.IndexOf(c.Output)}");
            Changed(false);
        }

        public static void Load(string fileName)
        {
            CurrentFile = fileName;
            var lines = File.ReadAllLines(_currentFile.FullName);
            LoadComponents(lines.TakeWhile(l => !l.Contains("---------")));
            Dispatcher.UIThread.InvokeAsync
            (
                () => LoadConnections(lines.SkipWhile(l => !l.Contains("---------")).Skip(1)),
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
                        App.MainWindow.GridCanvas.Canvas
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
                App.MainWindow.GridCanvas.Components.Add
                (
                    (Component) Activator.CreateInstance
                    (
                        t, App.MainWindow.GridCanvas,
                        double.Parse(vars[1]),
                        double.Parse(vars[2]),
                        bool.Parse(vars[3])
                    ) ?? throw new ComponentNotFoundException(vars[0])
                );
            }
        }

        // Helper Methods:
        public static void Changed(bool changed = true)
        {
            _hasChanged = changed;
            UpdateTitle();
        }

        private static void UpdateTitle() { App.MainWindow.Title = $"{Title}{CurrentFile}{(_hasChanged ? "*" : "")}"; }
    }
}