namespace Finance.Infrastructure.Import;

public sealed class RootDto
{
    public List<AccountDto> Accounts { get; init; } = new();
    public List<CategoryDto> Categories { get; init; } = new();
    public List<OperationDto> Operations { get; init; } = new();
}

public sealed record AccountDto(string Name, decimal Balance);
public sealed record CategoryDto(string Name, string Type);
public sealed record OperationDto(string Type, string BankAccountId, decimal Amount, string Date, string? Description, string? CategoryId);
