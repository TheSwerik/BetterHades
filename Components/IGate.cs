using System;
using BetterHades.Exceptions;
using SharpDX;

namespace BetterHades.Components
{
    public interface IGate : IComponent, IObserver<Connection>
    {
        void IObserver<Connection>.OnCompleted() { throw new CompletedException(); }
        void IObserver<Connection>.OnError(Exception error) { Console.WriteLine(error); }
        void IObserver<Connection>.OnNext(Connection value) { Update(value); }
    }
}