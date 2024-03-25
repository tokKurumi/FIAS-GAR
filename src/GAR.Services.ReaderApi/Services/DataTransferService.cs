namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class DataTransferService(
    ILogger<DataTransferService> logger,
    ZipXmlReaderService zipXmlReaderService,
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    private readonly ILogger<DataTransferService> _logger = logger;
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    private bool _disposed;

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

        base.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _zipXmlReaderService.Dispose();
        }

        _disposed = true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var postgresDataWriterService = scope.ServiceProvider.GetRequiredService<DataWriterService>();

        await InsertObjectsAsync(postgresDataWriterService, stoppingToken);
    }

    private async Task InsertObjectsAsync(DataWriterService dataWriterService, CancellationToken cancelationToken = default)
    {
        var sw = Stopwatch.StartNew();

        while (_zipXmlReaderService.CanReadObjects)
        {
            var bucket = _zipXmlReaderService.ReadObjectsAsync();
            await dataWriterService.ImportObjects(bucket, cancelationToken);
        }

        _logger.LogInformation("Done in {Milliseconds}", sw.ElapsedMilliseconds);
    }
}
