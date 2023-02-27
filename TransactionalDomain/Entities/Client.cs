using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TransactionalDomain.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")] 
        public virtual User User { get; set; }
        [Required] 
        public string? Password { get; set; }
        public bool Status { get; set; }
        public ICollection<Account> Accounts { get; set; }

    }
}
