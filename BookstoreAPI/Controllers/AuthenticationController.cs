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
                var saltEncrypted = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]!)); 

                var signature = new SigningCredentials(saltEncrypted, SecurityAlgorithms.HmacSha256);

                
                var claimsForToken = new List<Claim>();
                claimsForToken.Add(new Claim("sub", userLogged.Id.ToString())); 
                claimsForToken.Add(new Claim("role", userLogged.Role.ToString())); 

                var jwtSecurityToken = new JwtSecurityToken( 
                  _configuration["Authentication:Issuer"],
                  _configuration["Authentication:Audience"],
                  claimsForToken,
                  DateTime.UtcNow,
                  DateTime.UtcNow.AddHours(1),
                  signature);

                var tokenToReturn = new JwtSecurityTokenHandler() 
                    .WriteToken(jwtSecurityToken);
                return Ok(tokenToReturn);

            }
            return Unauthorized();

        }

    }
}
