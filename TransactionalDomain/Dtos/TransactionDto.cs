using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionalDomain.Entities;

namespace TransactionalDomain.Dtos
{
    public class TransactionDto
    {
        [Required]
        public string TransactionType { get; set; }
        [Required]
        public decimal Value { get; set; }        
        [Required]
        public int AccountId { get; set; }
    }
}
