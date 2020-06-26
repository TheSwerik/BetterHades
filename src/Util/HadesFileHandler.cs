﻿using System;
using System.IO;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;

namespace BetterHades.Util
{
    public static class HadesFileHandler
    {
        private static int _n = 1;

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
                       ANDGate and =>
                       $"hades.models.gatter.And{and.NumberOfInputs} i{_n++} {and.Pos.X} {and.Pos.Y} @N 1001 1.0E-8",
                       INVGate inv => $"hades.models.gatter.Inv i{_n++} {inv.Pos.X} {inv.Pos.Y} @N 1001 5.0E-9",
                       NANDGate nand =>
                       $"hades.models.gatter.Nand{nand.NumberOfInputs} i{_n++} {nand.Pos.X} {nand.Pos.Y} @N 1001 1.0E-8",
                       NORGate nor =>
                       $"hades.models.gatter.Nor{nor.NumberOfInputs} i{_n++} {nor.Pos.X} {nor.Pos.Y} @N 1001 1.0E-8",
                       ORGate or =>
                       $"hades.models.gatter.Or{or.NumberOfInputs} i{_n++} {or.Pos.X} {or.Pos.Y} @N 1001 1.0E-8",
                       XNORGate xnor =>
                       $"hades.models.gatter.Xnor{xnor.NumberOfInputs} i{_n++} {xnor.Pos.X} {xnor.Pos.Y} @N 1001 1.0E-8",
                       XORGate xor =>
                       $"hades.models.gatter.Xor{xor.NumberOfInputs} i{_n++} {xor.Pos.X} {xor.Pos.Y} @N 1001 1.0E-8",
                       InputPulse ip =>
                       $"hades.models.io.PulseSwitch {ip.Name} {ip.Pos.X} {ip.Pos.Y} @N 1001 0.1 null",
                       InputClock ic =>
                       $"hades.models.io.ClockGen {ic.Name} {ic.Pos.X} {ic.Pos.Y} @N 1001 {InputClock.MsToSec()} 0.5 0.0",
                       Input i => $"hades.models.io.Ipin {i.Name} {i.Pos.X} {i.Pos.Y} @N 1001  {i.IsActive}",
                       Output o => $"hades.models.io.Opin {o.Name} {o.Pos.X} {o.Pos.Y} @N 1001 5.0E-9",
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