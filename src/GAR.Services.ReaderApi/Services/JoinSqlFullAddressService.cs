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
            CREATE TEMP TABLE IF NOT EXISTS tmp_Parts AS
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
            SELECT * FROM PathParts;

            CREATE TEMP TABLE IF NOT EXISTS tmp_FullNames AS
            SELECT ""FullName"", ""ObjectId"" FROM ""AddressObject""
            UNION ALL
            SELECT ""FullName"", ""ObjectId"" FROM ""House""
            UNION ALL
            SELECT ""FullName"", ""ObjectId"" FROM ""Apartment""
            UNION ALL
            SELECT ""FullName"", ""ObjectId"" FROM ""Room""
            UNION ALL
            SELECT ""FullName"", ""ObjectId"" FROM ""Stead"";

            CREATE TABLE IF NOT EXISTS public.""FullAddress"" AS
            SELECT
                h.""ObjectId"",
                h.""Path"",
                STRING_AGG(COALESCE(fn.""FullName"", ''), ', ' ORDER BY pp.part_index) AS ""Address""
            FROM
                tmp_Parts pp
            JOIN
                ""Hierarchy"" h ON h.""Path"" = pp.""Path""
            LEFT JOIN tmp_FullNames fn ON fn.""ObjectId""::text = pp.part_value
            GROUP BY h.""ObjectId"", h.""Path"";
        ");

        await cmd.ExecuteNonQueryAsync(cancellationToken);

        sw.Stop();
        _logger.LogInformation("Full address was joined in {Milliseconds}.", sw.ElapsedMilliseconds);
    }
}
