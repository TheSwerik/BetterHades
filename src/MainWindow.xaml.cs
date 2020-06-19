// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global

using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BetterHades.Frontend;
using BetterHades.Util;

namespace BetterHades
{
    public class MainWindow : Window
    {
        public GridCanvas GridCanvas;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // this.AttachDevTools();
#endif
            GridCanvas = new GridCanvas((DockPanel) LogicalChildren[0]);
            KeyDown += KeyPressed;
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void KeyPressed(object sender, RoutedEventArgs args)
        {
            var arg = (KeyEventArgs) args;
            if ((arg.KeyModifiers & KeyModifiers.Control) != 0 && arg.Key == Key.S) FileHandler.Save();
        }

        public void New(object sender, RoutedEventArgs args)
        {
            GridCanvas = new GridCanvas((DockPanel) LogicalChildren[0]);
            FileHandler.New();
        }

        public void Save(object sender, RoutedEventArgs args) { FileHandler.Save("Test"); }

        public void Load(object sender, RoutedEventArgs args) { FileHandler.Load("Test"); }
        public void Exit(object sender, RoutedEventArgs args) { Close(); }

        public void AboutOnClick(object sender, RoutedEventArgs args)
        {
            var window = new Window
                         {
                             Title = "About",
                             CanResize = false,
                             ShowInTaskbar = false,
                             Width = 300,
                             Height = 100,
                             WindowStartupLocation = WindowStartupLocation.CenterOwner,
                             Content = string.Join("\n", File.ReadLines(@"res\about.txt"))
                         };
            window.ShowDialog(App.MainWindow);
        }
    }
}