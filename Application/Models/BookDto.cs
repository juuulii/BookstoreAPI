using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string? AuthorName { get; set; }
        public string? AuthorLastName { get; set; }
        public string? PublisherName { get; set; }
        public string? CategoryName { get; set; }
    }

}
