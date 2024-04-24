namespace Gamlib.Services.Interfaces;

public interface IImageCacheService
{
    Task<ImageSource?> GetImageAsync(string imageUrl, CancellationToken token);
}

