using System;
using System.IO;
using System.Linq;
using Avalonia;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;

namespace BetterHades.Util
{
    public static class HadesFileHandler
    {
        private static int _n;

        // File Handling:
        public static void ExportToHades(string fileName = null)
        {
            if (fileName == null) fileName = FileHandler.FullPath;
            using var file = new StreamWriter(
                fileName.Replace(
                    ".bhds",
                    ".hds",
                    StringComparison.InvariantCultureIgnoreCase
                ));
            file.WriteLine("# hades.models.Design file");
            file.WriteLine("#  ");
            file.WriteLine(
                $"[name] {new string(fileName.Skip(fileName.LastIndexOf('\\') + 1).ToArray()).TrimEnd(".bhds".ToCharArray())}");
            file.WriteLine("[components]");
            foreach (var component in App.MainWindow.GridCanvas.Components) file.WriteLine(component.ToHadesString());
            file.WriteLine("[end components]");
            file.WriteLine("[signals]");
            foreach (var component in App.MainWindow.GridCanvas.Components)
                file.WriteLine(component.ToConnectionHadesString());
            file.WriteLine("[end signals]");
            file.WriteLine("[end]");
        }

        // Helper Methods:
        private static string ToHadesString(this Component component)
        {
            return component switch
                   {
                       ANDGate and =>
                       $"hades.models.gatter.And{and.NumberOfInputs} {and.Name} {and.Pos.X * 3}0 {and.Pos.Y * 3}0 @N 1001 1.0E-8",
                       INVGate inv =>
                       $"hades.models.gatter.Inv {inv.Name} {inv.Pos.X * 3}0 {inv.Pos.Y * 3}0 @N 1001 5.0E-9",
                       NANDGate nand =>
                       $"hades.models.gatter.Nand{nand.NumberOfInputs} {nand.Name} {nand.Pos.X * 3}0 {nand.Pos.Y * 3}0 @N 1001 1.0E-8",
                       NORGate nor =>
                       $"hades.models.gatter.Nor{nor.NumberOfInputs} {nor.Name} {nor.Pos.X * 3}0 {nor.Pos.Y * 3}0 @N 1001 1.0E-8",
                       ORGate or =>
                       $"hades.models.gatter.Or{or.NumberOfInputs} {or.Name} {or.Pos.X * 3}0 {or.Pos.Y * 3}0 @N 1001 1.0E-8",
                       XNORGate xnor =>
                       $"hades.models.gatter.Xnor{xnor.NumberOfInputs} {xnor.Name} {xnor.Pos.X * 3}0 {xnor.Pos.Y * 3}0 @N 1001 1.0E-8",
                       XORGate xor =>
                       $"hades.models.gatter.Xor{xor.NumberOfInputs} {xor.Name} {xor.Pos.X * 3}0 {xor.Pos.Y * 3}0 @N 1001 1.0E-8",
                       InputPulse ip =>
                       $"hades.models.io.PulseSwitch {ip.Name} {ip.Pos.X * 3}0 {ip.Pos.Y * 3}0 @N 1001 0.1 null",
                       InputClock ic =>
                       $"hades.models.io.ClockGen {ic.Name} {ic.Pos.X * 3}0 {ic.Pos.Y * 3}0 @N 1001 {InputClock.MsToSec()} 0.5 0.0",
                       Input i => $"hades.models.io.Ipin {i.Name} {i.Pos.X * 3}0 {i.Pos.Y * 3}0 @N 1001  {i.IsActive}",
                       Output o => $"hades.models.io.Opin {o.Name} {o.Pos.X * 3}0 {o.Pos.Y * 3}0 @N 1001 5.0E-9",
                       _ => throw new ComponentNotFoundException(component.GetType().ToString())
                   };
        }

        private static string ToConnectionHadesString(this Component component)
        {
            if (!component.Outputs.Any()) return "\r";
            var result = $"hades.signals.SignalStdLogic1164 n{_n++} {component.Outputs.Count + 1} ";
            result += component.Name + " Y ";
            foreach (var output in component.Outputs)
                result = result + $"{output.Output.Name} {'A' + output.Output.Inputs.IndexOf(output)} ";
            var x = 0;
            foreach (var connection in component.Outputs)
            foreach (var point in connection.Points.Where(p => p != component.OutPoint))
                if (point != component.OutPoint)
                    x++;

            result += $"{1 + x} ";

            foreach (var connection in component.Outputs)
            {
                var connectionPoints = connection.Points as Point[] ?? connection.Points.ToArray();
                if (connectionPoints.First() != component.OutPoint)
                    connectionPoints = connectionPoints.Reverse().ToArray();
                for (var i = 1; i < connectionPoints.Length; i++)
                {
                    var p1 = connectionPoints[i - 1];
                    var p2 = connectionPoints[i];
                    result += $"2 {p1.X * 3}0 {p1.Y * 3}0 {p2.X * 3}0 {p2.Y * 3}0 ";
                }
            }

            return result;
        }
    }
}