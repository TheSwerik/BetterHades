using System;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public interface IObservingComponent : IComponent, IObserver<Connection>
    {
        // Implemented:
        void IObserver<Connection>.OnCompleted() { throw new CompletedException(); }
        void IObserver<Connection>.OnError(Exception error) { Console.WriteLine(error); }
        void IObserver<Connection>.OnNext(Connection value) { Update(value); }

        // Abstract:
        public void Update(Connection connection);
    }
}