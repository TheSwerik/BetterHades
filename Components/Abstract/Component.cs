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
            Connection,
            AND,
            Input,
            InputClock,
            Output
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

        public static List<Type> ToList() { return Enum.GetValues(typeof(Type)).Cast<Type>().ToList(); }

        public static System.Type GetComponent(Component.Type componentType)
        {
            var typeString = componentType switch
                             {
                                 Type.Connection => "Connection",
                                 Type.AND => "Gates.ANDGate",
                                 Type.Input => "IO.Input",
                                 Type.InputClock => "IO.InputClock",
                                 Type.Output => "IO.Output",
                                 _ => ""
                             };
            var t = System.Type.GetType($"BetterHades.Components.Implementations.{typeString}");
            if (t == null) throw new ComponentNotFoundException(componentType);
            return t;
        }
    }
}