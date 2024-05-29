namespace GAR.Services.SearchApi.Models;

using System.Text.Json.Serialization;

public class FullAddress
{
    [JsonPropertyName("address")]
    public string Address { get; set; } = string.Empty;

    [JsonPropertyName("objectid")]
    public long ObjectId { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;
}
