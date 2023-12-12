using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository
{
    public class PersonaRepository : Generic<Persona>, IPersona
    {
        private readonly PushContext _context;
        public PersonaRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Persona> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Personas.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Nombre.ToLower().Contains(search));
            }

            query = query.OrderBy(p => p.Id);
            var totalRegistros = await query.CountAsync();
            var registros = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (totalRegistros, registros);
        }


        public async Task<IEnumerable<object>> ListarEmpleados()
        {
            var empleados = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Empleado")
                .Select(e => new
                {
                    Id = e.IdPersona,
                    Nombre = e.Nombre,
                    Puesto = e.TipoPersona.Descripcion
                })
                .ToListAsync();

            return empleados;
        }


        public async Task<IEnumerable<object>> ListarVigilantes()
        {
            var vigilantes = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Empleado" && p.CategoriaPersona.Nombre == "Vigilante")
                .Select(e => new
                {
                    Id = e.IdPersona,
                    Nombre = e.Nombre,
                    Puesto = e.CategoriaPersona.Nombre
                })
                .ToListAsync();

            return vigilantes;
        }
        public async Task<IEnumerable<object>> ListarNumerosDeContactoVigilantes()
        {
            var numerosDeContacto = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Empleado" && p.CategoriaPersona.Nombre == "Vigilante")
                .SelectMany(e => e.ContactoPersonas.Select(cp => cp.TipoContacto))
                .ToListAsync();

            return numerosDeContacto;
        }

        public async Task<IEnumerable<object>> ListarClientesEnBucaramanga()
        {
            var clientesEnBucaramanga = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Cliente" && p.Ciudad.Nombre == "Bucaramanga")
                .Select(c => new
                {
                    IdCliente = c.IdPersona,
                    NombreCliente = c.Nombre,
                    Ciudad = c.Ciudad.Nombre
                })
                .ToListAsync();

            return clientesEnBucaramanga;
        }

        public async Task<IEnumerable<object>> ListarEmpleadosEnPiedecuestaOYGiron()
        {
            var empleadosEnPiedecuestaOYGiron = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Empleado" &&
                            (p.Ciudad.Nombre == "Piedecuesta" || p.Ciudad.Nombre == "Giron"))
                .Select(e => new
                {
                    IdEmpleado = e.IdPersona,
                    NombreEmpleado = e.Nombre,
                    Ciudad = e.Ciudad.Nombre,

                })
                .ToListAsync();

            return empleadosEnPiedecuestaOYGiron;
        }

        public async Task<IEnumerable<object>> ListarClientesConMasDe5AnosAntiguedad()
        {
            DateTime fechaLimite = DateTime.Now.AddYears(-5);

            var clientesConMasDe5Anios = await _context.Personas
                .Where(p => p.TipoPersona.Descripcion == "Cliente" && p.FechaRegistro <= fechaLimite)
                .Select(c => new
                {
                    IdCliente = c.IdPersona,
                    NombreCliente = c.Nombre,
                    Antiguedad = (DateTime.Now - c.FechaRegistro).TotalDays / 365
                })
                .ToListAsync();

            return clientesConMasDe5Anios;
        }

    }

}