using System;
using System.Linq;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class AddOperationCommand : ICommand
{
    public string Key => "3";
    public string Name => "Добавить операцию";

    private readonly AccountFacade _accounts;
    private readonly OperationFacade _ops;

    public AddOperationCommand(AccountFacade accounts, OperationFacade ops)
    {
        _accounts = accounts;
        _ops = ops;
    }

    public void Execute()
    {
        var acc = ConsoleSelector.Select(
            _accounts.List().ToList(),
            a => $"{a.Name} ({a.Balance}₽)"
        );

        Console.Write("Тип (income/expense): ");
        var type = Console.ReadLine()!.Trim().ToLower() == "income"
            ? OpType.Income
            : OpType.Expense;

        Console.Write("Сумма: ");
        var amount = decimal.Parse(Console.ReadLine()!);

        _ops.Create(type, acc.Id, amount, DateOnly.FromDateTime(DateTime.Today));
        Console.WriteLine("Операция добавлена.");
    }
}
