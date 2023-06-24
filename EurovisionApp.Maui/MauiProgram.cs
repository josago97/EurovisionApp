using EurovisionApp.Common.Logic.Data;
using EurovisionApp.Maui.Logic;
using Microsoft.Extensions.Logging;

namespace EurovisionApp.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IRepository, Repository>();
            Common.Program.Init(builder.Services);

            return builder.Build();
        }
    }
}