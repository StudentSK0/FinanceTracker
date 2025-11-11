using System;
using System.Collections.Generic;

namespace Finance.Domain;

public sealed class AnalyticsFacade
{
    private readonly IOperationRepo _ops;
    private readonly ICategoryRepo _cats;

    public AnalyticsFacade(IOperationRepo ops, ICategoryRepo cats)
    {
        _ops = ops;
        _cats = cats;
    }

    public (decimal income, decimal expense, decimal delta) IncomeVsExpense(Period p)
    {
        decimal inc = 0m, exp = 0m;

        foreach (var op in _ops.List())
        {
            if (op.Date < p.Start || op.Date > p.End) continue;
            if (op.Type == OpType.Income) inc += op.Amount;
            else exp += op.Amount;
        }

        return (inc, exp, inc - exp);
    }

    public Dictionary<string, decimal> GroupedByCategory(Period p)
    {
        var dict = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var names = new Dictionary<Guid, string>();

        foreach (var c in _cats.List())
            names[c.Id] = c.Name;

        foreach (var op in _ops.List())
        {
            if (op.Date < p.Start || op.Date > p.End) continue;
            if (op.CategoryId is null) continue;
            if (!names.TryGetValue(op.CategoryId.Value, out var name)) continue;

            var signed = op.Type == OpType.Income ? op.Amount : -op.Amount;
            dict[name] = dict.TryGetValue(name, out var cur) ? cur + signed : signed;
        }

        return dict;
    }
}
