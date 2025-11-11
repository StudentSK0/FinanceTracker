using System;
using System.IO;
using Finance.Domain.Facades;
using Finance.Infrastructure.Import;

namespace Finance.App.Commands;

public sealed class ImportCommand : ICommand
{
    public string Key => "12";
    public string Name => "Импорт данных";

    private readonly ImportFacade _import;
    private readonly JsonImporter _json;
    private readonly YamlImporter _yaml;
    private readonly CsvImporter _csv;

    public ImportCommand(ImportFacade fac, JsonImporter j, YamlImporter y, CsvImporter c)
    {
        _import = fac;
        _json = j;
        _yaml = y;
        _csv = c;
    }

    public void Execute()
    {
        Console.Write("Введите путь к файлу (json/yaml) или к папке с csv: ");
        var path = Console.ReadLine()!.Trim();

        if (path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            _import.Import(_json, path);

        else if (path.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
            _import.Import(_yaml, path);

        else if (Directory.Exists(path)) // CSV folder mode
            _import.Import(_csv, path);

        else
        {
            Console.WriteLine("❌ Неверный формат. Ожидается .json / .yaml / папка CSV");
            return;
        }

        Console.WriteLine("✅ Импорт успешно выполнен");
    }
}
