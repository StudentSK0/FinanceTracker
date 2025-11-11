using System.Data;

namespace Finance.Infrastructure.Persistence;

public static class DbSchemaInitializer
{
    public static void EnsureCreated(DbConnectionFactory factory)
    {
        using var conn = factory.Get();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Accounts (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Balance REAL NOT NULL
);
CREATE TABLE IF NOT EXISTS Categories (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Type TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Operations (
    Id TEXT PRIMARY KEY,
    Type TEXT NOT NULL,
    BankAccountId TEXT NOT NULL,
    Amount REAL NOT NULL,
    Date TEXT NOT NULL,
    Description TEXT,
    CategoryId TEXT
);";
        cmd.ExecuteNonQuery();
    }
}
