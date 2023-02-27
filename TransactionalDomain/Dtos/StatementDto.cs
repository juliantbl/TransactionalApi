
using System.ComponentModel.DataAnnotations;

namespace TransactionalDomain.Dtos
{
    public class StatementDto
    {
        [Required]
        public DateTime InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public int ClientId { get; set; }

    }
}
