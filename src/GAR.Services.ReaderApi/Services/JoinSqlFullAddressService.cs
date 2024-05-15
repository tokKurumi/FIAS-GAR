namespace GAR.Services.ReaderApi.Services;

using System.Diagnostics;
using Npgsql;

public class JoinSqlFullAddressService(
    NpgsqlDataSource dataSource,
    ILogger<JoinSqlFullAddressService> logger)
{
    private readonly NpgsqlDataSource _dataSource = dataSource;
    private readonly ILogger<JoinSqlFullAddressService> _logger = logger;

    public async Task JoinFullAddressAsync(CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        await using var cmd = _dataSource.CreateCommand(@"
            CREATE TABLE IF NOT EXISTS public.""FullAddress"" AS
            WITH RECURSIVE PathParts AS (
                SELECT
                    ""ObjectId"",
                    ""Path"",
                    1 AS part_index,
                    split_part(""Path"", '.', 1) AS part_value,
                    string_to_array(""Path"", '.') AS parts_array
                FROM
                    ""Hierarchy""
                UNION ALL
                SELECT
                    ""ObjectId"",
                    ""Path"",
                    part_index + 1,
                    parts_array[part_index + 1],
                    parts_array
                FROM
                    PathParts
                WHERE
                    part_index < cardinality(parts_array)
            )
            SELECT
                h.""ObjectId"",
                h.""Path"",
                STRING_AGG(COALESCE(obj.""FullName"", ''), ', ' ORDER BY pp.part_index) AS ""Address""
            FROM
                PathParts pp
            JOIN
                ""Hierarchy"" h ON h.""Path"" = pp.""Path""
            LEFT JOIN LATERAL (
                SELECT ""FullName""
                FROM (
                    SELECT ""FullName"", ""ObjectId"" FROM ""AddressObject""
                    UNION ALL
                    SELECT ""FullName"", ""ObjectId"" FROM ""House""
                    UNION ALL
                    SELECT ""FullName"", ""ObjectId"" FROM ""Apartment""
                    UNION ALL
                    SELECT ""FullName"", ""ObjectId"" FROM ""Room""
                    UNION ALL
                    SELECT ""FullName"", ""ObjectId"" FROM ""Stead""
                ) AS sub
                WHERE sub.""ObjectId""::text = pp.part_value
                LIMIT 1
            ) AS obj ON TRUE
            GROUP BY h.""ObjectId"", h.""Path"";
        ");

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        sw.Stop();
        _logger.LogInformation("Full address was joined in {Milliseconds}.", sw.ElapsedMilliseconds);
    }
}
