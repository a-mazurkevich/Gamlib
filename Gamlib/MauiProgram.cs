using Gamlib.Extensions;
using Microsoft.Extensions.Logging;

namespace Gamlib;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureAppEnvironment();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder ConfigureAppEnvironment(this MauiAppBuilder builder)
    {
        builder.Services
            .AddPlatformSpecificServices()
            .AddServices()
            .AddApiServices()
            .AddViewModels();
        return builder;
    }
}