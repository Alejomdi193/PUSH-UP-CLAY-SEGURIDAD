using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class PersonaDto
    {
        public int Id {get; set;}
         public int IdPersona {get; set;}
        public string Nombre {get; set;}
        public DateTime FechaRegistro {get; set;}
    }
}