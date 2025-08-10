using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }

        // colección de libros del autor
        [JsonIgnore] // para q no se hagan ciclos
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
