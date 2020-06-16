namespace BetterHades.Components.IO
{
    public class InputImpl : Components.Input
    {
        protected override bool Check() { return IsActive; }
    }
}