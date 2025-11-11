namespace Finance.Domain;

public sealed record OperationDeleted(Operation Operation) : IDomainEvent;
