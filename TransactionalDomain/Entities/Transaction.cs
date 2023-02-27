using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace TransactionalDomain.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TransactionTypeId { get; set; }
        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
