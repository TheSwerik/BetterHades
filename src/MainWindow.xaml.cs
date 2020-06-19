// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Collections;
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
        private MenuItem SaveButton;
        private readonly List<FileDialogFilter> filters;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // this.AttachDevTools();
#endif
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\BetterHades";
            Directory.CreateDirectory(path);
            Environment.CurrentDirectory = path;
            filters = new List<FileDialogFilter>()
                      {
                          new FileDialogFilter()
                          {
                              Extensions = new List<string>() {"bhds"},
                              Name = "BetterHades File"
                          }
                      };
            GridCanvas = new GridCanvas((DockPanel) LogicalChildren[0]);
            KeyDown += KeyPressed;
            SaveButton = (MenuItem) LogicalChildren[0]
                                    .LogicalChildren[0]
                                    .LogicalChildren[0]
                                    .LogicalChildren
                                    .First(c => ((MenuItem) c).Header.Equals("_Save"));
            SaveButton.IsEnabled = false;
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void KeyPressed(object sender, RoutedEventArgs args)
        {
            var arg = (KeyEventArgs) args;
            if ((arg.KeyModifiers & KeyModifiers.Control) != 0 && arg.Key == Key.S)
                if (SaveButton.IsEnabled) Save(null, null);
                else SaveAs(null, null);
        }

        // Title Bar Buttons:
        public void New(object sender, RoutedEventArgs args)
        {
            GridCanvas = new GridCanvas((DockPanel) LogicalChildren[0]);
            FileHandler.New();
            SaveButton.IsEnabled = false;
        }

        public async void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new OpenFileDialog()
                         {
                             Title = "Open BetterHades-File",
                             Filters = filters,
                             Directory = Environment.CurrentDirectory,
                             AllowMultiple = false,
                         };
            var result = await dialog.ShowAsync(this);
            if (result == null) return;
            FileHandler.Load(result[0]);
            SaveButton.IsEnabled = true;
        }

        public async void Save(object sender, RoutedEventArgs args) { FileHandler.Save(); }

        public async void SaveAs(object sender, RoutedEventArgs args)
        {
            var dialog = new SaveFileDialog
                         {
                             Title = "Save BetterHades-File",
                             DefaultExtension = "bhds",
                             InitialFileName = "Unnamed",
                             Filters = filters,
                             Directory = Environment.CurrentDirectory
                         };
            var result = await dialog.ShowAsync(this);
            if (result == null) return;
            FileHandler.Save(result);
            SaveButton.IsEnabled = true;
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