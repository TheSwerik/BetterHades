using System;
using System.Collections.Generic;

namespace BetterHades.Components
{
    public abstract class Component : IComponent
    {
        private readonly List<IObserver<IComponent>> _outputs;

        protected Component() { _outputs = new List<IObserver<IComponent>>(); }

        public IDisposable Subscribe(IObserver<IComponent> observer)
        {
            _outputs.Add(observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }

        public bool IsActive { get; set; }
    }
}