using Gamlib.Services.Interfaces;

namespace Gamlib.Services;

public class DownloadImageService : IImageCacheService
{
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
                    var httpClient = new HttpClient();
                    try
                    {
                        var data = await httpClient.GetByteArrayAsync(imageUri);
                        if (data != null)
                        {
                            imageSource = ImageSource.FromStream(() => new MemoryStream(data));
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

