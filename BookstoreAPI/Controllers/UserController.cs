using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            return Ok(_service.Get(name));
        }

        [HttpPut]
        public IActionResult UpdateEmail(string email)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _service.GetById(userId);
            _service.Update(user);
            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserForAddRequest body)
        {
            return Ok(_service.AddUser(body));
        }
    }
}