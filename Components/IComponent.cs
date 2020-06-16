using System;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public interface IComponent : IObserver<Connection>, IObservable<IComponent>
    {
        // Implemented:
        void IObserver<Connection>.OnCompleted() { throw new CompletedException(); }
        void IObserver<Connection>.OnError(Exception error) { Console.WriteLine("COMPONENT ERROR --- ", error); }
        void IObserver<Connection>.OnNext(Connection connection) { Update(connection); }

        // Abstract:
        protected void Update(Connection connection);
        protected void Notify(bool b);
    }
}