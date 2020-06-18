﻿using System;
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
            foreach (var component in canvas.Components) file.WriteLine(component.ToHadesString());
            file.WriteLine("[end components]");
            file.WriteLine("[signals]");
            foreach (var connection in canvas.Connections) file.WriteLine(connection.ToHadesString());
            file.WriteLine("[end signals]");
            file.WriteLine("[end]");
        }

        private static string ToHadesString(this Component component)
        {
            return component switch
                   {
                       ANDGate and => $"hades.models.gatter.And_NUMBER_ {and.X} {and.Y} @N 1001 1.0E-8",
                       INVGate inv => $"hades.models.gatter.Inv _NAME_ {inv.X} {inv.Y} @N 1001 5.0E-9",
                       NANDGate nand => $"hades.models.gatter.Nand_NUMBER_ {nand.X} {nand.Y} @N 1001 1.0E-8",
                       NORGate nor => $"hades.models.gatter.Nor_NUMBER_ _NAME_ {nor.X} {nor.Y} @N 1001 1.0E-8",
                       ORGate or => $"hades.models.gatter.Or_NUMBER_ _NAME_ {or.X} {or.Y} @N 1001 1.0E-8",
                       XNORGate xnor => $"hades.models.gatter.Xnor_NUMBER_ _NAME_ {xnor.X} {xnor.Y} @N 1001 1.0E-8",
                       XORGate xor => $"hades.models.gatter.Xor_NUMBER_ _NAME_ {xor.X} {xor.Y} @N 1001 1.0E-8",
                       // InputPulse ip => $"hades.models.io.PulseSwitch _NAME_ {ip.X} {ip.Y} @N 1001 _PULSE_DURATION_ null",
                       InputClock ic => $"hades.models.io.ClockGen _NAME_ {ic.X} {ic.Y} @N 1001 {ic.MsToSec()} 0.5 0.0",
                       Input i => $"hades.models.io.Ipin _NAME_ {i.X} {i.Y} @N 1001  {i.IsActive}",
                       Output o => $"hades.models.io.Opin _NAME_ {o.X} {o.Y} @N 1001 5.0E-9",
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