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
    public class PaisRepository : Generic<Pais>, IPais
    {
        private readonly PushContext _context;
        public PaisRepository(PushContext context) : base(context)
        {
            _context = context;
        }


        public override async Task<IEnumerable<Pais>> GetAllAsync()
        {
            return await _context.Paises
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Pais> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Paises.AsQueryable();

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