using System;
using System.Linq;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class RecalcBalanceCommand : ICommand
{
    public string Key => "10";
    public string Name => "Пересчитать баланс счёта";

    private readonly AccountFacade _accounts;

    public RecalcBalanceCommand(AccountFacade accounts)
    {
        _accounts = accounts;
    }

    public void Execute()
    {
        var acc = ConsoleSelector.Select(_accounts.List().ToList(), a => $"{a.Name} ({a.Balance}₽)");
        var bal = _accounts.RecalcBalance(acc.Id);
        Console.WriteLine($"Баланс пересчитан: {bal}₽");
    }
}
