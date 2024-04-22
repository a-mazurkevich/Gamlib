namespace Gamlib.Models.Api;

public class GamesChunkModel
{
	public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<GameModel>? Results { get; set; }
}

