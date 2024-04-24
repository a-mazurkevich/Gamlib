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

//    private static void ConfigureApiServices(IServiceCollection services)
//    {
//        var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rawg.io/api") };
//        services.AddSingleton(RestService.For<IGamesApi>(httpClient));
//    }

//    private static void ConfigureViewModels(IServiceCollection services)
//    {
//        services.AddTransient<HomePageViewModel>();
//    }

//    private static void ConfigureSharedServices(IServiceCollection services)
//    {
//#if ANDROID
//        services.AddSingleton<IDeviceInfoService, Platforms.Android.Services.DeviceInfoService>();
//#elif IOS
//        services.AddSingleton<IDeviceInfoService, Platforms.iOS.Services.DeviceInfoService>();
//#endif
//    }

}