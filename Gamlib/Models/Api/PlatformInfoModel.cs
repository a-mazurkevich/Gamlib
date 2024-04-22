namespace Gamlib.Models.Api;

public class PlatformInfoModel
{
    public PlatformModel? Platform { get; set; }
    public string? ReleasedAt { get; set; }
    public RequirementsModel? Requirements { get; set; }
}

