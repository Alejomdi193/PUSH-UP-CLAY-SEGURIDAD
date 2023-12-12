using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository
{
    public class ContratoRepository : Generic<Contrato>, IContrato
    {
        public readonly PushContext _context;
        public ContratoRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Contrato>> GetAllAsync()
        {
            return await _context.Contratos
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Contrato> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Contratos.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.FechaContrato.ToString().ToLower().Contains(search));
            }

            query = query.OrderBy(p => p.Id);
            var totalRegistros = await query.CountAsync();
            var registros = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (totalRegistros, registros);

        }

        public async Task<IEnumerable<object>> ListarContratosActivos()
        {
            var contratosActivos = await _context.Contratos
                .Where(c => c.Estado.Descripcion == "Activo")
                .Select(c => new
                {
                    NumeroContrato = c.Id,
                    NombreCliente = c.Cliente.Nombre,
                    EmpleadoRegistrador = c.Empleado.Nombre
                })
                .ToListAsync();

            return contratosActivos;
        }

    }
}