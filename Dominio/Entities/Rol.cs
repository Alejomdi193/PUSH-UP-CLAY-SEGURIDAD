using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Rol : BaseEntity
    {
        public string Nombre { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<UserRol> UserRols { get; set; }
    }
}