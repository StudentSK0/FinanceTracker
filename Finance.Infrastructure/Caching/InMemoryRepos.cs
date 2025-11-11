using System;
using System.Collections.Generic;
using Finance.Domain;

namespace Finance.Infrastructure.Caching;

// ===== In-memory Accounts =====
public sealed class InMemoryAccountRepo : IAccountRepo
{
    private readonly Dictionary<Guid, BankAccount> _acc = new();

    public void Add(BankAccount acc) => _acc[acc.Id] = acc;
    public BankAccount Get(Guid id) => _acc[id];
    public IEnumerable<BankAccount> List() => _acc.Values;
    public void Update(BankAccount acc) => _acc[acc.Id] = acc;
    public void Remove(Guid id) => _acc.Remove(id);
}

// ===== In-memory Categories =====
public sealed class InMemoryCategoryRepo : ICategoryRepo
{
    private readonly Dictionary<Guid, Category> _cat = new();

    public void Add(Category c) => _cat[c.Id] = c;
    public Category Get(Guid id) => _cat[id];
    public IEnumerable<Category> List() => _cat.Values;
    public void Rename(Guid id, string newName) { var c = _cat[id]; c.Rename(newName); }
    public void Remove(Guid id) => _cat.Remove(id);
}

// ===== In-memory Operations =====
public sealed class InMemoryOperationRepo : IOperationRepo
{
    private readonly Dictionary<Guid, Operation> _ops = new();

    public Operation Get(Guid id) => _ops[id];
    public IEnumerable<Operation> List() => _ops.Values;
    public IEnumerable<Operation> ListByAccount(Guid accountId)
    {
        foreach (var op in _ops.Values)
            if (op.BankAccountId == accountId)
                yield return op;
    }

    public void AddAndApplyBalance(Operation op, BankAccount account)
    {
        account.Apply(op);
        _ops[op.Id] = op;
    }

    public void RemoveAndRevertBalance(Operation op, BankAccount account)
    {
        _ops.Remove(op.Id);
        if (op.Type == OpType.Income) account.SetBalance(account.Balance - op.Amount);
        else account.SetBalance(account.Balance + op.Amount);
    }
}
