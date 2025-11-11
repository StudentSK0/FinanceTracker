using Finance.Domain.Export;

namespace Finance.Domain;

public sealed class ExportFacade
{
    private readonly IAccountRepo _acc;
    private readonly ICategoryRepo _cat;
    private readonly IOperationRepo _ops;

    public ExportFacade(IAccountRepo acc, ICategoryRepo cat, IOperationRepo ops)
    {
        _acc = acc; _cat = cat; _ops = ops;
    }

    public void Accept(IExportVisitor visitor)
        => visitor.Visit(_acc.List(), _cat.List(), _ops.List());
}
