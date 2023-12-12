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
    public class RolRepository : Generic<Rol>, IRol
    {
        protected readonly PushContext _context;

        public RolRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Rols
                .ToListAsync();
        }

        public override async Task<Rol> GetByIdAsync(int id)
        {
            return await _context.Rols
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public override async Task<(int totalRegistros, IEnumerable<Rol> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Rols as IQueryable<Rol>;

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