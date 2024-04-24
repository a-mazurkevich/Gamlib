using Foundation;
using Gamlib.Services.Shared;

namespace Gamlib.Platforms.iOS.Services;

public class DeviceInfoService : IDeviceInfoService
{
    public string? GetPlatformCacheDirectory() => GetDirectory(NSSearchPathDirectory.CachesDirectory);

    private string? GetDirectory(NSSearchPathDirectory directory)
    {
        var dirs = NSSearchPath.GetDirectories(directory, NSSearchPathDomain.User);

        if (dirs == null || dirs.Length == 0)
        {
            return null;
        }

        return dirs[0];
    }
}
