using Finance.Domain;
using Finance.Domain.Facades;
using Finance.App.Commands;
using Finance.Infrastructure.Caching;
using Finance.Infrastructure.Persistence;
using Finance.Infrastructure.Export;
using Finance.Infrastructure.Import;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// -------------------- ИНИЦИАЛИЗАЦИЯ ИНФРАСТРУКТУРЫ --------------------
services.AddSingleton<DbConnectionFactory>();

// Репозитории c кэшем поверх SQLite
services.AddSingleton<IAccountRepo>(sp =>
    new CachedAccountRepo(
        new DbAccountRepo(sp.GetRequiredService<DbConnectionFactory>())
    ));

services.AddSingleton<ICategoryRepo>(sp =>
    new CachedCategoryRepo(
        new DbCategoryRepo(sp.GetRequiredService<DbConnectionFactory>())
    ));

services.AddSingleton<DbAccountRepo>(); // нужно для DbOperationRepo
services.AddSingleton<IOperationRepo>(sp =>
    new DbOperationRepo(
        sp.GetRequiredService<DbConnectionFactory>(),
        sp.GetRequiredService<DbAccountRepo>()
    ));

// -------------------- ФАСАДЫ --------------------
services.AddSingleton<AccountFacade>();
services.AddSingleton<CategoryFacade>();
services.AddSingleton<OperationFacade>();
services.AddSingleton<AnalyticsFacade>();
services.AddSingleton<ExportFacade>();
services.AddSingleton<ImportFacade>();

// -------------------- ИМПОРТЁРЫ --------------------
services.AddSingleton<JsonImporter>();
services.AddSingleton<YamlImporter>();
services.AddSingleton<CsvImporter>();

var provider = services.BuildServiceProvider();

// Создаём таблицы если их нет
DbSchemaInitializer.EnsureCreated(provider.GetRequiredService<DbConnectionFactory>());

// -------------------- РЕГИСТРАЦИЯ КОМАНД --------------------
var commands = new List<ICommand>
{
    new TimedCommand(new CreateAccountCommand(provider.GetRequiredService<AccountFacade>())),
    new TimedCommand(new CreateCategoryCommand(provider.GetRequiredService<CategoryFacade>())),
    new TimedCommand(new AddOperationCommand(
        provider.GetRequiredService<AccountFacade>(),
        provider.GetRequiredService<OperationFacade>()
    )),
    new ListAccountsCommand(provider.GetRequiredService<AccountFacade>()),
    new ListOperationsCommand(provider.GetRequiredService<OperationFacade>()),
    new TimedCommand(new TodayAnalyticsCommand(provider.GetRequiredService<AnalyticsFacade>())),
    new TimedCommand(new ExportCommand(provider.GetRequiredService<ExportFacade>())),
    new TimedCommand(new EditOperationCommand(provider.GetRequiredService<OperationFacade>())),
    new TimedCommand(new DeleteOperationCommand(provider.GetRequiredService<OperationFacade>())),
    new TimedCommand(new RecalcBalanceCommand(provider.GetRequiredService<AccountFacade>())),
    new TimedCommand(new GroupByCategoryCommand(provider.GetRequiredService<AnalyticsFacade>())),
    new TimedCommand(new ImportCommand(
        provider.GetRequiredService<ImportFacade>(),
        provider.GetRequiredService<JsonImporter>(),
        provider.GetRequiredService<YamlImporter>(),
        provider.GetRequiredService<CsvImporter>()
    ))
};

// -------------------- ОСНОВНОЕ МЕНЮ --------------------
while (true)
{
    Console.WriteLine("\n=== МЕНЮ ===");
    foreach (var cmd in commands)
        Console.WriteLine($"{cmd.Key}. {cmd.Name}");

    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");
    var key = Console.ReadLine();

    if (key == "0") return;

    var command = commands.FirstOrDefault(c => c.Key == key);
    if (command is null)
    {
        Console.WriteLine("Некорректный выбор");
        continue;
    }

    Console.Clear();
    command.Execute();
    Console.WriteLine("\nНажмите Enter для продолжения...");
    Console.ReadLine();
}
