using TransactionalDomain.Dtos;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Interfaces
{
    public interface  IReportService
    {
        Task<IEnumerable<Object>> GenerateStatement(StatementDto statementDto);
    }
}
