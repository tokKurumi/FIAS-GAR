namespace GAR.Services.ReaderApi.Services;

using GAR.Services.ReaderApi.Models;

public class PostgresDataWriterService
{
    public async Task ImportObjects(IAsyncEnumerable<AddressObject> addressObjects, CancellationToken cancellationToken = default)
    {
    }
}
