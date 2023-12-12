using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class ContactoPersona : BaseEntity
    {
        public string Descripcion {get; set;}
        public int IdPersonaFk {get; set;}
        public Persona Persona {get; set;}
        public int IdTipoContactoFk {get; set;}
        public TipoContacto TipoContacto {get; set;}
        
    }
}