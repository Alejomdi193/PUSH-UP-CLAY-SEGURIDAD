using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Turno : BaseEntity
    {
        public string Nombre {get; set;}
        public DateTime HoraTurnoI {get; set;}
        public DateTime HoraTurnoFinal {get; set;}
        public ICollection<Programacion> Programaciones {get; set;}
        
    }
}