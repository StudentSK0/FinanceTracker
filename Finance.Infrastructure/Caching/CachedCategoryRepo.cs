using System;
using System.Collections.Generic;
using System.Data;
using Finance.Domain; // ← ВАЖНО: для ICategoryRepo, Category

namespace Finance.Infrastructure.Caching;

public sealed class CachedCategoryRepo : ICategoryRepo
{
    private readonly ICategoryRepo _db;
    private readonly Dictionary<Guid, Category> _cache = new();

    public CachedCategoryRepo(ICategoryRepo db)
    {
        _db = db;
        foreach (var c in db.List())
            _cache[c.Id] = c;
    }

    public IEnumerable<Category> List() => _cache.Values;

    public Category Get(Guid id) => _cache[id];

    public void Add(Category c)
    {
        _db.Add(c);
        _cache[c.Id] = c;
    }

    public void Rename(Guid id, string newName)
    {
        _db.Rename(id, newName);
        _cache[id].Rename(newName);
    }

    public void Remove(Guid id)
    {
        _db.Remove(id);
        _cache.Remove(id);
    }
}
