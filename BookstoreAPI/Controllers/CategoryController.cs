using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Cliente")] 
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] bool includeDeleted = false)
    {
        var categories = await _categoryService.GetAllAsync(includeDeleted);

        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Books = c.Books.Select(b => new BookDto
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

        return Ok(categoryDtos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Cliente")] 
    public async Task<ActionResult<CategoryDto>> GetCategory(int id, [FromQuery] bool includeDeleted = false)
    {
        var category = await _categoryService.GetByIdAsync(id, includeDeleted);
        if (category == null) return NotFound();

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Books = category.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                Stock = b.Stock
            }).ToList()
        };

        return Ok(categoryDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryRequest createDto)
    {
        var category = new Category
        {
            Name = createDto.Name
        };

        var created = await _categoryService.CreateAsync(category);

        var categoryDto = new CategoryDto
        {
            Id = created.Id,
            Name = created.Name,
            Books = new List<BookDto>()
        };

        return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> UpdateCategory(int id, CreateCategoryRequest updateDto)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();

        category.Name = updateDto.Name;

        var updated = await _categoryService.UpdateAsync(category);
        if (!updated) return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}
