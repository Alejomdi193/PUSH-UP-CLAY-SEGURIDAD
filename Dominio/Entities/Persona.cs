using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Persona : BaseEntity
    {
        public int IdPersona {get; set;}
        public string Nombre {get; set;}
        public DateTime FechaRegistro {get; set;}
        public int IdTipoPersonaFk {get; set;}
        public TipoPersona TipoPersona {get; set;}
        public int IdCategortiaPersonaFk {get; set;}
        public CategoriaPersona CategoriaPersona {get; set;}
        public int IdCiudadFk {get; set;}
        public Ciudad Ciudad {get; set;}
        public ICollection<DirPersona> DirPersonas {get; set;}
        public ICollection<Contrato> ContratosEmpleado {get; set;}
        public ICollection<Contrato> ContratosCliente {get; set;}
        public ICollection<ContactoPersona> ContactoPersonas {get; set;}
        public ICollection<Programacion> Programaciones {get; set;}


    }
}