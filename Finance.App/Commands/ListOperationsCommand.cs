using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class ListOperationsCommand : ICommand
{
    public string Key => "5";
    public string Name => "Показать операции";

    private readonly OperationFacade _ops;

    public ListOperationsCommand(OperationFacade ops)
    {
        _ops = ops;
    }

    public void Execute()
    {
        foreach (var o in _ops.List())
            Console.WriteLine($"{o.Id} | {o.Date}: {o.Type} {o.Amount}₽ (Счёт {o.BankAccountId})");
    }
}
