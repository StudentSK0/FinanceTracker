using System;
using System.Collections.Generic;
using Dapper;
using Finance.Domain;

namespace Finance.Infrastructure.Persistence;

public sealed class DbOperationRepo : IOperationRepo
{
    private readonly DbConnectionFactory _db;
    private readonly DbAccountRepo _accounts;

    public DbOperationRepo(DbConnectionFactory db, DbAccountRepo accounts)
    {
        _db = db;
        _accounts = accounts;
    }

    // =====================================
    // Основная логика (с транзакциями)
    // =====================================
    public void AddAndApplyBalance(Operation op, BankAccount account)
    {
        using var conn = _db.Get();
        using var tx = conn.BeginTransaction();

        // 1) применяем операцию к бизнес-сущности
        account.Apply(op);

        // 2) сохраняем операцию в БД
        conn.Execute(SqlQueries.Operations_Insert, new
        {
            op.Id,
            Type = op.Type.ToString(),
            op.BankAccountId,
            op.Amount,
            Date = op.Date.ToString("yyyy-MM-dd"),
            op.Description,
            CategoryId = op.CategoryId?.ToString()
        }, tx);

        // 3) обновляем баланс счёта
        conn.Execute(SqlQueries.Accounts_UpdateBalance,
            new { Id = account.Id, Balance = account.Balance }, tx);

        tx.Commit();
    }

    public void RemoveAndRevertBalance(Operation op, BankAccount account)
    {
        using var conn = _db.Get();
        using var tx = conn.BeginTransaction();

        // 1) отменяем действие операции в доменной модели
        account.Revert(op);

        // 2) удаляем операцию
        conn.Execute(SqlQueries.Operations_Delete, new { Id = op.Id }, tx);

        // 3) обновляем баланс счёта
        conn.Execute(SqlQueries.Accounts_UpdateBalance,
            new { Id = account.Id, Balance = account.Balance }, tx);

        tx.Commit();
    }

    // =====================================
    // Чтение
    // =====================================
    public Operation Get(Guid id)
    {
        using var conn = _db.Get();
        var row = conn.QuerySingle<dynamic>(SqlQueries.Operations_SelectOne, new { Id = id });

        return new Operation(
            Guid.Parse((string)row.Id),
            Enum.Parse<OpType>((string)row.Type),
            Guid.Parse((string)row.BankAccountId),
            (decimal)row.Amount,
            DateOnly.Parse((string)row.Date),
            (string?)row.Description,
            row.CategoryId is null ? null : Guid.Parse((string)row.CategoryId)
        );
    }

    public IEnumerable<Operation> List()
    {
        using var conn = _db.Get();
        foreach (var row in conn.Query<dynamic>(SqlQueries.Operations_SelectAll))
        {
            yield return new Operation(
                Guid.Parse((string)row.Id),
                Enum.Parse<OpType>((string)row.Type),
                Guid.Parse((string)row.BankAccountId),
                (decimal)row.Amount,
                DateOnly.Parse((string)row.Date),
                (string?)row.Description,
                row.CategoryId is null ? null : Guid.Parse((string)row.CategoryId)
            );
        }
    }

    public IEnumerable<Operation> ListByAccount(Guid accountId)
    {
        using var conn = _db.Get();
        foreach (var row in conn.Query<dynamic>(SqlQueries.Operations_SelectByAccount, new { AccountId = accountId }))
        {
            yield return new Operation(
                Guid.Parse((string)row.Id),
                Enum.Parse<OpType>((string)row.Type),
                Guid.Parse((string)row.BankAccountId),
                (decimal)row.Amount,
                DateOnly.Parse((string)row.Date),
                (string?)row.Description,
                row.CategoryId is null ? null : Guid.Parse((string)row.CategoryId)
            );
        }
    }
}
