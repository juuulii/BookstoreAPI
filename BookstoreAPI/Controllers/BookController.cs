using Application.Dtos;
using Application.DTOs; // Para CreateBookRequest
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;

        public BookController(BookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Book
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetBooks([FromQuery] bool includeDeleted = false)
        {
            var books = _bookService.GetAll(includeDeleted)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Stock = b.Stock,
                    AuthorName = b.Author?.Name,
                    AuthorLastName = b.Author?.LastName,
                    PublisherName = b.Publisher?.Name,
                    CategoryName = b.Category?.Name
                });

            return Ok(books);
        }

        // GET: api/Book/{id}
        [HttpGet("{id}")]
        public ActionResult<BookDto> GetBook(int id)
        {
            var book = _bookService.GetById(id);
            if (book == null || book.IsDeleted)
                return NotFound();

            var dto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
                Stock = book.Stock,
                AuthorName = book.Author?.Name,
                AuthorLastName = book.Author?.LastName,
                PublisherName = book.Publisher?.Name,
                CategoryName = book.Category?.Name
            };

            return Ok(dto);
        }

        // POST: api/Book
        [HttpPost]
        public ActionResult<BookDto> CreateBook([FromBody] CreateBookRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            var dto = _bookService.CreateBook(request);
            if (dto == null)
                return NotFound();

            return CreatedAtAction(nameof(GetBook), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            var updated = _bookService.Update(id, request);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE lógico: api/Book/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var deleted = _bookService.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}


