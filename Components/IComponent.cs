using System;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public interface IComponent: IObservable<IComponent>
    {
        // Implemented:
        public string ToString() { return IsActive() + ""; }

        // Abstract:
        protected void Update(Connection connection);
        protected void Notify(bool b);
        public bool IsActive();
    }
}