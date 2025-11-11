using System;
using System.Linq;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class EditOperationCommand : ICommand
{
    public string Key => "8";
    public string Name => "Редактировать операцию";

    private readonly OperationFacade _ops;

    public EditOperationCommand(OperationFacade ops)
    {
        _ops = ops;
    }

    public void Execute()
    {
        var op = ConsoleSelector.Select(_ops.List().ToList(), o => $"{o.Date} {o.Type} {o.Amount}₽");
        Console.Write("Новая сумма: ");
        var amount = decimal.Parse(Console.ReadLine()!);
        Console.Write("Новое описание (можно пусто): ");
        var desc = Console.ReadLine();

        _ops.Update(op.Id, amount, desc);
        Console.WriteLine("Операция обновлена.");
    }
}
