namespace Finance.Domain.Facades;

using Finance.Domain.Import;

public sealed class ImportFacade
{
    private readonly IAccountRepo _acc;
    private readonly ICategoryRepo _cat;
    private readonly IOperationRepo _ops;

    public ImportFacade(IAccountRepo acc, ICategoryRepo cat, IOperationRepo ops)
    {
        _acc = acc;
        _cat = cat;
        _ops = ops;
    }

    public void Import(IImporter importer, string path)
        => importer.Import(path);
}
