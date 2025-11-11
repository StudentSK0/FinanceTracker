using Finance.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Finance.Infrastructure.Import;

public sealed class YamlImporter : ImporterBase
{
    public YamlImporter(IAccountRepo a, ICategoryRepo c, IOperationRepo o) : base(a, c, o) { }

    protected override RootDto Load(string path)
    {
        var des = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return des.Deserialize<RootDto>(File.ReadAllText(path)) ?? new RootDto();
    }
}
