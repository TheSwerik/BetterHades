using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BetterHades.Util;

namespace BetterHades.Frontend
{
    public class Dialog : Window
    {
        public enum ButtonType
        {
            Cancel = 10,
            Ok = 1,
            OkCancel = 11,
            YesNo = 21,
            Save = 22
        }

        public bool Result;
        private readonly IBrush standardColor;

        public Dialog() : this("") { }

        public Dialog(string title = "", string message = "", ButtonType buttonType = ButtonType.Ok, int width = 400,
                      int height = 200)
        {
            AvaloniaXamlLoader.Load(this);
            Width = width;
            Height = height;
            Title = title;
            CanResize = ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Find<TextBlock>("Message").Text = message;
            InitButtons(buttonType);
            standardColor = this.Find<Button>("Ok").Background;
        }

        private void InitButtons(ButtonType buttonType)
        {
            var okButton = this.Find<Button>("Ok");
            var cancelButton = this.Find<Button>("Cancel");
            var saveButton = this.Find<Button>("Save");
            okButton.IsVisible = buttonType != ButtonType.Cancel;
            okButton.IsDefault = true;
            cancelButton.IsVisible = buttonType != ButtonType.Ok;
            cancelButton.IsCancel = true;
            saveButton.IsVisible = buttonType == ButtonType.Save;
            if (buttonType == ButtonType.Save) okButton.Content = "Don't Save";
            if (buttonType != ButtonType.YesNo) return;
            okButton.Content = "Yes";
            cancelButton.Content = "No";
        }

        private void OnHover(object sender, PointerEventArgs e)
        {
            var button = (Button) sender;
            button.Background = Brushes.LightBlue;
        }

        private void NotHover(object sender, PointerEventArgs e)
        {
            var button = (Button) sender;
            button.Background = standardColor;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) { Close(Result = true); }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!App.MainWindow.CanSave)
            {
                var dialog = new SaveFileDialog
                             {
                                 Title = "Save BetterHades-File",
                                 DefaultExtension = "bhds",
                                 InitialFileName = "Unnamed",
                                 Filters = new List<FileDialogFilter>
                                           {
                                               new FileDialogFilter
                                               {
                                                   Extensions = new List<string> {"bhds"},
                                                   Name = "BetterHades File"
                                               }
                                           },
                                 Directory = Environment.CurrentDirectory
                             };
                var result = await dialog.ShowAsync(this);
                if (result == null) return;
                FileHandler.Save(result);
                Config.AddFileToHistory(result);
            }

            Close(Result = true);
        }

        public Task<bool> Show(Window owner) { return ShowDialog<bool>(owner); }

        private void CancelButton_Click(object sender, RoutedEventArgs e) { Close(Result); }
    }
}