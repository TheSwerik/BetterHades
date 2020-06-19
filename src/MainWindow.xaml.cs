// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BetterHades.Frontend;
using BetterHades.Util;

namespace BetterHades
{
    public class MainWindow : Window
    {
        public GridCanvas GridCanvas;
        public Canvas BackgroundCanvas;
        private readonly MenuItem _saveButton;
        private readonly RightClickContextMenu _contextMenu;
        private readonly List<FileDialogFilter> _filters;
        private readonly ZoomBorder _zoomBorder;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // this.AttachDevTools();
#endif
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BetterHades";
            Directory.CreateDirectory(path);
            Environment.CurrentDirectory = path;
            _filters = new List<FileDialogFilter>()
                       {
                           new FileDialogFilter()
                           {
                               Extensions = new List<string>() {"bhds"},
                               Name = "BetterHades File"
                           }
                       };
            BackgroundCanvas = (Canvas) LogicalChildren[0];
            _zoomBorder = this.Find<ZoomBorder>("zoomBorder");
            GridCanvas = new GridCanvas(_zoomBorder);
            _contextMenu = new RightClickContextMenu(this.Find<ContextMenu>("contextMenu"));
            GridCanvas.Canvas.PointerPressed += ClickHandler;
            KeyDown += KeyPressed;
            _saveButton = this.Find<MenuItem>("saveButton");
            _saveButton.IsEnabled = false;
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if ((e.KeyModifiers & KeyModifiers.Control) != 0 && e.Key == Key.S)
                if (_saveButton.IsEnabled) Save(null, null);
                else SaveAs(null, null);
        }

        private void ClickHandler(object sender, PointerPressedEventArgs e)
        {
            var pos = e.GetCurrentPoint(BackgroundCanvas).Position;
            // Console.WriteLine(pos.ToString());
            if (e.MouseButton == MouseButton.Right) _contextMenu.Show(pos.X, pos.Y);
            else if (e.MouseButton == MouseButton.Left) _contextMenu.Hide();
        }

        // Title Bar Buttons:
        public void New(object sender, RoutedEventArgs args)
        {
            GridCanvas = new GridCanvas(_zoomBorder);
            FileHandler.New();
            _saveButton.IsEnabled = false;
        }

        public async void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new OpenFileDialog()
                         {
                             Title = "Open BetterHades-File",
                             Filters = _filters,
                             Directory = Environment.CurrentDirectory,
                             AllowMultiple = false,
                         };
            var result = await dialog.ShowAsync(this);
            if (result == null) return;
            FileHandler.Load(result[0]);
            _saveButton.IsEnabled = true;
        }

        public async void Save(object sender, RoutedEventArgs args) { FileHandler.Save(); }

        public async void SaveAs(object sender, RoutedEventArgs args)
        {
            var dialog = new SaveFileDialog
                         {
                             Title = "Save BetterHades-File",
                             DefaultExtension = "bhds",
                             InitialFileName = "Unnamed",
                             Filters = _filters,
                             Directory = Environment.CurrentDirectory
                         };
            var result = await dialog.ShowAsync(this);
            if (result == null) return;
            FileHandler.Save(result);
            _saveButton.IsEnabled = true;
        }

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