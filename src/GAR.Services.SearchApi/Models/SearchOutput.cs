namespace GAR.Services.SearchApi.Models;

public class SearchOutput<T>
{
    public IEnumerable<SearchResult<T>> Data { get; set; } = [];

    public long Took { get; set; }
}
