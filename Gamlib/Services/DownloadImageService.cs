using Gamlib.Services.Interfaces;
using Microsoft.Maui.Graphics.Platform;

namespace Gamlib.Services;

public class DownloadImageService : IImageCacheService
{
    private readonly HttpClient _httpClient = new();

    public async Task<ImageSource?> GetImageAsync(string imageUrl, CancellationToken token)
    {
        ImageSource? imageSource = null;

        if (!string.IsNullOrEmpty(imageUrl))
        {
            imageSource = await DownloadImageAsync(imageUrl, token);
        }

        return imageSource;
    }

    private async Task<ImageSource?> DownloadImageAsync(string imageUrl, CancellationToken token)
    {
        ImageSource? imageSource = null;

        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri))
        {
            await Task.Run(async () =>
            {
                if (!token.IsCancellationRequested)
                {
                    try
                    {
                        var data = await _httpClient.GetByteArrayAsync(imageUri);
                        if (data != null)
                        {
                            var iimage = PlatformImage.FromStream(new MemoryStream(data));
                            iimage = iimage.Downsize(90, 60);

                            using var stream = iimage.AsStream();
                            imageSource = ImageSource.FromStream(() => stream);
                            //imageSource = ImageSource.FromStream(() => new MemoryStream(data));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {nameof(DownloadImageAsync)} - {imageUrl}: {ex.Message}");
                    }
                }
            });
        }

        return imageSource;
    }
}