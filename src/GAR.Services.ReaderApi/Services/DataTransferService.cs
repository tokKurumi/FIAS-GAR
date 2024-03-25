namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class DataTransferService(
    ILogger<DataTransferService> logger,
    ZipXmlReaderService zipXmlReaderService,
    DataWriterService dataWriterService)
    : IDisposable
{
    private readonly ILogger<DataTransferService> _logger = logger;
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;
    private readonly DataWriterService _dataWriterService = dataWriterService;

    private bool _disposed;

    public async Task InsertObjectsAsync(CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        while (_zipXmlReaderService.CanReadObjects)
        {
            var bucket = _zipXmlReaderService.ReadObjectsAsync();
            var saved = await _dataWriterService.ImportObjects(bucket, cancellationToken);

            _logger.LogInformation("Objects saved from bucket: {Saved}", saved);
        }

        _logger.LogInformation("Objects copying has ended in {Milliseconds}", sw.ElapsedMilliseconds);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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
}
