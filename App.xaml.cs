using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using PhysicalQuantities.Managers.Window;

namespace PhysicalQuantities;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Add handlers for unhandled exceptions
        Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        base.OnStartup(e);
        var startup = new Startup();
        var serviceProvider = startup.ConfigureServices();
        var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
        var windowService = serviceProvider.GetRequiredService<IWindowManager>();
        windowService.Show(mainWindow);
    }

    private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // Log the exception details
        MessageBox.Show($"An error occurred: {e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}",
            "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        // Prevent the application from crashing
        e.Handled = true;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            MessageBox.Show($"A fatal error occurred: {exception.Message}\n\nStack Trace:\n{exception.StackTrace}",
                "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

