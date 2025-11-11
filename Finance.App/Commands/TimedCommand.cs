using System;
using System.Diagnostics;

namespace Finance.App.Commands;

public sealed class TimedCommand : ICommand
{
    private readonly ICommand _inner;

    public string Key => _inner.Key;
    public string Name => _inner.Name + " (⏱)";

    public TimedCommand(ICommand inner)
    {
        _inner = inner;
    }

    public void Execute()
    {
        var sw = Stopwatch.StartNew();
        _inner.Execute();
        sw.Stop();
        Console.WriteLine($"⏱ Время выполнения: {sw.ElapsedMilliseconds} мс");
    }
}
