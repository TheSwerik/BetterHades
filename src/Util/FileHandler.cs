using System;
using System.IO;
using System.Threading;
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

            foreach (var component in canvas.Components)
                file.WriteLine($"{component.GetType()}; {component.X}; {component.Y}; {component}");
            file.WriteLine("--------------------------------------");
            foreach (var connection in canvas.Connections)
                file.WriteLine(
                    $"{connection.GetType()}; {canvas.Components.IndexOf(connection.Input)}; {canvas.Components.IndexOf(connection.Output)}");
        }

        public static void Load(GridCanvas canvas)
        {
            var lines = File.ReadAllLines("Test.txt");
            var max = lines.Length;
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("---------"))
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                                                    {
                                                        for (var j = i + 1; j < lines.Length; j++)
                                                        {
                                                            var vars = lines[j].Split(";");
                                                            var t = Type.GetType(vars[0]);
                                                            if (t == null)
                                                                throw new ComponentNotFoundException(vars[0]);
                                                            canvas.Connections.Add(
                                                                (Connection) Activator.CreateInstance(
                                                                    t, canvas.Components[int.Parse(vars[1])],
                                                                    (ObservingComponent) canvas.Components[
                                                                        int.Parse(vars[2])], canvas.Canvas) ??
                                                                throw new ComponentNotFoundException(vars[0]));
                                                        }
                                                    }, DispatcherPriority.Render);
                    break;
                }

                var vars = lines[i].Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                var component = (Component) Activator.CreateInstance(t, canvas,
                                                                     double.Parse(vars[1]),
                                                                     double.Parse(vars[2]),
                                                                     bool.Parse(vars[3])) ??
                                throw new ComponentNotFoundException(vars[0]);
                canvas.Components.Add(component);
            }
        }
    }
}