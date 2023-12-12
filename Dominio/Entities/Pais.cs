using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Pais : BaseEntity
    {
        public string Nombre {get; set;}
        public ICollection<Departamento> Departamentos {get; set;}
        
    }
}