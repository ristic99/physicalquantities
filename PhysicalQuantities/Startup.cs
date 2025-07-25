using Microsoft.Extensions.DependencyInjection;
using PhysicalQuantities.Managers.Window;

namespace PhysicalQuantities;

public class Startup
{
    private readonly IServiceCollection _services = new ServiceCollection();

    public IServiceProvider ConfigureServices()
    {
        _services
            .AddViewModels()
            .AddSingleton<MainWindow>()
            .AddSingleton<IWindowManager, WindowManager>();

        return _services.BuildServiceProvider();
    }
}