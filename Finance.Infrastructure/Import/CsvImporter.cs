using Finance.Domain;

namespace Finance.Infrastructure.Import;

public sealed class CsvImporter : ImporterBase
{
    public CsvImporter(IAccountRepo a, ICategoryRepo c, IOperationRepo o) : base(a, c, o) { }

    // path — директория, ожидаем accounts.csv, categories.csv, operations.csv
    protected override RootDto Load(string path)
    {
        var dto = new RootDto();
        dto.Accounts.AddRange(CsvHelpers.ReadAccounts(Path.Combine(path, "accounts.csv")));
        dto.Categories.AddRange(CsvHelpers.ReadCategories(Path.Combine(path, "categories.csv")));
        dto.Operations.AddRange(CsvHelpers.ReadOperations(Path.Combine(path, "operations.csv")));
        return dto;
    }
}
