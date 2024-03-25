namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GAR.Services.ReaderApi.Models;
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
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{AddressObject.TableEntityName}" (
                "{nameof(AddressObject.Id)}" SERIAL PRIMARY KEY,
                "{nameof(AddressObject.ObjectId)}" integer NOT NULL,
                "{nameof(AddressObject.TypeName)}" text NOT NULL,
                "{nameof(AddressObject.Name)}" text NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateApartmentsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{Apartment.TableEntityName}" (
                "{nameof(Apartment.Id)}" SERIAL PRIMARY KEY,
                "{nameof(Apartment.ObjectId)}" integer NOT NULL,
                "{nameof(Apartment.Number)}" text NOT NULL,
                "{nameof(Apartment.ApartType)}" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateHierarchiesTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{Hierarchy.TableEntityName}" (
                "{nameof(Hierarchy.Id)}" SERIAL PRIMARY KEY,
                "{nameof(Hierarchy.ObjectId)}" text NOT NULL,
                "{nameof(Hierarchy.Path)}" text NOT NULL
           );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateHousesTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{House.TableEntityName}" (
                "{nameof(House.Id)}" SERIAL PRIMARY KEY,
                "{nameof(House.ObjectId)}" integer NOT NULL,
                "{nameof(House.HouseNum)}" text NOT NULL,
                "{nameof(House.HouseType)}" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateRoomsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{Room.TableEntityName}" (
                "{nameof(Room.Id)}" SERIAL PRIMARY KEY,
                "{nameof(Room.ObjectId)}" integer NOT NULL,
                "{nameof(Room.Number)}" text NOT NULL,
                "{nameof(Room.RoomType)}" integer NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task CreateSteadsTableAsync(CancellationToken cancellationToken = default)
    {
        await using var cmd = _dataSource.CreateCommand($"""
            CREATE TABLE IF NOT EXISTS public."{Stead.TableEntityName}" (
                "{nameof(Stead.Id)}" SERIAL PRIMARY KEY,
                "{nameof(Stead.ObjectId)}" integer NOT NULL,
                "{nameof(Stead.Number)}" text NOT NULL
            );
        """);

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
