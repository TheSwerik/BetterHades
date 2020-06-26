using System;
using System.IO;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;

namespace BetterHades.Util
{
    public static class HadesFileHandler
    {
        // File Handling:
        public static void ExportToHades()
        {
            using var file = new StreamWriter(
                FileHandler.FullPath.Replace(
                    ".bhds",
                    ".hds",
                    StringComparison.InvariantCultureIgnoreCase
                ));

            file.WriteLine("# hades.models.Design file");
            file.WriteLine("#  ");
            file.WriteLine($"[name] {FileHandler.CurrentFile}");
            file.WriteLine("[components]");
            foreach (var component in App.MainWindow.GridCanvas.Components) file.WriteLine(component.ToHadesString());
            file.WriteLine("[end components]");
            file.WriteLine("[signals]");
            foreach (var con in App.MainWindow.GridCanvas.Connections) file.WriteLine(con.ToHadesString());
            file.WriteLine("[end signals]");
            file.WriteLine("[end]");
        }

        // Helper Methods:
        private static string ToHadesString(this Component component)
        {
            return component switch
                   {
                       ANDGate and => $"hades.models.gatter.And_NUMBER_ {and.Pos.X} {and.Pos.Y} @N 1001 1.0E-8",
                       INVGate inv => $"hades.models.gatter.Inv _NAME_ {inv.Pos.X} {inv.Pos.Y} @N 1001 5.0E-9",
                       NANDGate nand => $"hades.models.gatter.Nand_NUMBER_ {nand.Pos.X} {nand.Pos.Y} @N 1001 1.0E-8",
                       NORGate nor => $"hades.models.gatter.Nor_NUMBER_ _NAME_ {nor.Pos.X} {nor.Pos.Y} @N 1001 1.0E-8",
                       ORGate or => $"hades.models.gatter.Or_NUMBER_ _NAME_ {or.Pos.X} {or.Pos.Y} @N 1001 1.0E-8",
                       XNORGate xnor =>
                       $"hades.models.gatter.Xnor_NUMBER_ _NAME_ {xnor.Pos.X} {xnor.Pos.Y} @N 1001 1.0E-8",
                       XORGate xor => $"hades.models.gatter.Xor_NUMBER_ _NAME_ {xor.Pos.X} {xor.Pos.Y} @N 1001 1.0E-8",
                       // InputPulse ip => $"hades.models.io.PulseSwitch _NAME_ {ip.Pos.X} {ip.Pos.Y} @N 1001 _PULSE_DURATION_ null",
                       InputClock ic =>
                       $"hades.models.io.ClockGen _NAME_ {ic.Pos.X} {ic.Pos.Y} @N 1001 {InputClock.MsToSec()} 0.5 0.0",
                       Input i => $"hades.models.io.Ipin _NAME_ {i.Pos.X} {i.Pos.Y} @N 1001  {i.IsActive}",
                       Output o => $"hades.models.io.Opin _NAME_ {o.Pos.X} {o.Pos.Y} @N 1001 5.0E-9",
                       _ => throw new ComponentNotFoundException(component.GetType().ToString())
                   };
        }

        private static string ToHadesString(this Connection connection)
        {
            //TODO WHY IS IT LIKE THIS I HATE MY LIFE
            return $"hades.signals.SignalStdLogic1164 _NAME_ {connection}";
        }
    }
}