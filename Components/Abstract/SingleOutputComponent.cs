using System;

namespace BetterHades.Components
{
    public abstract class SingleOutputComponent : IComponent
    {
        private IObserver<IComponent> _output;

        public IDisposable Subscribe(IObserver<IComponent> observer)
        {
            _output = observer;
            return (IDisposable) observer;
        }

        public void Notify(bool b) { _output?.OnNext(this); }
        public abstract void Update();
        public abstract bool IsActive();
    }
}