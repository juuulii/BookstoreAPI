namespace Application.DTOs
{
    public class CreateBookRequest
    {
        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
    }
}

