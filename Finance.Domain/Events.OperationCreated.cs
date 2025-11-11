namespace Finance.Domain;

public sealed record OperationCreated(Operation Operation) : IDomainEvent;
