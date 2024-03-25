namespace GAR.Services.ReaderApi.Constrollers;

using Asp.Versioning;
using GAR.Services.ReaderApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/[controller]")]
public class DatabaseController(
    DatabaseInitializerService databaseInitializerService)
    : ControllerBase
{
    private readonly DatabaseInitializerService _databaseInitializerService = databaseInitializerService;

    [HttpPost("initialize")]
    public async Task<IActionResult> InitializeAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.InitializeAsync(cancellationToken);

        return Ok();
    }

    [HttpPost("insertObjects")]
    public async Task<IActionResult> InsertObjectsAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.InitializeAsync(cancellationToken);

        return Ok();
    }
}
