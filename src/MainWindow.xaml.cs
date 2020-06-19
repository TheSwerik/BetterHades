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
            if ((arg.KeyModifiers & KeyModifiers.Control) != 0 && arg.Key == Key.S) Save(null, null);
        }

        public void Open(object sender, RoutedEventArgs args) { }

        public void New(object sender, RoutedEventArgs args)
        {
            GridCanvas = new GridCanvas((DockPanel) LogicalChildren[0]);
            FileHandler.New();
            SaveButton.IsEnabled = false;
        }

        public async void Save(object sender, RoutedEventArgs args)
        {
            if (!SaveButton.IsEnabled)
            {
                var x = new SaveFileDialog
                        {
                            Title = "Save BetterHades-File",
                            DefaultExtension = "bhds",
                            InitialFileName = "Unnamed",
                            Filters = filters,
                        };
                var y = await x.ShowAsync(this);
                if (y == null) return;
                FileHandler.Save(y);
            }
            else FileHandler.Save();
        }

        public void Load(object sender, RoutedEventArgs args) { FileHandler.Load("Test.bhds"); }
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