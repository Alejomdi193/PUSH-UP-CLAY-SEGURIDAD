using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Contrato : BaseEntity
    {
        public DateTime FechaContrato {get; set;}
        public DateTime FechaFin {get; set;}
        public int IdClienteFk {get; set;}
        public Persona Cliente {get; set;}
        public int IdEmpleadoFk {get; set;}
        public Persona Empleado {get; set;}
        public int IdEstadoFk {get; set;}
        public Estado Estado {get; set;}
        public ICollection<Programacion> Programaciones {get; set;}
        


    }
}