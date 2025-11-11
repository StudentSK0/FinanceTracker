using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class CreateAccountCommand : ICommand
{
    public string Key => "1";
    public string Name => "Создать счёт";

    private readonly AccountFacade _accounts;

    public CreateAccountCommand(AccountFacade accounts)
    {
        _accounts = accounts;
    }

    public void Execute()
    {
        Console.Write("Название счёта: ");
        var name = Console.ReadLine()!;

        Console.Write("Начальный баланс: ");
        var balance = decimal.Parse(Console.ReadLine()!);

        _accounts.Create(name, balance);
        Console.WriteLine("Счёт создан.");
    }
}
