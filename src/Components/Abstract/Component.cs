using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace BetterHades.Components
{
    public abstract class Component : IObservable<Component>
    {
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
        protected Ellipse OutPointCircle;
        public Polygon Polygon;
        public Point Pos;
        public TextBlock Text;

        protected Component(Point position, bool isActive, string text)
        {
            _outputs = new List<Connection>();
            IsActive = isActive;
            Pos = position;
            Polygon = new Polygon {Fill = Brushes.Gray, Points = GetPoints()};
            App.MainWindow.GridCanvas.Canvas.Children.Add(Polygon);
            OutPointCircle = GenerateIOPort(OutPoint, Brushes.Blue);
            Text = new TextBlock
                   {
                       ZIndex = int.MaxValue,
                       Text = text,
                       Width = 2 * MainWindow.GridCellSize,
                       TextAlignment = TextAlignment.Center,
                       FontSize = MainWindow.GridCellSize,
                       IsEnabled = false,
                       TextWrapping = TextWrapping.Wrap
                   };
            App.MainWindow.GridCanvas.Canvas.Children.Add(Text);
            Canvas.SetLeft(Text, Pos.X - MainWindow.GridCellSize * PositionMultiplier);
            Canvas.SetTop(Text, Pos.Y - MainWindow.GridCellSize * 0.75);
        }

        protected virtual double PositionMultiplier => 1;

        // Properties
        public virtual Point OutPoint => Pos.WithX(Pos.X + MainWindow.GridCellSize);
        public bool IsActive { get; set; }

        // Observable
        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Component> observer)
        {
            _outputs.Add((Connection) observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b) { _outputs.ForEach(o => o.OnNext(this)); }

        // Converters
        public static Dictionary<string, List<Type>> ToDictionary()
        {
            var list = ToList();
            return new Dictionary<string, List<Type>>
                   {
                       {"Gates", list.FindAll(t => t >= (Type) 100 && t < (Type) 200)},
                       {"IO", list.FindAll(t => t >= (Type) 200)}
                   };
        }

        private static List<Type> ToList() { return Enum.GetValues(typeof(Type)).Cast<Type>().ToList(); }
        public override string ToString() { return $"{{{GetType()}, {Pos.X}, {Pos.Y}, {IsActive}}}"; }

        // Helper Methods
        protected static Ellipse GenerateIOPort(Point pos, ISolidColorBrush color)
        {
            const double diameter = MainWindow.GridCellSize / 2.0;
            var result = new Ellipse {Fill = color, Width = diameter, Height = diameter};
            App.MainWindow.GridCanvas.Canvas.Children.Add(result);
            Canvas.SetLeft(result, pos.X - diameter / 2);
            Canvas.SetTop(result, pos.Y - diameter / 2);
            return result;
        }

        public virtual void MoveTo(Point pos)
        {
            var oldOut = OutPoint;
            Pos = pos;
            App.MainWindow.GridCanvas.Canvas.Children.Remove(Polygon);
            Polygon = new Polygon {Width = 100, Height = 100, Fill = Brushes.Gray, Points = GetPoints()};
            App.MainWindow.GridCanvas.Canvas.Children.Add(Polygon);
            App.MainWindow.GridCanvas.Canvas.Children.Remove(OutPointCircle);
            OutPointCircle = GenerateIOPort(OutPoint, Brushes.Blue);
            _outputs.ForEach(c => c.UpdateLine(oldOut, OutPoint));
            Canvas.SetLeft(Text, Pos.X - MainWindow.GridCellSize * PositionMultiplier);
            Canvas.SetTop(Text, Pos.Y - MainWindow.GridCellSize * 0.75 * (Text.FontSize / MainWindow.GridCellSize));
        }

        public virtual void Remove()
        {
            for (var i = 0; i < _outputs.Count; i++) _outputs[i--].Remove();
            App.MainWindow.GridCanvas.Canvas.Children.Remove(Polygon);
            App.MainWindow.GridCanvas.Canvas.Children.Remove(OutPointCircle);
            App.MainWindow.GridCanvas.Canvas.Children.Remove(Text);
        }

        public virtual void Remove(Connection connection)
        {
            if (_outputs.Contains(connection)) _outputs.Remove(connection);
        }

        // Abstract
        protected abstract List<Point> GetPoints();
    }
}