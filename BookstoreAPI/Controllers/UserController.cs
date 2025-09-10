using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //ver esto
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult Get(string name)
        {
            var user = _service.Get(name);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult Update(int id, [FromBody] UserForUpdateRequest body)
        {
            var user = _service.GetById(id);
            if (user == null) return NotFound();

            _service.Update(id, body);
            return Ok("Usuario actualizado correctamente.");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public IActionResult Get([FromQuery] bool includeDeleted = false)
        {
            return Ok(_service.Get(includeDeleted));
        }


        [HttpPost]
        [AllowAnonymous] // 👈 importante para registrarse
        public IActionResult Add([FromBody] UserForAddRequest body)
        {
            return Ok(_service.AddUser(body));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
