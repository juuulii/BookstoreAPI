using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }   

        public int ClienteId { get; set; }
        public User? Cliente { get; set; } 

        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
} 
