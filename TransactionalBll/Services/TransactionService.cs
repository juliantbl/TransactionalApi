
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Enums;
using TransactionalDomain.Interfaces;

namespace TransactionalBll.Services
{
    public class TransactionService : ITransactionService
    {
        IRepositoryAsync<Transaction> repository;
        IRepositoryAsync<Account> accountRepository;


        public TransactionService(IRepositoryAsync<Transaction> _repository, IRepositoryAsync<Account> _accountRepository)
        {
            repository = _repository;
            accountRepository = _accountRepository;
        }
        public async Task<String> AddTransaction(TransactionDto transactionDto)
        {
            var transactionTypeId = ParseTransactionType(transactionDto.TransactionType);

            if (transactionTypeId == -1) return "Transacción rechazada,tipo de transacción invalida";

            var account = await accountRepository.GetById(transactionDto.AccountId);

            if (account == null) return "Transacción rechazada, cuenta no encontrada";

            transactionDto.Value= AdjustTransactionValue( transactionTypeId, transactionDto.Value);

            var newBalance = account.Balance + transactionDto.Value;

            if (newBalance < 0) return "Transacción rechazada, saldo no disponible ";

            var newTransaction = new Transaction {
             AccountId = account.Id,
             Balance = newBalance,
             Date = DateTime.Now,
             TransactionTypeId= transactionTypeId,
             Value= transactionDto.Value
            };

            account.Balance = newBalance;

            await repository.Insert(newTransaction);

            await accountRepository.Update(account);

            return "Transacción exitosa";
        }

        protected int ParseTransactionType(string transactionType)
        {
            var transactionTypeId = -1;

            if (transactionType.ToLower() == nameof(TransactionTypesEnum.Retiro).ToLower())
            {
                transactionTypeId = (int)TransactionTypesEnum.Retiro;
            }
            else if (transactionType.ToLower() == nameof(TransactionTypesEnum.Deposito).ToLower())
            {
                transactionTypeId = (int)TransactionTypesEnum.Deposito;
            }

            return transactionTypeId;
        }

        protected decimal AdjustTransactionValue(int transactionTypeId, decimal value ) 
        {
            if( (transactionTypeId == (int) TransactionTypesEnum.Retiro && value > 0)  ||
                (transactionTypeId == (int)TransactionTypesEnum.Deposito && value < 0) 
                )
                value = value * -1;

            return value;
        }

        public async Task<IEnumerable<Transaction>> GetAccountTransactions(int accountId)
        {
            return await repository.FindAll(x => x.AccountId == accountId);
        }

        public async Task<bool> TransactionExists(int id)
        {
            var transaction = await repository.GetById(id);
            return transaction != null;
        }

        public async Task<Transaction> GetTransactionById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task RemoveTransaction(int id)
        {
            //TODO: Account Balance Should be Refunded?
            await repository.Delete(id);
        }

        public async Task<Transaction> UpdateTransaction(int id, TransactionDto transactionDto)
        {
            var currentTransaction = await repository.GetById(id);

            if (currentTransaction == null) return null;

            var transactionTypeId = ParseTransactionType(transactionDto.TransactionType);

            transactionDto.Value = AdjustTransactionValue(transactionTypeId, transactionDto.Value);

            //Reverse prior transaction and redo 
            var account = await accountRepository.GetById(transactionDto.AccountId);

            if (account == null) return null;

            var newBalance = (currentTransaction.Value * -1)+ transactionDto.Value+ account.Balance;

            if (newBalance < 0) return null;

            //Apply changes

            currentTransaction.Date = DateTime.Now;
            currentTransaction.Value = transactionDto.Value;
            currentTransaction.Balance = newBalance;
            currentTransaction.TransactionTypeId=transactionTypeId;

            account.Balance = newBalance;

            //For security clients balance or Id can't be updated here

            await repository.Update(currentTransaction);

            await accountRepository.Update(account);

            return currentTransaction;
        }
    }
}
