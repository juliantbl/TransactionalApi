
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetClientAccounts(int clientId);
        Task<bool> AccountExists(string number);
        Task AddAccount(AccountDto accountDto);
        Task<Account> GetAccountById(int id);
        Task RemoveAccount(int id);
        Task<Account> UpdateAccount(int id, AccountDto accountDto, bool replace);
    }
}
