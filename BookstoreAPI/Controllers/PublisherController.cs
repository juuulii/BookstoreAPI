using Application.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PublishersController : ControllerBase
{
    private readonly PublisherService _publisherService;

    public PublishersController(PublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Cliente")] 
    public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers([FromQuery] bool includeDeleted = false)
    {
        var publishers = await _publisherService.GetAllAsync(includeDeleted);

        var publisherDtos = publishers.Select(p => new PublisherDto
        {
            Id = p.Id,
            Name = p.Name,
            Books = p.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                Stock = b.Stock
            }).ToList()
        });

        return Ok(publisherDtos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Cliente")] 
    public async Task<ActionResult<PublisherDto>> GetPublisher(int id, [FromQuery] bool includeDeleted = false)
    {
        var publisher = await _publisherService.GetByIdAsync(id, includeDeleted);
        if (publisher == null) return NotFound();

        var publisherDto = new PublisherDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Books = publisher.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Price = b.Price,
                Stock = b.Stock
            }).ToList()
        };

        return Ok(publisherDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] 
    public async Task<ActionResult<PublisherDto>> CreatePublisher(CreatePublisherRequest createDto)
    {
        var publisher = new Publisher
        {
            Name = createDto.Name
        };

        var created = await _publisherService.CreateAsync(publisher);

        var publisherDto = new PublisherDto
        {
            Id = created.Id,
            Name = created.Name,
            Books = new List<BookDto>()
        };

        return CreatedAtAction(nameof(GetPublisher), new { id = publisherDto.Id }, publisherDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> UpdatePublisher(int id, CreatePublisherRequest updateDto)
    {
        var publisher = await _publisherService.GetByIdAsync(id);
        if (publisher == null) return NotFound();

        publisher.Name = updateDto.Name;

        var updated = await _publisherService.UpdateAsync(publisher);
        if (!updated) return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> DeletePublisher(int id)
    {
        var deleted = await _publisherService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}

