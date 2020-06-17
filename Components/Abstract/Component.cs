using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

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
        protected readonly Polygon _polygon;
        public readonly Point OutPoint;

        protected Component(IPanel parent, double x, double y, Point outPoint)
        {
            _outputs = new List<IObserver<IComponent>>();
            _polygon = new Polygon
                       {
                           Width = 100,
                           Height = 100,
                           Fill = Brushes.Gray,
                           Points = GetPoints(x, y)
                       };
            OutPoint = outPoint;
            parent.Children.Add(_polygon);
        }

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

        public static Dictionary<string, List<Type>> ToDictionary()
        {
            var list = ToList();
            return new Dictionary<string, List<Type>>
                   {
                       {"Gates", list.FindAll(t => t >= (Type) 100 && t < (Type) 200)},
                       {"IO", list.FindAll(t => t >= (Type) 200)}
                   };
        }

        protected abstract List<Point> GetPoints(double x, double y);
    }
}