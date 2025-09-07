using Application.Dtos;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//La autenticación puede leer el UserId desde los claims del JWT.?

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private IConfiguration _configuration;

        public AuthenticationController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] CredentialsRequest credentials)
        {
            UserModel? userLogged = _userService.CheckCredentials(credentials);
            if (userLogged is not null)
            {
                var saltEncrypted = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]!)); //encripta nuestro secreto, el '!' significa q nunca va a ser nulo

                var signature = new SigningCredentials(saltEncrypted, SecurityAlgorithms.HmacSha256);

                //Los claims son datos en clave->valor que nos permite guardar data del usuario.
                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", userLogged.Id.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
                claimsForToken.Add(new Claim("role", userLogged.Role.ToString())); //Debería venir del usuario

                var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
                  _configuration["Authentication:Issuer"],
                  _configuration["Authentication:Audience"],
                  claimsForToken,
                  DateTime.UtcNow,
                  DateTime.UtcNow.AddHours(1),
                  signature);

                var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                    .WriteToken(jwtSecurityToken);
                return Ok(tokenToReturn);

            }
            return Unauthorized();

        }

    }
}
