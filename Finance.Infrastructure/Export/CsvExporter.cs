using System.Globalization;
using Finance.Domain;
using Finance.Domain.Export;

namespace Finance.Infrastructure.Export;

public sealed class CsvExporter : IExportVisitor
{
    private readonly string _folder;

    public CsvExporter(string folder)
    {
        _folder = folder;
        Directory.CreateDirectory(folder);
    }

    public void Visit(
        IEnumerable<BankAccount> accounts,
        IEnumerable<Category> categories,
        IEnumerable<Operation> operations)
    {
        File.WriteAllLines(Path.Combine(_folder, "accounts.csv"),
            new[] { "Name,Balance" }.Concat(
                accounts.Select(a => $"{a.Name},{a.Balance.ToString(CultureInfo.InvariantCulture)}")
            ));

        File.WriteAllLines(Path.Combine(_folder, "categories.csv"),
            new[] { "Name,Type" }.Concat(
                categories.Select(c => $"{c.Name},{c.Type}")
            ));

        File.WriteAllLines(Path.Combine(_folder, "operations.csv"),
            new[] { "Type,BankAccountId,Amount,Date,Description,CategoryId" }.Concat(
                operations.Select(o =>
                    $"{o.Type}," +
                    $"{o.BankAccountId}," +
                    $"{o.Amount.ToString(CultureInfo.InvariantCulture)}," +
                    $"{o.Date:yyyy-MM-dd}," +
                    $"{o.Description}," +
                    $"{o.CategoryId}"
                )
            ));
    }
}
