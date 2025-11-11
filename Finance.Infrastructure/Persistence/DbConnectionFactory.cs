using System.Data;
using Microsoft.Data.Sqlite;

namespace Finance.Infrastructure.Persistence;

public sealed class DbConnectionFactory
{
    private readonly string _connectionString = "Data Source=finance.db";

    public IDbConnection Get()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }
}
