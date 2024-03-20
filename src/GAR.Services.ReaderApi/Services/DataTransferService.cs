namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class DataTransferService(
    ZipXmlReaderService zipXmlReaderService)
    : BackgroundService
{
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;
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
        var sw = Stopwatch.StartNew();

        _zipXmlReaderService.StartReader();

        while (_zipXmlReaderService.CanReadObjects)
        {
            var bucket = _zipXmlReaderService.ReadObjectsAsync();
            await foreach (var addressObject in bucket)
            {
            }
        }

        while (_zipXmlReaderService.CanReadHouses)
        {
            var bucket = _zipXmlReaderService.ReadHousesAsync();
            await foreach (var house in bucket)
            {
            }
        }

        while (_zipXmlReaderService.CanReadApartments)
        {
            var bucket = _zipXmlReaderService.ReadApartmentsAsync();
            await foreach (var apartment in bucket)
            {
            }
        }

        while (_zipXmlReaderService.CanReadRooms)
        {
            var bucket = _zipXmlReaderService.ReadRoomsAsync();
            await foreach (var room in bucket)
            {
            }
        }

        while (_zipXmlReaderService.CanReadSteads)
        {
            var bucket = _zipXmlReaderService.ReadSteadsAsync();
            await foreach (var stead in bucket)
            {
            }
        }

        sw.Stop();
        await Console.Out.WriteLineAsync($"Done in {sw.ElapsedMilliseconds}");
    }
}
