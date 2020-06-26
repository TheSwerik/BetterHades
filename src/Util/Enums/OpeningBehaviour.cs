using BetterHades.Exceptions;

namespace BetterHades.Util.Enums
{
    public static class OpeningBehaviourMethods
    {
        public static OpeningBehaviour Parse(string value)
        {
            if (value.Equals(OpeningBehaviour.AlwaysOpen.ToString())) return OpeningBehaviour.AlwaysOpen;
            if (value.Equals(OpeningBehaviour.NeverOpen.ToString())) return OpeningBehaviour.NeverOpen;
            if (value.Equals(OpeningBehaviour.AlwaysAsk.ToString())) return OpeningBehaviour.AlwaysAsk;
            throw new OpeningBehaviourParseException(value);
        }
    }

    public enum OpeningBehaviour
    {
        AlwaysOpen,
        NeverOpen,
        AlwaysAsk
    }
}