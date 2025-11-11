using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Finance.Domain;

namespace Finance.Infrastructure.Persistence;

// ============== BANK ACCOUNTS ==============
public sealed class DbAccountRepo : IAccountRepo
{
    private readonly DbConnectionFactory _db;
    public DbAccountRepo(DbConnectionFactory db) => _db = db;

    public void Add(BankAccount acc)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Accounts_Insert, new { acc.Id, acc.Name, acc.Balance });
    }

    public BankAccount Get(Guid id)
    {
        using var conn = _db.Get();
        var row = conn.QuerySingle<(string Id, string Name, decimal Balance)>(SqlQueries.Accounts_SelectOne, new { id });
        return new BankAccount(Guid.Parse(row.Id), row.Name, row.Balance);
    }

    public IEnumerable<BankAccount> List()
    {
        using var conn = _db.Get();
        foreach (var r in conn.Query<(string Id, string Name, decimal Balance)>(SqlQueries.Accounts_SelectAll))
            yield return new BankAccount(Guid.Parse(r.Id), r.Name, r.Balance);
    }

    public void Update(BankAccount acc)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Accounts_UpdateBalance, new { acc.Id, acc.Balance });
    }

    public void Remove(Guid id)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Accounts_Delete, new { id });
    }
}

// ============== CATEGORIES ==============
public sealed class DbCategoryRepo : ICategoryRepo
{
    private readonly DbConnectionFactory _db;
    public DbCategoryRepo(DbConnectionFactory db) => _db = db;

    public void Add(Category c)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Categories_Insert, new { c.Id, c.Name, c.Type });
    }

    public Category Get(Guid id)
    {
        using var conn = _db.Get();
        var row = conn.QuerySingle<(string Id, string Name, OpType Type)>(SqlQueries.Categories_SelectOne, new { id });
        return new Category(Guid.Parse(row.Id), row.Name, row.Type);
    }

    public IEnumerable<Category> List()
    {
        using var conn = _db.Get();
        foreach (var r in conn.Query<(string Id, string Name, OpType Type)>(SqlQueries.Categories_SelectAll))
            yield return new Category(Guid.Parse(r.Id), r.Name, r.Type);
    }

    public void Rename(Guid id, string newName)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Categories_Rename, new { newName, id });
    }

    public void Remove(Guid id)
    {
        using var conn = _db.Get();
        conn.Execute(SqlQueries.Categories_Delete, new { id });
    }
}
