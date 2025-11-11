namespace Finance.Domain.Import;

public interface IImporter
{
    void Import(string path);
}
