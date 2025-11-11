using System;
using System.Collections.Generic;

namespace Finance.Domain;

public sealed class AccountFacade
{
    private readonly IAccountRepo _accounts;
    private readonly IOperationRepo _ops;

    public AccountFacade(IAccountRepo accounts, IOperationRepo ops)
    {
        _accounts = accounts;
        _ops = ops;
    }

    public BankAccount Create(string name, decimal openingBalance = 0m)
    {
        var acc = DomainFactory.BankAccount(name, openingBalance);
        _accounts.Add(acc);
        return acc;
    }

    public void Rename(Guid id, string newName)
    {
        var acc = _accounts.Get(id);
        acc.Rename(newName);
        _accounts.Update(acc);
    }

    public void Delete(Guid id) => _accounts.Remove(id);

    public decimal RecalcBalance(Guid accountId)
    {
        var acc = _accounts.Get(accountId);
        decimal total = 0m;

        foreach (var op in _ops.ListByAccount(accountId))
            total += op.Type == OpType.Income ? op.Amount : -op.Amount;

        acc.SetBalance(total);
        _accounts.Update(acc);
        return total;
    }

    public IEnumerable<BankAccount> List() => _accounts.List();
}
