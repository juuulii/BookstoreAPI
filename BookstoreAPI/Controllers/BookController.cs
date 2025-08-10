using Application.Dtos;
using Application.Dtos.Application.Dtos; // Para UpdateBookRequest
using Application.DTOs; // Para CreateBookRequest
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
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/Book
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetBooks([FromQuery] bool includeDeleted = false)
        {
            var books = _bookRepository.GetAll(includeDeleted)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Stock = b.Stock,
                    AuthorName = b.Author.Name,
                    AuthorLastName = b.Author.LastName,
                    PublisherName = b.Publisher.Name,
                    CategoryName = b.Category.Name
                });

            return Ok(books);
        }


        // GET: api/Book/5
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _bookRepository.GetById(id);

            if (book == null || book.IsDeleted)
                return NotFound();

            return Ok(book);
        }

        // POST: api/Book (usando DTO)
        [HttpPost]
        public ActionResult<Book> CreateBook([FromBody] CreateBookRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            var book = new Book
            {
                Title = request.Title,
                Price = request.Price,
                Stock = request.Stock,
                AuthorId = request.AuthorId,
                PublisherId = request.PublisherId,
                CategoryId = request.CategoryId,
                IsDeleted = false
            };

            _bookRepository.Add(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/Book/5 (usando DTO)
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookRequest request)
        {
            if (request == null)
                return BadRequest("Datos inválidos");

            var book = _bookRepository.GetById(id);

            if (book == null || book.IsDeleted)
                return NotFound();

            book.Title = request.Title;
            book.Price = request.Price;
            book.Stock = request.Stock;
            book.AuthorId = request.AuthorId;
            book.PublisherId = request.PublisherId;
            book.CategoryId = request.CategoryId;

            _bookRepository.Update(book);

            return NoContent();
        }

        // DELETE lógico: api/Book/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _bookRepository.GetById(id);

            if (book == null || book.IsDeleted)
                return NotFound();

            _bookRepository.Delete(id);

            return NoContent();
        }
    }
}



