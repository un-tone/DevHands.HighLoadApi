using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace DevHands.HighLoadApi.Data;

public class DapperContext(IConfiguration configuration) : IDapperContext
{
    private const string SqlConnectionName = "SqlDb";

    private readonly string _connectionString =
        configuration.GetConnectionString(SqlConnectionName)
        ?? throw new InvalidOperationException($"Not defined {SqlConnectionName} connection string.");

    public DbConnection Connect() => new SqlConnection(_connectionString);
}