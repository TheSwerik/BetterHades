using System;

namespace BetterHades.Components
{
    public interface IComponent : IObservable<IComponent>
    {
        public bool IsActive { get; set; }

        // Abstract:
        public void Notify(bool b);
    }
}