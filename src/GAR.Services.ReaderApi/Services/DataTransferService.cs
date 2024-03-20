namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class DataTransferService(
    ZipXmlReaderService zipXmlReaderService)
    : BackgroundService
{
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;

    public override void Dispose()
    {
        _zipXmlReaderService.Dispose();
        GC.SuppressFinalize(this);

        base.Dispose();
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
                await Console.Out.WriteLineAsync(addressObject.Name);
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();

        while (_zipXmlReaderService.CanReadHouses)
        {
            var bucket = _zipXmlReaderService.ReadHousesAsync();
            await foreach (var house in bucket)
            {
                await Console.Out.WriteLineAsync(house.HouseNum);
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();

        while (_zipXmlReaderService.CanReadApartments)
        {
            var bucket = _zipXmlReaderService.ReadApartmentsAsync();
            await foreach (var apartment in bucket)
            {
                await Console.Out.WriteLineAsync(apartment.Number);
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();

        while (_zipXmlReaderService.CanReadRooms)
        {
            var bucket = _zipXmlReaderService.ReadRoomsAsync();
            await foreach (var room in bucket)
            {
                await Console.Out.WriteLineAsync(room.Number);
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }

        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();
        await Console.Out.WriteLineAsync();

        while (_zipXmlReaderService.CanReadSteads)
        {
            var bucket = _zipXmlReaderService.ReadSteadsAsync();
            await foreach (var stead in bucket)
            {
                await Console.Out.WriteLineAsync(stead.Number.ToString());
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }

        sw.Stop();
        await Console.Out.WriteLineAsync($"Done in {sw.ElapsedMilliseconds}");
    }
}
