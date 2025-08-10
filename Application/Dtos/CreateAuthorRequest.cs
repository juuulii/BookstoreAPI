using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
    }
}
