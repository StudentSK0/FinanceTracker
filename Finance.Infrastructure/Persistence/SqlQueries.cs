namespace Finance.Infrastructure.Persistence;

public static class SqlQueries
{
    // ===== Accounts =====
    public const string Accounts_SelectAll =
        "SELECT Id, Name, Balance FROM Accounts";

    public const string Accounts_SelectOne =
        "SELECT Id, Name, Balance FROM Accounts WHERE Id = @Id";

    public const string Accounts_Insert =
        "INSERT INTO Accounts(Id, Name, Balance) VALUES (@Id, @Name, @Balance)";

    public const string Accounts_Delete =
        "DELETE FROM Accounts WHERE Id = @Id";

    public const string Accounts_UpdateBalance =
        "UPDATE Accounts SET Balance = @Balance WHERE Id = @Id";


    // ===== Categories =====
    public const string Categories_SelectAll =
        "SELECT Id, Name, Type FROM Categories";

    public const string Categories_SelectOne =
        "SELECT Id, Name, Type FROM Categories WHERE Id = @Id";

    public const string Categories_Insert =
        "INSERT INTO Categories(Id, Name, Type) VALUES (@Id, @Name, @Type)";

    public const string Categories_Delete =
        "DELETE FROM Categories WHERE Id = @Id";

    public const string Categories_Rename =
        "UPDATE Categories SET Name = @newName WHERE Id = @Id";


    // ===== Operations =====
    public const string Operations_SelectAll =
        "SELECT Id, Type, BankAccountId, Amount, Date, Description, CategoryId FROM Operations";

    public const string Operations_SelectOne =
        "SELECT Id, Type, BankAccountId, Amount, Date, Description, CategoryId FROM Operations WHERE Id = @Id";

    public const string Operations_SelectByAccount =
        "SELECT Id, Type, BankAccountId, Amount, Date, Description, CategoryId FROM Operations WHERE BankAccountId = @AccountId";

    public const string Operations_Insert =
        "INSERT INTO Operations(Id, Type, BankAccountId, Amount, Date, Description, CategoryId) " +
        "VALUES (@Id, @Type, @BankAccountId, @Amount, @Date, @Description, @CategoryId)";

    public const string Operations_Delete =
        "DELETE FROM Operations WHERE Id = @Id";
}
