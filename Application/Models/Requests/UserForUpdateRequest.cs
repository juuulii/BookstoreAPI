using Domain.Entities;

namespace Application.Dtos
{
    public class UserForUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
