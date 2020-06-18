using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace BetterHades
{
    public class App : Application
    {
        public static MainWindow MainWindow;
        public override void Initialize() { AvaloniaXamlLoader.Load(this); }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = MainWindow = new MainWindow();
            base.OnFrameworkInitializationCompleted();
        }
    }
}