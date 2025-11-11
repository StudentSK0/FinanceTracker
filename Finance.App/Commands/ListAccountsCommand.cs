using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class ListAccountsCommand : ICommand
{
    public string Key => "4";
    public string Name => "Показать счета";

    private readonly AccountFacade _accounts;

    public ListAccountsCommand(AccountFacade accounts)
    {
        _accounts = accounts;
    }

    public void Execute()
    {
        foreach (var a in _accounts.List())
            Console.WriteLine($"{a.Id} — {a.Name}: {a.Balance}₽");
    }
}
