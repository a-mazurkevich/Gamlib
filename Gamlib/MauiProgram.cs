using Gamlib.Services.Api;
using Gamlib.ViewModels;
using Microsoft.Extensions.Logging;
using Refit;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        ConfigureApiServices(builder.Services);
        ConfigureViewModels(builder.Services);

        return builder.Build();
    }

    private static void ConfigureApiServices(IServiceCollection services)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rawg.io/api") };
        services.AddSingleton(RestService.For<IGamesApi>(httpClient));
    }

    private static void ConfigureViewModels(IServiceCollection services)
    {
        services.AddTransient<HomePageViewModel>();
    }

}