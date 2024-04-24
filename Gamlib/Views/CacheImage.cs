using Gamlib.Enums;
using Gamlib.Helpers;
using Gamlib.Services;
using Gamlib.Services.Interfaces;

namespace Gamlib.Views;

public class CacheImage : Image
{
    private CancellationTokenSource _sourceCancellationTokenSource;
    private IImageCacheService _cacheImageService;

    public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create(nameof(ImageUrl), typeof(string),
        typeof(CacheImage), null, propertyChanged: OnImageUrlChanged);

    public static readonly BindableProperty CacheModeProperty = BindableProperty.Create(nameof(CacheMode), typeof(ImageCache),
        typeof(CacheImage), default, propertyChanged: OnCacheModeChanged);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public ImageCache CacheMode
    {
        get => (ImageCache)GetValue(CacheModeProperty);
        set => SetValue(CacheModeProperty, value);
    }

    public string FailedLoadedImageName { get; set; }

    public string PlaceholderImageName { get; set; }

    public CacheImage()
    {
        _cacheImageService = ServiceHelper.GetService<DownloadImageService>();

    }

    private static void OnImageUrlChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CacheImage control)
        {
            control.UpdateImage();
        }
    }

    private static void OnCacheModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CacheImage control)
        {
            control.UpdateCacheMode();
        }
    }

    private async void UpdateImage()
    {
        _sourceCancellationTokenSource?.Cancel();

        Source = PlaceholderImageName;
        await Task.Delay(100);

        if (!string.IsNullOrEmpty(ImageUrl))
        {
            _sourceCancellationTokenSource = new CancellationTokenSource();
            var token = _sourceCancellationTokenSource.Token;
            var result = await _cacheImageService.GetImageAsync(ImageUrl, token);
            Source = result ?? FailedLoadedImageName;
        }
        else
        {
            Source = FailedLoadedImageName;
        }
    }

    private void UpdateCacheMode()
    {
        _cacheImageService = CacheMode switch
        {
            ImageCache.None => ServiceHelper.GetService<DownloadImageService>(),
            ImageCache.LocalSave => ServiceHelper.GetService<ImageCacheService>(),
            _ => ServiceHelper.GetService<DownloadImageService>(),
        };
    }
}
