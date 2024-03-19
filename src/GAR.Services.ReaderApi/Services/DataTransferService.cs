namespace GAR.Services.ReaderApi.Services;

using System.Threading;
using System.Threading.Tasks;

public class DataTransferService(
    ZipXmlReaderService zipXmlReaderService)
    : BackgroundService
{
    private readonly ZipXmlReaderService _zipXmlReaderService = zipXmlReaderService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
                await Console.Out.WriteLineAsync(apartment.Number.ToString());
            }

            await Console.Out.WriteLineAsync("<=== END OF BUCKET ===>");
        }
    }
}
