using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Programacion : BaseEntity
    {
        public int IdContratoFk {get; set; }
        public Contrato Contrato {get; set;}
        public int IdTurnoFk {get; set;}
        public  Turno Turno {get; set;}
        public int IdEmpleadoFk {get; set;}
        public Persona Empleado {get; set;}
        
        
    }
}