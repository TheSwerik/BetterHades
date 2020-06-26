using System;
using JetBrains.Annotations;

namespace BetterHades.Exceptions
{
    public class OpeningBehaviourParseException : Exception
    {
        public OpeningBehaviourParseException([CanBeNull] string message) : base($"Cant Parse {message}.") { }
    }
}