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
    public class ProgramacionRepository : Generic<Programacion>, IProgramacion
    {
        public readonly PushContext _context; 
        public ProgramacionRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Programacion>> GetAllAsync()
        {
            return await _context.Programaciones
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Programacion> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Programaciones.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Id.ToString().ToLower().Contains(search));
            }

            query = query.OrderBy(p => p.Id);
            var totalRegistros = await query.CountAsync();
            var registros = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (totalRegistros, registros);
        }
    }
}