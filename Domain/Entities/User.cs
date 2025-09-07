using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //va a ser numerico y unico
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public UserRole Role { get; set; }
        [Required]
        public required string Password { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public enum UserRole
    {
        Cliente,
        Admin
    }
}