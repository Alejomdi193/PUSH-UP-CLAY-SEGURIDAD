using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Repository
{
    public class UserRepository : Generic<User>, IUser
    {
        protected readonly PushContext _context;

        public UserRepository(PushContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                    .Include(u => u.Rols)
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Rols)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Nombre.ToLower() == username.ToLower());
        }

        public override async Task<(int totalRegistros, IEnumerable<User> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Users as IQueryable<User>;

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