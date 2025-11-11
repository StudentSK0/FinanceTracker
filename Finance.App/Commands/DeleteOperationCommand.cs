using System;
using System.Linq;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class DeleteOperationCommand : ICommand
{
    public string Key => "9";
    public string Name => "Удалить операцию";

    private readonly OperationFacade _ops;

    public DeleteOperationCommand(OperationFacade ops)
    {
        _ops = ops;
    }

    public void Execute()
    {
        var op = ConsoleSelector.Select(_ops.List().ToList(), o => $"{o.Date} {o.Type} {o.Amount}₽");
        _ops.Delete(op.Id);
        Console.WriteLine("Операция удалена.");
    }
}
