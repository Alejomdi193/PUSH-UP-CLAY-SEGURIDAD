using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class TurnoDto
    {
        public int Id {get; set;}
        public string Nombre {get; set;}
        public DateTime HoraTurnoI {get; set;}
        public DateTime HoraTurnoFinal {get; set;}
    }
}