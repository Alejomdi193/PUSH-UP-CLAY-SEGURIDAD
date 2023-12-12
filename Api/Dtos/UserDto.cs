using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class UserDto
    {
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}