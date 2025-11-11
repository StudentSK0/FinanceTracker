namespace Finance.App.Commands;

public interface ICommand
{
    /// <summary>Код команды (строка, т.е. "1", "2", "7"...)</summary>
    string Key { get; }

    /// <summary>Отображаемое имя в меню.</summary>
    string Name { get; }

    void Execute();
}
