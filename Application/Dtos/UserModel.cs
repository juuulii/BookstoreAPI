using Domain.Entities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] //en vez de ser 1 y 2, transforma en user y admin, este es para la response y el otro para la bd
        public UserRole Role { get; set; }
    }
}