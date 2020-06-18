using System.IO;
using BetterHades.Frontend;

namespace BetterHades.Util
{
    public static class FileHandler
    {
        public static void Save(GridCanvas canvas)
        {
            using var file = new StreamWriter("Test.txt");

            foreach (var component in canvas.Components) file.WriteLine($"{component.GetType()} {component}");
            foreach (var connection in canvas.Connections) file.WriteLine($"{connection.GetType()} {connection}");
        }
    }
}