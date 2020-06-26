// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BetterHades.Frontend;
using BetterHades.Util;
using BetterHades.Util.Enums;

namespace BetterHades
{
    public class MainWindow : Window
    {
        public const int GridSize = 5000;
        public const int GridCellSize = 25;
        private readonly List<FileDialogFilter> _filters;
        private readonly MenuItem _saveButton;
        private readonly ZoomBorder _zoomBorder;
        public readonly RightClickContextMenu RightClickContextMenu;
        public GridCanvas GridCanvas;

        public MainWindow()
        {
            InitializeComponent();
            SetDirectory();
            KeyDown += KeyPressed;
            _filters = new List<FileDialogFilter>
                       {
                           new FileDialogFilter
                           {
                               Extensions = new List<string> {"bhds"},
                               Name = "BetterHades File"
                           }
                       };
            Closing += OnClose;
            _zoomBorder = this.Find<ZoomBorder>("zoomBorder");
            _saveButton = this.Find<MenuItem>("saveButton");
            _saveButton.IsEnabled = false;
            RightClickContextMenu = new RightClickContextMenu(this.Find<ContextMenu>("contextMenu"));
            GridCanvas = new GridCanvas(_zoomBorder);
            Config.Init();
            UpdateFileHistory();
        }

        public bool CanSave => _saveButton.IsEnabled;

        public void UpdateFileHistory()
        {
            var fileMenu = this.Find<MenuItem>("FileMenu");
            var items = new List<MenuItem>();
            items.AddRange(fileMenu.Items.Cast<MenuItem>().TakeWhile(i => !i.Header.Equals("-")));
            items.Add(new MenuItem {Header = "-"});
            foreach (var item in Config.FileHistory.Select(file => new MenuItem {Header = file}))
            {
                item.Click += FileHistoryClick;
                items.Add(item);
            }

            fileMenu.Items = items;
        }

        private static void SetDirectory()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/BetterHades/Projects";
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (UnauthorizedAccessException)
            {
                path = "/BetterHades/Projects";
                Directory.CreateDirectory(path);
            }

            Environment.CurrentDirectory = path;
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        // Handlers
        public void KeyPressed(object sender, KeyEventArgs e)
        {
            if ((e.KeyModifiers & KeyModifiers.Control) != 0 && e.Key == Key.S)
                if (CanSave) Save(null, null);
                else SaveAs(null, null);
        }

        private async void OnClose(object sender, CancelEventArgs e)
        {
            if (!FileHandler.HasChanged) return;
            e.Cancel = true;
            var dialog = new Dialog(
                "Exit?",
                "You have unsaved changes.",
                Dialog.ButtonType.Save,
                220,
                120
            );
            if (!await dialog.Show(this)) return;
            FileHandler.HasChanged = false;
            Close();
        }

        private void FileHistoryClick(object sender, RoutedEventArgs e)
        {
            var item = ((MenuItem) sender).Header as string;
            FileHandler.Load(item);
            _saveButton.IsEnabled = true;
            Config.AddFileToHistory(item);
        }

        // Title Bar Buttons:
        public async void New(object sender, RoutedEventArgs args)
        {
            var dialog = new Dialog(
                new[] {"New Window", "This Window", "Cancel"},
                "Remember my decision.",
                "New...",
                "Open in a new Window?",
                Dialog.ButtonType.Save,
                300,
                100
            );

            var result = await dialog.ShowDialog<bool[]>(this);
            if (result == null || !result[0]) return;
            if (result[1])
            {
                if (dialog.Checkbox) Config.OpeningBehaviour = OpeningBehaviour.AlwaysOpen;
                //TODO new Window
            }
            else
            {
                if (dialog.Checkbox) Config.OpeningBehaviour = OpeningBehaviour.NeverOpen;
                GridCanvas = new GridCanvas(_zoomBorder);
                FileHandler.New();
                _saveButton.IsEnabled = false;
            }
        }

        public async void Open(object sender, RoutedEventArgs args)
        {
            var dialog = new OpenFileDialog
                         {
                             Title = "Open BetterHades-File",
                             Filters = _filters,
                             Directory = Environment.CurrentDirectory,
                             AllowMultiple = false
                         };
            var result = await dialog.ShowAsync(this);
            if (result == null || result.Length <= 0) return;
            FileHandler.Load(result[0]);
            _saveButton.IsEnabled = true;
            Config.AddFileToHistory(result[0]);
        }

        private static void Save(object sender, RoutedEventArgs args) { FileHandler.Save(); }

        private async void SaveAs(object sender, RoutedEventArgs args)
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
            Config.AddFileToHistory(result);
        }

        public async void Exit(object sender, RoutedEventArgs args) { Close(); }

        public async void AboutOnClick(object sender, RoutedEventArgs args)
        {
            var x = new Dialog(
                "About",
                string.Join("\n", File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + @"res\about.txt")),
                Dialog.ButtonType.Ok,
                220,
                120
            ).Show(this);
        }
    }
}