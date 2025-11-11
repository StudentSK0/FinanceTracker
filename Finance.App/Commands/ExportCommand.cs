using System;
using Finance.Domain;
using Finance.Infrastructure.Export;

namespace Finance.App.Commands;

public sealed class ExportCommand : ICommand
{
    public string Key => "7";
    public string Name => "Экспорт в CSV и JSON";

    private readonly ExportFacade _exporter;

    public ExportCommand(ExportFacade exporter)
    {
        _exporter = exporter;
    }

    public void Execute()
    {
        _exporter.Accept(new JsonExporter("out/json"));
        _exporter.Accept(new CsvExporter("out/csv"));
        Console.WriteLine("Экспорт выполнен.");
    }
}
