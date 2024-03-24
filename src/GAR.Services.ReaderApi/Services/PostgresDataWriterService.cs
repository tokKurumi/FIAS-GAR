namespace GAR.Services.ReaderApi.Services;

using GAR.Services.ReaderApi.Data;

public class PostgresDataWriterService(
     GarDbContext dbContext)
{
    private readonly GarDbContext _dbContext = dbContext;

    public Task BulkInsert<T>(IEnumerable<T> values, string table)
    {
        return Task.CompletedTask;
    }
}
