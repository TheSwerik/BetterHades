using System;

namespace BetterHades.Components
{
    public interface IComponent : IObservable<IComponent>
    {
        public bool IsActive { get; set; }

        // Implemented:
        public string ToString() { return IsActive + ""; }

        // Abstract:
        public void Notify(bool b);
    }
}