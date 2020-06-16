using System;
using System.Collections.Generic;

namespace BetterHades.Components
{
    public abstract class MultiOutputComponent : IComponent
    {
        private readonly List<IObserver<IComponent>> _outputs;

        protected MultiOutputComponent() { _outputs = new List<IObserver<IComponent>>(); }

        public IDisposable Subscribe(IObserver<IComponent> observer)
        {
            _outputs.Add(observer);
            return (IDisposable) observer;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }
        public abstract void Update();
        public abstract bool IsActive();
    }
}