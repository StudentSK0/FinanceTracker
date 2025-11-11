using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class GroupByCategoryCommand : ICommand
{
    public string Key => "11";
    public string Name => "Группировка по категориям за период";

    private readonly AnalyticsFacade _analytics;

    public GroupByCategoryCommand(AnalyticsFacade analytics)
    {
        _analytics = analytics;
    }

    public void Execute()
    {
        Console.Write("Дата начала (yyyy-mm-dd): ");
        var ds = DateOnly.Parse(Console.ReadLine()!);

        Console.Write("Дата конца (yyyy-mm-dd): ");
        var de = DateOnly.Parse(Console.ReadLine()!);

        var map = _analytics.GroupedByCategory(new Period(ds, de));

        Console.WriteLine("\nКатегория — Сумма:");
        foreach (var (category, sum) in map)
            Console.WriteLine($"{category}: {sum}₽");
    }
}
