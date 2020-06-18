using System;
using System.IO;
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
            var components = true;
            foreach (var line in File.ReadLines("Test.txt"))
            {
                if (line.Contains("--------"))
                {
                    components = false;
                    continue;
                }

                var vars = line.Split(";");
                var t = Type.GetType(vars[0]);
                if (t == null) throw new ComponentNotFoundException(vars[0]);
                if (components)
                {
                    // canvas.Components.Add((Component) Activator.CreateInstance(t, canvas, vars[1], vars[2]) ??
                    // throw new ComponentNotFoundException(vars[0]));
                    var component =
                        (Component) Activator.CreateInstance(t, canvas, double.Parse(vars[1]), double.Parse(vars[2])) ??
                        throw new ComponentNotFoundException(vars[0]);
                    component.IsActive = bool.Parse(vars[3]);
                    canvas.Components.Add(component);
                }
                else
                {
                    canvas.Connections.Add(
                        (Connection) Activator.CreateInstance(t, canvas.Components[int.Parse(vars[1])],
                                                              (ObservingComponent) canvas.Components[
                                                                  int.Parse(vars[2])], canvas.Canvas) ??
                        throw new ComponentNotFoundException(vars[0]));
                }
            }

            Console.WriteLine(string.Join("\n", canvas.Connections));
        }
    }
}