using Gamlib.Services;
using Gamlib.Services.Api;
using Gamlib.Services.Shared;
using Gamlib.ViewModels;
using Refit;

namespace Gamlib.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://api.rawg.io/api") };
        services.AddSingleton(RestService.For<IGamesApi>(httpClient));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ImageCacheService, ImageCacheService>();
        services.AddSingleton<DownloadImageService, DownloadImageService>();
        return services;
    }

    public static IServiceCollection AddPlatformSpecificServices(this IServiceCollection services)
    {
#if ANDROID
        services.AddSingleton<IDeviceInfoService, Platforms.Android.Services.DeviceInfoService>();
#elif IOS
        services.AddSingleton<IDeviceInfoService, Platforms.iOS.Services.DeviceInfoService>();
#endif
        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<HomePageViewModel>();
        return services;
    }
}

