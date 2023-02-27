
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionalDomain.Entities;
using System.Runtime.CompilerServices;

namespace TransactionalDomain.Dtos
{
    public class ClientDto
    {
        [Required]
        public string Name { get; set; }
        public string Gender { get; set; } = "undefined";
        [Required]
        public int Age { get; set; }
        [Required]
        public string Identification { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Status { get; set; }
    }
}
