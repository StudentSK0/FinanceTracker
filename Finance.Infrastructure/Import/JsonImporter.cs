using System.Text.Json;
using Finance.Domain;

namespace Finance.Infrastructure.Import;

public sealed class JsonImporter : ImporterBase
{
    public JsonImporter(IAccountRepo a, ICategoryRepo c, IOperationRepo o) : base(a, c, o) { }

    protected override RootDto Load(string path)
        => JsonSerializer.Deserialize<RootDto>(File.ReadAllText(path)) ?? new RootDto();
}
