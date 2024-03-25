namespace GAR.Services.ReaderApi.Constrollers;

using Asp.Versioning;
using GAR.Services.ReaderApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/[controller]")]
public class DatabaseController(
    DatabaseInitializerService databaseInitializerService,
    DataTransferService dataTransferService)
    : ControllerBase
{
    private readonly DatabaseInitializerService _databaseInitializerService = databaseInitializerService;
    private readonly DataTransferService _dataTransferService = dataTransferService;

    [HttpPost]
    public async Task<IActionResult> InitializeAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.InitializeAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.DropSchemaAsync(cancellationToken);

        return Ok();
    }

    [HttpPost("objects")]
    public async Task<IActionResult> InsertObjectsAsync(CancellationToken cancellationToken)
    {
        await _dataTransferService.InsertObjectsAsync(cancellationToken);

        return Ok();
    }
}
