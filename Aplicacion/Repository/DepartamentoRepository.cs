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
    public class DepartamentoRepository : Generic<Departamento>, IDepartamento
    {
        private readonly PushContext _context;
        public DepartamentoRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await _context.Departamentos
                .ToListAsync();
        }

        public override async Task<(int totalRegistros, IEnumerable<Departamento> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Departamentos.AsQueryable();

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