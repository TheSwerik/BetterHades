using System;

namespace BetterHades.Components
{
    public interface IComponent : IObservable<IComponent>
    {
        // Implemented:
        public string ToString() { return IsActive() + ""; }

        // Abstract:
        public void Update();
        public void Notify(bool b);
        public bool IsActive();
    }
}