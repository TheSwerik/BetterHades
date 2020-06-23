using System;
using JetBrains.Annotations;

namespace BetterHades.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException([CanBeNull] string message) : base(message) { }
    }
}