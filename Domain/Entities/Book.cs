using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        // claves foráneas
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }

        public Book() { }

        public Book(string title, decimal price, int stock, int authorId, int publisherId, int categoryId)
        {
            Title = title;
            Price = price;
            Stock = stock;
            AuthorId = authorId;
            PublisherId = publisherId;
            CategoryId = categoryId;
        }
    }

}
