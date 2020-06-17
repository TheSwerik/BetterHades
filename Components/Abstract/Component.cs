using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
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

        private readonly List<Connection> _outputs;
        protected readonly Polygon Polygon;
        public readonly Point OutPoint;
        public bool IsClicked;

        protected Component(IPanel parent, double x, double y, Point outPoint)
        {
            _outputs = new List<Connection>();
            Polygon = new Polygon
                      {
                          Width = 100,
                          Height = 100,
                          Fill = Brushes.Gray,
                          Points = GetPoints(x, y)
                      };
            OutPoint = outPoint;
            parent.Children.Add(Polygon);
            Polygon.PointerPressed += SetClicked;
        }

        private void SetClicked(object sender, PointerPressedEventArgs e) { IsClicked = true; }

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Component> observer)
        {
            _outputs.Add((Connection) observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }
        public bool IsActive { get; set; }
        public override string ToString() { return IsActive + ""; }
        private static List<Type> ToList() { return Enum.GetValues(typeof(Type)).Cast<Type>().ToList(); }

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