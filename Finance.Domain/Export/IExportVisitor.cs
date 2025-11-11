namespace Finance.Domain.Export;

public interface IExportVisitor
{
    void Visit(
        IEnumerable<BankAccount> accounts,
        IEnumerable<Category> categories,
        IEnumerable<Operation> operations
    );
}
