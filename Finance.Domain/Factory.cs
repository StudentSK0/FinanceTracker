using System;

namespace Finance.Domain;

public static class DomainFactory
{
    public static Category Category(string name, OpType type)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name must be non-empty.");
        return new Category(Guid.NewGuid(), name.Trim(), type);
    }

    public static BankAccount BankAccount(string name, decimal openingBalance = 0m)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name must be non-empty.");
        if (openingBalance < 0m)
            throw new ArgumentException("Opening balance cannot be negative.");
        return new BankAccount(Guid.NewGuid(), name.Trim(), openingBalance);
    }

    public static Operation Operation(OpType type, Guid accountId, decimal amount,
        DateOnly date, string? description = null, Guid? categoryId = null)
    {
        if (amount <= 0m)
            throw new ArgumentException("Amount must be positive.");
        return new Operation(Guid.NewGuid(), type, accountId, amount, date, description, categoryId);
    }
}
