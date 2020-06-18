using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Exceptions;
using BetterHades.Frontend;

namespace BetterHades.Util
{
    public static class FileHandler
    {
        public static void Save(GridCanvas canvas)
        {
            using var file = new StreamWriter("Test.txt");

            foreach (var c in canvas.Components) file.WriteLine($"{c.GetType()}; {c.X}; {c.Y}; {c.IsActive}");
            file.WriteLine("--------------------------------------");
            foreach (var c in canvas.Connections)
                file.WriteLine(
                    $"{c.GetType()}; {canvas.Components.IndexOf(c.Input)}; {canvas.Components.IndexOf(c.Output)}");
        }

        public static void Load(GridCanvas canvas)
        {
            var lines = File.ReadAllLines("Test.txt");
            var max = lines.Length;
            LoadComponents(canvas, lines.TakeWhile(l => !l.Contains("---------")));
            Dispatcher.UIThread.InvokeAsync
            (
                () => LoadConnections(canvas, lines.SkipWhile(l => !l.Contains("---------")).Skip(1)),
                DispatcherPriority.Render
            );
        }

        private static void LoadConnections(GridCanvas canvas, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var vars = line.Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                canvas.Connections.Add
                (
                    (Connection) Activator.CreateInstance(
                        t,
                        canvas.Components[int.Parse(vars[1])],
                        (ObservingComponent) canvas.Components[int.Parse(vars[2])],
                        canvas.Canvas
                    ) ?? throw new ComponentNotFoundException(vars[0])
                );
            }
        }

        private static void LoadComponents(GridCanvas canvas, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var vars = line.Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                canvas.Components.Add
                (
                    (Component) Activator.CreateInstance
                    (
                        t, canvas,
                        double.Parse(vars[1]),
                        double.Parse(vars[2]),
                        bool.Parse(vars[3])
                    ) ?? throw new ComponentNotFoundException(vars[0])
                );
            }
        }
    }
}