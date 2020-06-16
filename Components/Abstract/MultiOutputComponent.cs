using System;
using System.Collections.Generic;

namespace BetterHades.Components
{
    public abstract class MultiOutputComponent : IComponent
    {
        private readonly List<IObserver<IComponent>> _outputs;
        private bool _isActive;
        protected MultiOutputComponent() { _outputs = new List<IObserver<IComponent>>(); }

        public IDisposable Subscribe(IObserver<IComponent> observer)
        {
            _outputs.Add(observer);
            return (IDisposable) observer;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }
        public abstract void Update();
        public bool IsActive() { return _isActive; }
        protected void SetActive(bool isActive) { _isActive = isActive; }
    }
}