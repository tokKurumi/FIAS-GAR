namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

public class DatabaseInitializerService(
    NpgsqlDataSource dataSource,
    ILogger<DatabaseInitializerService> logger)
    : BackgroundService
{
    private readonly NpgsqlDataSource _dataSource = dataSource;
    private readonly ILogger<DatabaseInitializerService> _logger = logger;

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
            _dataSource.Dispose();
        }

        _disposed = true;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cw = Stopwatch.StartNew();

        await CreateSchemaAsync(stoppingToken);
        await CreateAddressesTableAsync(stoppingToken);
        await CreateApartmentsTableAsync(stoppingToken);
        await CreateHierarchiesTableAsync(stoppingToken);
        await CreateHousesTableAsync(stoppingToken);
        await CreateRoomsTableAsync(stoppingToken);
        await CreateSteadsTableAsync(stoppingToken);

        _logger.LogInformation("Database initialization has ended in {Milliseconds}ms", cw.ElapsedMilliseconds);
    }

    private async Task CreateSchemaAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand(@"
            CREATE SCHEMA IF NOT EXISTS public;
        ");

        await cmd.ExecuteNonQueryAsync(cancellationToken);
        _logger.LogInformation("Schema was created.");
    }

    private async Task CreateAddressesTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Addresses" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" integer NOT NULL,
                "TypeName" text NOT NULL,
                "Name" text NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateApartmentsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Apartments" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" integer NOT NULL,
                "Number" text NOT NULL,
                "ApartType" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateHierarchiesTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Hierarchies" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" text NOT NULL,
                "path" text NOT NULL
           );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateHousesTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Houses" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" integer NOT NULL,
                "HouseNum" text NOT NULL,
                "HouseType" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateRoomsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Rooms" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" integer NOT NULL,
                "Number" text NOT NULL,
                "RoomType" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateSteadsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand("""
            CREATE TABLE IF NOT EXISTS public."Steads" (
                "Id" SERIAL PRIMARY KEY,
                "ObjectId" integer NOT NULL,
                "Number" text NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
