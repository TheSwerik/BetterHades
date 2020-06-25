using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Themes.Fluent;

namespace BetterHades.Frontend
{
    public class Dialog : Window
    {
        private IBrush standardColor;
        public bool Result = false;

        public enum ButtonType
        {
            Cancel = 10,
            Ok = 1,
            OkCancel = 11,
            YesNo = 21,
        }

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
            okButton.IsVisible = buttonType != ButtonType.Cancel;
            okButton.IsDefault = true;
            cancelButton.IsVisible = buttonType != ButtonType.Ok;
            cancelButton.IsCancel = true;
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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close(Result = true);
        }
        
        public Task<bool> Show(Window owner) => this.ShowDialog<bool>(owner);
        

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close(Result);
    }
}