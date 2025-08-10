using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        // relación con libros
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

