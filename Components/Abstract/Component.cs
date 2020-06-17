using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public abstract class Component : IComponent
    {
        public enum Type
        {
            Connection = 0,
            AND = 100,
            Input = 200,
            InputClock = 201,
            Output = 202
        }

        private readonly List<IObserver<IComponent>> _outputs;

        protected Component() { _outputs = new List<IObserver<IComponent>>(); }

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<IComponent> observer)
        {
            _outputs.Add(observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }

        public bool IsActive { get; set; }
        public override string ToString() { return IsActive + ""; }
        public static List<Type> ToList() => Enum.GetValues(typeof(Type)).Cast<Type>().ToList();

        public static Dictionary<string, List<Type>> ToDictionary()
        {
            var list = ToList();
            return new Dictionary<string, List<Type>>
                   {
                       {"Gates", list.FindAll(t => t >= (Type) 100 && t < (Type) 200)},
                       {"IO", list.FindAll(t => t >= (Type) 200)}
                   };
        }
    }
}