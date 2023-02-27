using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Interfaces
{
    public interface ITransactionService
    {
        Task<String> AddTransaction(TransactionDto transactionDto);

        Task<IEnumerable<Transaction>> GetAccountTransactions(int accountId);
        Task<bool> TransactionExists(int id);
        Task<Transaction> GetTransactionById(int id);
        Task RemoveTransaction(int id);
        Task<Transaction> UpdateTransaction(int id, TransactionDto transactionDto);
    }
}
