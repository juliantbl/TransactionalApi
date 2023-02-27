using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionalDomain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        public string? Name { get; set; }
        public string Gender { get; set; } = "unknown";
        public int Age { get; set; }
        [Required] 
        public string Identification { get; set; } = "undefined";
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
