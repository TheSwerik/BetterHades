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
        private static bool _hasChanged = true;

        public static void Changed(bool changed = true)
        {
            _hasChanged = changed;
            UpdateTitle();
        }

        public static string CurrentFile { get; set; } = "Unnamed";

        private static void UpdateTitle() { App.MainWindow.Title = $"{Title}{CurrentFile}{(_hasChanged ? "*" : "")}"; }

        public static void Save(string fileName)
        {
            CurrentFile = fileName;
            Save();
        }

        public static void Save()
        {
            using var file = new StreamWriter($"{CurrentFile}.bhds");
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
            var lines = File.ReadAllLines($"{CurrentFile}.bhds");
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

        public static void New()
        {
            CurrentFile = "Unnamed";
            Changed();
        }
    }
}