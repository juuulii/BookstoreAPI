using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Cliente")] 
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery] bool includeDeleted = false)
        {
            var authors = await _authorService.GetAllAsync(includeDeleted);

            var authorDtos = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name,
                LastName = a.LastName,
                Nationality = a.Nationality,
                Books = a.Books.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Stock = b.Stock,
                    AuthorName = b.Author?.Name,
                    AuthorLastName = b.Author?.LastName,
                    PublisherName = b.Publisher?.Name,
                    CategoryName = b.Category?.Name
                }).ToList()
            });

            return Ok(authorDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Cliente")] 
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id, [FromQuery] bool includeDeleted = false)
        {
            var author = await _authorService.GetByIdAsync(id, includeDeleted);
            if (author == null) return NotFound();

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                LastName = author.LastName,
                Nationality = author.Nationality,
                Books = author.Books.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Stock = b.Stock
                }).ToList()
            };

            return Ok(authorDto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorRequest createDto)
        {
            var author = new Author
            {
                Name = createDto.Name,
                LastName = createDto.LastName,
                Nationality = createDto.Nationality
            };

            var created = await _authorService.CreateAsync(author);

            var authorDto = new AuthorDto
            {
                Id = created.Id,
                Name = created.Name,
                LastName = created.LastName,
                Nationality = created.Nationality,
                Books = new List<BookDto>() 
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = authorDto.Id }, authorDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> UpdateAuthor(int id, CreateAuthorRequest updateDto)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null) return NotFound();

            author.Name = updateDto.Name;
            author.LastName = updateDto.LastName;
            author.Nationality = updateDto.Nationality;

            var updated = await _authorService.UpdateAsync(author);
            if (!updated) return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var deleted = await _authorService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}


