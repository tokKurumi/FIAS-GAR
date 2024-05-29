namespace GAR.Services.SearchApi.Models;

public class SearchResult<T>
{
    public T? Source { get; set; }

    public double Score { get; set; }
}
