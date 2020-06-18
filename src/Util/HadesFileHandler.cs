using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;
using BetterHades.Frontend;

namespace BetterHades.Util
{
    public static class HadesFileHandler
    {
        public static void ExportToHades(GridCanvas canvas)
        {
            var name = "Test";
            using var file = new StreamWriter($"{name}.hds");

            file.WriteLine("# hades.models.Design file");
            file.WriteLine("#  ");
            file.WriteLine($"[name] {name}");
            file.WriteLine("[components]");
            foreach (var component in canvas.Components) file.WriteLine(ToHadesString(component));
            file.WriteLine("[end components]");
            file.WriteLine("[signals]");
            file.WriteLine("[end signals]");
            file.WriteLine("[end]");


            foreach (var connection in canvas.Connections)
                file.WriteLine(
                    $"{connection.GetType()}; {canvas.Components.IndexOf(connection.Input)}; {canvas.Components.IndexOf(connection.Output)}");
        }

        private static string ToHadesString(Component component)
        {
            return component switch
                   {
                       ANDGate and => $"hades.models.gatter.And_NUMBER_ {and.X} {and.Y} @N 1001 1.0E-8",
                       INVGate inv => $"hades.models.gatter.Inv _NAME_ {inv.X} {inv.Y} @N 1001 5.0E-9",
                       NANDGate nand => $"hades.models.gatter.Ipin",
                       NORGate nor => $"hades.models.gatter.Or _NUMBER_OF_INPUTS ",
                       ORGate or => $"hades.models.gatter.Or_NUMBER_ _NAME_ {or.X} {or.Y} @N 1001 1.0E-8",
                       XNORGate xnor => $"hades.models.gatter.Ipin",
                       XORGate xor => $"hades.models.gatter.Ipin",
                       // InputClock x => $"hades.models.io.PulseSwitch _NAME_ {component.X} {component.Y} @N 1001 _PULSE_DURATION_ null",
                       InputClock ic => $"hades.models.io.ClockGen _NAME_ {ic.X} {ic.Y} @N 1001 {ic.MsToSec()} 0.5 0.0",
                       Input i => $"hades.models.io.Ipin _NAME_ {i.X} {i.Y} @N 1001  {i.IsActive}",
                       Output o => $"hades.models.io.Opin _NAME_ {o.X} {o.Y} @N 1001 5.0E-9",
                       // Connection x => "hades.signals.SignalStdLogic1164",
                       _ => throw new ComponentNotFoundException(component.GetType().ToString())
                   };
        }
    }
}