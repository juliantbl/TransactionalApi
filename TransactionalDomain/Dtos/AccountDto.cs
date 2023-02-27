
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Dtos
{
    public class AccountDto
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountType { get; set; }
        public decimal Balance { get; set; } = 0;
        public bool Status { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
