using System;

namespace BetterHades.Components
{
    public interface IComponent : IObservable<Component>
    {
        public bool IsActive { get; set; }

        // Abstract:
        public void Notify(bool b);
    }
}