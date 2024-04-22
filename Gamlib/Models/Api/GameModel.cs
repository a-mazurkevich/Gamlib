using System.Text.Json.Serialization;

namespace Gamlib.Models.Api;

public class GameModel
{
    public long Id { get; set; }
    public string? Slug { get; set; }
    public string? Name { get; set; }
    public DateTime? Released { get; set; }
    public bool Tba { get; set; }
    [JsonPropertyName("background_image")]
    public string? BackgroundImage { get; set; }
    public double Rating { get; set; }
    [JsonPropertyName("rating_top")]
    public int RatingTop { get; set; }
    public List<RaitingModel>? Ratings { get; set; }
    [JsonPropertyName("ratings_count")]
    public int RatingsCount { get; set; }
    [JsonPropertyName("reviews_text_count")]
    public int ReviewsTextCount { get; set; }
    public int Added { get; set; }
    [JsonPropertyName("added_by_status")]
    public object? AddedByStatus { get; set; }
    public int? Metacritic { get; set; }
    public int Playtime { get; set; }
    [JsonPropertyName("suggestions_count")]
    public int SuggestionsCount { get; set; }
    public DateTime? Updated { get; set; }
    [JsonPropertyName("esrb_rating")]
    public EsrbRatingModel? EsrbRating { get; set; }
    public List<PlatformInfoModel>? Platforms { get; set; }
}

