using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class ContratoDto
    {
        public int Id {get; set;}
        public DateTime FechaContrato {get; set;}
        public DateTime FechaFin {get; set;}
    }
}