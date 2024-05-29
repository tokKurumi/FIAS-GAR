namespace GAR.Services.SearchApi.Services;

using Elastic.Clients.Elasticsearch;
using GAR.Services.SearchApi.Models;

public class SearchService(ElasticsearchClient client)
{
    private readonly ElasticsearchClient _client = client;

    public async Task<SearchOutput<FullAddress>> SearchAsync(string searchAddress, int count, CancellationToken cancellationToken)
    {
        var searchResponse = await _client.SearchAsync<FullAddress>(
            s => s.Query(q => q
                .MatchPhrase(m => m
                    .Field(f => f.Address)
                    .Query(searchAddress)
                    .Slop(2)))
                .Size(count)
                .Index("fulladdress"),
            cancellationToken);

        if (!searchResponse.IsValidResponse)
        {
            return new SearchOutput<FullAddress>()
            {
                Data = [],
                Took = searchResponse.Took,
            };
        }

        var results = searchResponse.Hits.Select(h => new SearchResult<FullAddress>
        {
            Source = h?.Source,
            Score = h?.Score ?? 0,
        });

        return new SearchOutput<FullAddress>()
        {
            Data = results,
            Took = searchResponse.Took,
        };
    }
}
