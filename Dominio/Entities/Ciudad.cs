using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Ciudad : BaseEntity
    {
        public string Nombre {get; set;}
        public int IdDepartamentoFk {get; set;}
        public Departamento Departamento {get; set;}
        public ICollection<Persona> Personas {get; set;}
        

    }
}