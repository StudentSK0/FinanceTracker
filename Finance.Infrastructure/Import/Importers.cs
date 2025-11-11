using Finance.Domain;
using Finance.Domain.Import;

namespace Finance.Infrastructure.Import;

// База: общий шаблонный метод; реализация контракта IImporter
public abstract class ImporterBase : IImporter
{
    protected readonly IAccountRepo Accounts;
    protected readonly ICategoryRepo Categories;
    protected readonly IOperationRepo Operations;

    protected ImporterBase(IAccountRepo a, ICategoryRepo c, IOperationRepo o)
    {
        Accounts = a;
        Categories = c;
        Operations = o;
    }

    public void Import(string path)
    {
        var dto = Load(path);     // различается (файл JSON/YAML или папка CSV)
        Upsert(dto);              // одинаковая логика записи в репозитории
    }

    protected abstract RootDto Load(string path);

    protected virtual void Upsert(RootDto dto)
    {
        foreach (var a in dto.Accounts)
            Accounts.Add(DomainFactory.BankAccount(a.Name, a.Balance));

        foreach (var c in dto.Categories)
            Categories.Add(DomainFactory.Category(c.Name, Enum.Parse<OpType>(c.Type, true)));

        foreach (var o in dto.Operations)
        {
            var op = DomainFactory.Operation(
                Enum.Parse<OpType>(o.Type, true),
                Guid.Parse(o.BankAccountId), o.Amount,
                DateOnly.Parse(o.Date),
                o.Description,
                string.IsNullOrWhiteSpace(o.CategoryId) ? null : Guid.Parse(o.CategoryId));

            var acc = Accounts.Get(op.BankAccountId);
            Operations.AddAndApplyBalance(op, acc);
            Accounts.Update(acc);
        }
    }
}
