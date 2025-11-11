using System;

namespace Finance.Domain;

public enum OpType { Income, Expense }

public sealed class Category
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = "";
    public OpType Type { get; set; }

    // Конструктор для нашей логики
    public Category(Guid id, string name, OpType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    // Конструктор для Dapper
    public Category() { }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Название категории не может быть пустым.");
        Name = newName.Trim();
    }
}

public sealed class BankAccount
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    public BankAccount(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void Rename(string newName) => Name = newName;

    public void SetBalance(decimal balance) => Balance = balance;

    /// <summary>Применяем операцию к балансу (добавление операции)</summary>
    public void Apply(Operation op)
    {
        Balance += op.Type == OpType.Income ? op.Amount : -op.Amount;
    }

    /// <summary>Откатываем операцию (удаление операции)</summary>
    public void Revert(Operation op)
    {
        Balance -= op.Type == OpType.Income ? op.Amount : -op.Amount;
    }
}


public sealed class Operation
{
    public Guid Id { get; set; }
    public OpType Type { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
    public Guid? CategoryId { get; set; }

    // Конструктор для нашей логики
    public Operation(Guid id, OpType type, Guid accountId,
        decimal amount, DateOnly date, string? description = null, Guid? categoryId = null)
    {
        Id = id;
        Type = type;
        BankAccountId = accountId;
        Amount = amount;
        Date = date;
        Description = description;
        CategoryId = categoryId;
    }

    // Конструктор для Dapper
    public Operation() { }
}
