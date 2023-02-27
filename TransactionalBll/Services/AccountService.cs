using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Enums;
using TransactionalDomain.Interfaces;

namespace TransactionalBll.Services
{
    public class AccountService : IAccountService
    {
        IRepositoryAsync<Account> repository;

        public AccountService(IRepositoryAsync<Account> _repository)
        {
            repository = _repository;
        }
        public async Task<bool> AccountExists(string number)
        {
            var account = await repository.Find(x => x.AccountNumber == number);
            return account != null;
        }

        public async Task AddAccount(AccountDto accountDto)
        {
            var accountTypeId = ParseAccountType(accountDto.AccountType);

            if (accountTypeId == -1) return;

            var newAccount = new Account {
                AccountNumber = accountDto.AccountNumber,
                AccountTypeId = accountTypeId,
                Balance = accountDto.Balance,
                ClientId = accountDto.ClientId,
                Status = accountDto.Status              
            };

            await repository.Insert(newAccount);
        }

        public Task<Account> GetAccountById(int id)
        {
            return repository.GetById(id);
        }

        public Task<IEnumerable<Account>> GetClientAccounts(int clientId)
        {
            return repository.FindAll(x=>x.ClientId== clientId);

        }

        public async Task RemoveAccount(int id)
        {
            await repository.Delete(id);
        }

        public async Task<Account> UpdateAccount(int id, AccountDto accountDto, bool replace)
        {
            var currentAccount = await repository.GetById(id);

            if (currentAccount == null) return null;

            var accountTypeId = ParseAccountType(accountDto.AccountType);

            if (accountTypeId != -1) currentAccount.AccountTypeId = accountTypeId;

            if (replace || !string.IsNullOrEmpty(accountDto.AccountNumber))
                currentAccount.AccountNumber = accountDto.AccountNumber;
            //For security clients balance or Id can't be updated here

            currentAccount.Status = accountDto.Status;

            await repository.Update(currentAccount);

            return currentAccount;
        }

        protected int ParseAccountType(string accountType) 
        {
            var accountTypeId = -1;

            if (accountType.ToLower() == nameof(AccountTypesEnum.Ahorros).ToLower())
            {
                accountTypeId = (int)AccountTypesEnum.Ahorros;
            }
            else if (accountType.ToLower() == nameof(AccountTypesEnum.Corriente).ToLower())
            {
                accountTypeId = (int)AccountTypesEnum.Corriente;
            }

            return accountTypeId;
        }
    }
}
