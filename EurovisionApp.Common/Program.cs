using BootstrapBlazor;
using BlazorPro.BlazorSize;
using Microsoft.Extensions.DependencyInjection;

namespace EurovisionApp.Common;

public class Program
{
    public static void Init(IServiceCollection services)
    {
        services.AddBootstrapBlazor();

        services.AddMediaQueryService();
        services.AddResizeListener();

        services.AddSingleton<Navigator>();
    }
}
