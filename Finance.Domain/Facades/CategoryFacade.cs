using System;
using System.Collections.Generic;

namespace Finance.Domain;

public sealed class CategoryFacade
{
    private readonly ICategoryRepo _cats;

    public CategoryFacade(ICategoryRepo cats) => _cats = cats;

    public Category Create(string name, OpType type)
    {
        var c = DomainFactory.Category(name, type);
        _cats.Add(c);
        return c;
    }

    public void Rename(Guid id, string newName) => _cats.Rename(id, newName);
    public void Delete(Guid id) => _cats.Remove(id);
    public IEnumerable<Category> List() => _cats.List();
}
