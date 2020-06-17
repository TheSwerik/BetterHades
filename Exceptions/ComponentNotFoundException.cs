using System;
using BetterHades.Components;
using JetBrains.Annotations;

namespace BetterHades.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(Component.Type componentType) : base(componentType + "") { }
    }
}