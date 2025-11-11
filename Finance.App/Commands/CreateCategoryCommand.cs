using System;
using Finance.Domain;

namespace Finance.App.Commands;

public sealed class CreateCategoryCommand : ICommand
{
    public string Key => "2";
    public string Name => "Создать категорию";

    private readonly CategoryFacade _categories;

    public CreateCategoryCommand(CategoryFacade categories)
    {
        _categories = categories;
    }

    public void Execute()
    {
        Console.Write("Название категории: ");
        var name = Console.ReadLine()!;

        Console.Write("Тип (income/expense): ");
        var raw = Console.ReadLine()!.Trim().ToLower();
        var type = raw == "income" ? OpType.Income : OpType.Expense;

        _categories.Create(name, type);
        Console.WriteLine("Категория создана.");
    }
}
