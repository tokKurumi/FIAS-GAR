namespace GAR.Services.ReaderApi.Constrollers;

using Asp.Versioning;
using GAR.Services.ReaderApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiVersion(1.0)]
[Route("v{version:apiVersion}/[controller]")]
public class DatabaseController(
    DatabaseInitializerService databaseInitializerService,
    DataTransferService dataTransferService,
    JoinSqlFullAddressService joinSqlFullAddressService)
    : ControllerBase
{
    private readonly DatabaseInitializerService _databaseInitializerService = databaseInitializerService;
    private readonly DataTransferService _dataTransferService = dataTransferService;
    private readonly JoinSqlFullAddressService _joinSqlFullAddressService = joinSqlFullAddressService;

    [HttpPost]
    public async Task<IActionResult> InitializeAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.EnsureCreatedAsync(cancellationToken);
        await _databaseInitializerService.CreateSchemaAsync(cancellationToken);
        await _databaseInitializerService.CreateTablesAsync(cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken)
    {
        await _databaseInitializerService.DropSchemaAsync(cancellationToken);

        return Ok();
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync(CancellationToken cancellationToken)
    {
        await _dataTransferService.ImportAsync(cancellationToken);

        return Ok();
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinAsync(CancellationToken cancellationToken)
    {
        await _joinSqlFullAddressService.JoinFullAddressAsync(cancellationToken);

        return Ok();
    }
}
