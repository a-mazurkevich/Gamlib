using Gamlib.Models.Api;
using Refit;

namespace Gamlib.Services.Api;


public interface IGamesApi
{
    const string API_KEY = "689bde999a444ca090e426a15d696e4f";

    [Get($"/games?key={API_KEY}")]
	Task<GamesChunkModel> GetGames([AliasAs("page")] int page = 1, [AliasAs("page_size")] int pageSize = 20);
}

