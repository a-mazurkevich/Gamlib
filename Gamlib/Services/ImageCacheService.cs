using System.Collections.Concurrent;
using System.Security.Cryptography;
using Gamlib.Helpers;
using Gamlib.Services.Interfaces;
using Gamlib.Services.Shared;
using Microsoft.Maui.Graphics.Platform;

namespace Gamlib.Services;

public class ImageCacheService : IImageCacheService
{
    private const string GAMES_IMAGE_CACHE_FOLDER = "games_imgs";
    private const int MAX_PATH_LENGTH = 260;
    private const string FILE_NAME_TEMPLATE = "{0}_{1}";
    private const string HTTPS_SCHEME = "https";

    private readonly IDeviceInfoService _deviceInfoService;

    private readonly HttpClient _httpClient = new();

    private readonly TimeSpan _defaultCacheTime = TimeSpan.FromDays(7);
    private ConcurrentBag<string> _cachedImageNames = new();

    private string CachePath => Path.Combine(_deviceInfoService.GetPlatformCacheDirectory() ?? string.Empty, GAMES_IMAGE_CACHE_FOLDER);

    public ImageCacheService(IDeviceInfoService deviceInfoService)
    {
        _deviceInfoService = deviceInfoService;
    }

    public void Initialize()
    {
        Initialize(_defaultCacheTime);
    }

    public void Initialize(TimeSpan cacheTime)
    {
        if (Directory.Exists(CachePath))
        {
            var directory = new DirectoryInfo(CachePath);
            var now = DateTime.Now;

            foreach (var file in directory.GetFiles())
            {
                if (long.TryParse(file.Name.AsSpan(0, file.Name.IndexOf('_')), out var ticks))
                {
                    var cachedDateTime = new DateTime(ticks);
                    var delta = now - cachedDateTime;

                    if (delta > cacheTime)
                    {
                        file.Delete();
                    }
                    else
                    {
                        _cachedImageNames.Add(file.Name);
                    }
                }
            }
        }
        else
        {
            Directory.CreateDirectory(CachePath);
        }
    }

    public async Task<ImageSource?> GetImageAsync(string imageUrl, CancellationToken token)
    {
        ImageSource? imageSource = null;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            var imageNameHash = CryptoHelper.GetHash<MD5>(GetImageNameFromUrl(imageUrl) ?? string.Empty);
            var imageName = _cachedImageNames.FirstOrDefault(x => x.Contains(imageNameHash));

            if (string.IsNullOrEmpty(imageName))
            {
                imageSource = await DownloadAndSaveImageAsync(imageUrl, imageNameHash, token);
            }
            else
            {
                imageSource = TryToGetCachedImage(imageName);
            }
        }

        return imageSource;
    }

    public void ClearAllCache()
    {
        if (Directory.Exists(CachePath))
        {
            _cachedImageNames = new ConcurrentBag<string>();
            var directory = new DirectoryInfo(CachePath);

            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }
    }

    private async Task<ImageSource?> DownloadAndSaveImageAsync(string imageUrl, string imageNameHash, CancellationToken token)
    {
        ImageSource? imageSource = null;

        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri))
        {
            await Task.Run(async () =>
            {
                if (!token.IsCancellationRequested)
                {
                    byte[]? data = null;

                    try
                    {
                        var response = await _httpClient.GetAsync(imageUri);
                        if (response.IsSuccessStatusCode && response.Content != null)
                        {
                            data = await response.Content.ReadAsByteArrayAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            $"Error in {nameof(DownloadAndSaveImageAsync)} - {imageUrl}: {ex.Message}");
                    }

                    if (data != null)
                    {
                        var now = DateTime.Now.Ticks;

                        var imageName = string.Format(FILE_NAME_TEMPLATE, now, imageNameHash);
                        var imagePath = Path.Combine(CachePath, imageName);

                        if (imagePath.Length <= MAX_PATH_LENGTH)
                        {
                            var result = false;

                            while (!result)
                            {
                                try
                                {
                                    var iimage = PlatformImage.FromStream(new MemoryStream(data));
                                    iimage = iimage.Downsize(180);

                                    var resizedData = await iimage.AsBytesAsync();
                                    File.WriteAllBytes(imagePath, resizedData);
                                    result = true;
                                    //File.WriteAllBytes(imagePath, data);
                                }
                                catch (IOException ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(
                            $"Error in {nameof(DownloadAndSaveImageAsync)} - {imageUrl}: {ex.Message}");
                                }
                            }

                            _cachedImageNames.Add(imageName);
                            imageSource = ImageSource.FromFile(imagePath);
                        }
                        else
                        {
                            imageSource = ImageSource.FromStream(() => new MemoryStream(data));
                        }
                    }
                }
            });
        }

        return imageSource;
    }

    private ImageSource? TryToGetCachedImage(string name)
    {
        var imagePath = Path.Combine(CachePath, name);

        return File.Exists(imagePath) ? ImageSource.FromFile(imagePath) : null;
    }

    private string? GetImageNameFromUrl(string imageUrl) => imageUrl?.Replace(HTTPS_SCHEME, string.Empty).Replace('/', '_');
}

