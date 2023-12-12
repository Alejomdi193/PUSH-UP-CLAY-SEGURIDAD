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
    public class TurnoRepository : Generic<Turno>, ITurnos
    {
        private readonly PushContext _context;
        public TurnoRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Turno>> GetAllAsync()
        {
            return await _context.Turnos
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Turno> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Turnos.AsQueryable();

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
    }
}