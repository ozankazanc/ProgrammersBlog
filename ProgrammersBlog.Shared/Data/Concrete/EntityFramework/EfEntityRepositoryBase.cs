using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Shared.Data.Abstract;
using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Data.Concrete.EntityFramework
{
    public class EfEntityRepositoryBase<Tentity> : IEntityRepository<Tentity>
        where Tentity : class, IEntity, new()
    {
        protected readonly DbContext _context;
        public EfEntityRepositoryBase(DbContext context)
        {
            _context = context;
        }
        public async Task<Tentity> AddASync(Tentity entity)
        {
            await _context.Set<Tentity>().AddAsync(entity);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate)
        {
            return await _context.Set<Tentity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Tentity, bool>> predicate=null)
        {
            return await (predicate==null ? _context.Set<Tentity>().CountAsync() : _context.Set<Tentity>().CountAsync(predicate));
        }

        public async Task DeleteAsync(Tentity entity)
        {
            await Task.Run(() => _context.Set<Tentity>().Remove(entity));
        }

        public async Task<IList<Tentity>> GetAllAsync(Expression<Func<Tentity, bool>> predicate = null, params Expression<Func<Tentity, object>>[] includeProporties)
        {
            IQueryable<Tentity> query = _context.Set<Tentity>();
            if(predicate!=null)
            {
                query = query.Where(predicate);
            }
            if(includeProporties.Any())
            {
                foreach (var includeProperty in includeProporties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Tentity> GetAsync(Expression<Func<Tentity, bool>> predicate, params Expression<Func<Tentity, object>>[] includeProporties)
        {
            IQueryable<Tentity> query = _context.Set<Tentity>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includeProporties.Any())
            {
                foreach (var includeProperty in includeProporties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Tentity> UpdateAsync(Tentity entity)
        {
            await Task.Run(() => { _context.Set<Tentity>().Update(entity); });
            return entity;
        }
    }
}
