using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class TodayAnalyticsCommand : ICommand
{
    public string Key => "6";
    public string Name => "Аналитика за сегодня";

    private readonly AnalyticsFacade _analytics;

    public TodayAnalyticsCommand(AnalyticsFacade analytics)
    {
        _analytics = analytics;
    }

    public void Execute()
    {
        var d = DateOnly.FromDateTime(DateTime.Today);
        var (inc, exp, delta) = _analytics.IncomeVsExpense(new Period(d, d));
        Console.WriteLine($"Доход: {inc}, Расход: {exp}, Разница: {delta}");
    }
}
