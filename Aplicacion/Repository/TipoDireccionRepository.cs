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
    public class TipoDireccionRepository : Generic<TipoDireccion>, ITipoDireccion
    {
        private readonly PushContext _context;
        public TipoDireccionRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<TipoDireccion>> GetAllAsync()
        {
            return await _context.TipoDirecciones
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<TipoDireccion> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.TipoDirecciones.AsQueryable();

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