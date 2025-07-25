using Microsoft.Extensions.DependencyInjection;
using PhysicalQuantities.ViewModels;

namespace PhysicalQuantities
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            return services;
        }
    }
}
