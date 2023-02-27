using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionalDomain.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        [ForeignKey("AccountTypeId")]
        public virtual AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public bool Status { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

    }
}
