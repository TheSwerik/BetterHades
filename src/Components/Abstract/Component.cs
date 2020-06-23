using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace BetterHades.Components
{
    public abstract class Component : IObservable<Component>
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Type
        {
            Connection = 0,
            AND = 100,
            INV = 101,
            NAND = 102,
            NOR = 103,
            OR = 104,
            XNOR = 105,
            XOR = 106,
            Input = 200,
            InputClock = 201,
            Output = 202
        }

        private readonly List<Connection> _outputs;
        protected readonly Ellipse OutPointCircle;
        protected readonly Polygon Polygon;
        public Point Pos;

        protected Component(Point position, bool isActive)
        {
            _outputs = new List<Connection>();
            Pos = position;
            Polygon = new Polygon
                      {
                          Width = 100,
                          Height = 100,
                          Fill = Brushes.Gray,
                          Points = GetPoints()
                      };
            App.MainWindow.GridCanvas.Canvas.Children.Add(Polygon);

            const double diameter = MainWindow.GridCellSize / 2.0;
            OutPointCircle = new Ellipse {Fill = Brushes.Coral, Width = diameter, Height = diameter};
            App.MainWindow.GridCanvas.Canvas.Children.Add(OutPointCircle);
            Canvas.SetLeft(OutPointCircle, OutPoint.X - diameter / 2);
            Canvas.SetTop(OutPointCircle, OutPoint.Y - diameter / 2);
            IsActive = isActive;
        }

        public virtual Point OutPoint => Pos.WithX(Pos.X + MainWindow.GridCellSize);

        public bool IsActive { get; set; }

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Component> observer)
        {
            _outputs.Add((Connection) observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }
        public override string ToString() { return $"{{{GetType()}, {Pos.X}, {Pos.Y}, {IsActive}}}"; }
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

        protected abstract List<Point> GetPoints();
    }
}