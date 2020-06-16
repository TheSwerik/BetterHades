﻿using Avalonia.Controls;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : Component, IObservingComponent
    {
        private readonly TextBox _rectangle;

        public Output(TextBox rectangle) { _rectangle = rectangle; }

        public void Update(Connection connection) { ChangeColor(connection.IsActive); }

        private void ChangeColor(bool active) { _rectangle.Background = active ? Brushes.Red : Brushes.Gray; }
    }
}