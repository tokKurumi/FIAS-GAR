namespace GAR.Services.ReaderApi.Constrollers;

using Asp.Versioning;
using GAR.Services.SearchApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/[controller]")]
public class ElasticController(SearchService searchService)
    : ControllerBase
{
    private readonly SearchService _searchService = searchService;

    [HttpGet("{address}")]
    public async Task<IActionResult> SearchAsync([FromRoute] string address, CancellationToken cancellationToken)
    {
        var search = await _searchService.SearchAsync(address, 10, cancellationToken);

        return search.Data.Any() ? Ok(search) : NotFound();
    }
}
