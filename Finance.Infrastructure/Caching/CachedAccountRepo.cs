using System;
using System.Collections.Generic;
using Finance.Domain;

namespace Finance.Infrastructure.Caching;

public sealed class CachedAccountRepo : IAccountRepo
{
    private readonly IAccountRepo _db;
    private readonly Dictionary<Guid, BankAccount> _cache = new();

    public CachedAccountRepo(IAccountRepo db)
    {
        _db = db;
        foreach (var acc in db.List())
            _cache[acc.Id] = acc;
    }

    public IEnumerable<BankAccount> List() => _cache.Values;
    public BankAccount Get(Guid id) => _cache[id];

    public void Add(BankAccount acc)
    {
        _db.Add(acc);
        _cache[acc.Id] = acc;
    }

    public void Update(BankAccount acc)
    {
        _db.Update(acc);
        _cache[acc.Id] = acc;
    }

    public void Remove(Guid id)
    {
        _db.Remove(id);
        _cache.Remove(id);
    }
}
