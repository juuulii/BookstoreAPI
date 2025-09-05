using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        // relación con libros
        [JsonIgnore] //tmb podria haberlo configurado globalmente en program.cs
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}

