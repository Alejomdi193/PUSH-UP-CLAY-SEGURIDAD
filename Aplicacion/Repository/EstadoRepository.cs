using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Data.Configuration;

namespace Aplicacion.Repository
{
    public class EstadoRepository : Generic<Estado>, IEstado
    {
        public readonly PushContext _context;
        public EstadoRepository(PushContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<IEnumerable<Estado>> GetAllAsync()
        {
            return await _context.Estados
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Estado> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Estados.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Descripcion.ToLower().Contains(search));
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