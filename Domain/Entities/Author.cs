using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string Nationality { get; set; }
        public bool IsDeleted { get; set; } = false;

        // colección de libros del autor
        [JsonIgnore] // para q no se hagan ciclos
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
