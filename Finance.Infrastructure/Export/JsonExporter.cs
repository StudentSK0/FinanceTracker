using System.Text.Json;
using Finance.Domain;
using Finance.Domain.Export;

namespace Finance.Infrastructure.Export;

public sealed class JsonExporter : IExportVisitor
{
    private readonly string _folder;

    public JsonExporter(string folder)
    {
        _folder = folder;
        Directory.CreateDirectory(folder);
    }

    public void Visit(
        IEnumerable<BankAccount> accounts,
        IEnumerable<Category> categories,
        IEnumerable<Operation> operations)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        File.WriteAllText(
            Path.Combine(_folder, "accounts.json"),
            JsonSerializer.Serialize(accounts, options)
        );

        File.WriteAllText(
            Path.Combine(_folder, "categories.json"),
            JsonSerializer.Serialize(categories, options)
        );

        File.WriteAllText(
            Path.Combine(_folder, "operations.json"),
            JsonSerializer.Serialize(operations, options)
        );
    }
}
