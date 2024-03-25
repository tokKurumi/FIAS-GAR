namespace GAR.Services.ReaderApi.Services;

using Npgsql;
using PostgreSQLCopyHelper;

public class DataWriterService(
    NpgsqlDataSource dataSource)
{
    private readonly NpgsqlDataSource _dataSource = dataSource;

    public async Task<ulong> ImportAsync<T>(IPostgreSQLCopyHelper<T> copyHelper, IAsyncEnumerable<T> values, CancellationToken cancellationToken)
    {
        using var connection = _dataSource.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var saved = await copyHelper.SaveAllAsync(connection, values, cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return saved;
    }
}
