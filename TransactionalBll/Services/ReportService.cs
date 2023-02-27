
using Microsoft.EntityFrameworkCore;
using TransactionalDal;
using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;
using TransactionalDomain.Interfaces;

namespace TransactionalBll.Services
{
    public class ReportService : IReportService
    {
        private readonly TransactionsContext transactionsContext;
        private readonly IRepositoryAsync<Account> accountRepository;
        public ReportService(TransactionsContext _transactionsContext, IRepositoryAsync<Account> _accountRepository)
        {
            transactionsContext = _transactionsContext;
            accountRepository = _accountRepository;
        }
        public async Task<IEnumerable<Object>> GenerateStatement(StatementDto statementDto)
        {
            if (statementDto.FinalDate == null) statementDto.FinalDate = DateTime.Now.AddDays(3);

            var userAccounts = await transactionsContext.Accounts.AsNoTracking()
                                        .Where(a => a.ClientId == statementDto.ClientId)
                                        .Select(account => new 
                                        {
                                            account.AccountNumber,
                                            account.AccountType.Name,
                                            account.Balance,
                                            account.Status,
                                            Transactions = 
                                                account.Transactions.Where(t => t.Date >= statementDto.InitialDate && t.Date <= statementDto.FinalDate)
                                                        .Select(transaction => new
                                                        {
                                                            transaction.TransactionType.Name,
                                                            transaction.Value,
                                                            transaction.Date,
                                                        })
                                                        .ToList()
                                        })
                                        .ToListAsync();
            return userAccounts;
        }
    }
}
