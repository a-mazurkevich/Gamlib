using Android.Content;
using Gamlib.Services.Shared;
using AndroidApp = Android.App;

namespace Gamlib.Platforms.Android.Services;

public class DeviceInfoService : IDeviceInfoService
{
    private Context _context => AndroidApp.Application.Context;

    public string? GetPlatformCacheDirectory() => _context?.CacheDir?.AbsolutePath;
}

