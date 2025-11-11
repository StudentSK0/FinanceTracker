using System.Globalization;

namespace Finance.Infrastructure.Import;

internal static class CsvHelpers
{
    public static IEnumerable<AccountDto> ReadAccounts(string path)
    {
        foreach (var line in File.ReadAllLines(path)[1..])
        {
            var c = line.Split(';', ',');
            yield return new AccountDto(c[0].Trim(), decimal.Parse(c[1], CultureInfo.InvariantCulture));
        }
    }

    public static IEnumerable<CategoryDto> ReadCategories(string path)
    {
        foreach (var line in File.ReadAllLines(path)[1..])
        {
            var c = line.Split(';', ',');
            yield return new CategoryDto(c[0].Trim(), c[1].Trim());
        }
    }

    public static IEnumerable<OperationDto> ReadOperations(string path)
    {
        foreach (var line in File.ReadAllLines(path)[1..])
        {
            var c = line.Split(';', ',');
            yield return new OperationDto(
                c[0].Trim(), c[1].Trim(),
                decimal.Parse(c[2], CultureInfo.InvariantCulture),
                c[3].Trim(),
                c.Length > 4 ? c[4].Trim() : null,
                c.Length > 5 ? c[5].Trim() : null
            );
        }
    }
}
