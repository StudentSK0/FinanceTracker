using System;
using System.Collections.Generic;

namespace Finance.Domain;

public sealed class OperationFacade
{
    private readonly IOperationRepo _ops;
    private readonly IAccountRepo _accounts;

    public OperationFacade(IOperationRepo ops, IAccountRepo accounts)
    {
        _ops = ops;
        _accounts = accounts;
    }

    public Operation Create(OpType type, Guid accountId, decimal amount, DateOnly date,
        string? desc = null, Guid? categoryId = null)
    {
        var op = DomainFactory.Operation(type, accountId, amount, date, desc, categoryId);
        var acc = _accounts.Get(accountId);

        _ops.AddAndApplyBalance(op, acc);
        _accounts.Update(acc);
        return op;
    }

    public void Update(Guid id, decimal newAmount, string? newDescription)
    {
        var old = _ops.Get(id);
        Delete(id);
        Create(old.Type, old.BankAccountId, newAmount, old.Date, newDescription, old.CategoryId);
    }

    public void Delete(Guid id)
    {
        var old = _ops.Get(id);
        var acc = _accounts.Get(old.BankAccountId);

        _ops.RemoveAndRevertBalance(old, acc);
        _accounts.Update(acc);
    }

    public IEnumerable<Operation> List() => _ops.List();
}
