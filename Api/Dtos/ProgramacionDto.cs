using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class ProgramacionDto
    {
        public int Id {get; set;}
        public int IdContratoFk {get; set; }
        public ContratoDto Contrato {get; set;}
        public int IdTurnoFk {get; set;}
        public  TurnoDto Turno {get; set;}
        public int IdEmpleadoFk {get; set;}
        public PersonaDto Empleado {get; set;}
    }
}