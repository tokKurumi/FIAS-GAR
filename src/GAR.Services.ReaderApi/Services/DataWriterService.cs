namespace GAR.Services.ReaderApi.Services;

using GAR.Services.ReaderApi.Data;
using GAR.Services.ReaderApi.Models;
using Npgsql;

public class DataWriterService(
    NpgsqlDataSource dataSource,
    DataMapHelper dataMapHelper)
{
    private readonly NpgsqlDataSource _dataSource = dataSource;
    private readonly DataMapHelper _dataMapHelper = dataMapHelper;

    public async Task<ulong> ImportObjects(IAsyncEnumerable<AddressObject> addressObjects, CancellationToken cancellationToken = default)
    {
        using var connection = _dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var saved = await _dataMapHelper.Addresses.SaveAllAsync(connection, addressObjects, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return saved;
    }
}
