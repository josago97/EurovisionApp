using BlazorPro.BlazorSize;
using Microsoft.Extensions.DependencyInjection;

namespace EurovisionApp.Common;

public class Program
{
    public static void Init(IServiceCollection services)
    {
        services.AddMediaQueryService();
        services.AddResizeListener();
    }
}
