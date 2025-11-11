using System;
using System.Collections.Generic;

namespace Finance.Domain;

public interface IAccountRepo
{
    void Add(BankAccount acc);
    BankAccount Get(Guid id);
    IEnumerable<BankAccount> List();

    // Обновляем только баланс счёта
    void Update(BankAccount acc);

    void Remove(Guid id);
}

public interface ICategoryRepo
{
    void Add(Category cat);
    Category Get(Guid id);
    IEnumerable<Category> List();
    void Rename(Guid id, string newName);
    void Remove(Guid id);
}

public interface IOperationRepo
{
    Operation Get(Guid id);
    IEnumerable<Operation> List();
    IEnumerable<Operation> ListByAccount(Guid accountId);

    // Добавление операции с автоматической корректировкой баланса
    void AddAndApplyBalance(Operation op, BankAccount account);

    // Удаление операции с автоматическим откатом баланса
    void RemoveAndRevertBalance(Operation op, BankAccount account);
}
