using UDEMY_PROJECT.Data;
using UDEMY_PROJECT.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;
using UDEMY_PROJECT.Services.Exceptions;

namespace UDEMY_PROJECT.Services
{
    public class RepositoryService<T> where T : class
    {
        private readonly UDEMY_PROJECTContext _context;

        public RepositoryService(UDEMY_PROJECTContext context)
        {
            _context = context;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetOneAsync(int? id, Expression<Func<T, object>>? lambda ,Expression<Func<T, bool>>? func) 
        {
            if(lambda == null || func == null)
            {
                return await _context.Set<T>().FindAsync(id);
            }
            return await _context.Set<T>().AsQueryable().Include(lambda).FirstOrDefaultAsync(func);
        }

        public async Task AddAsync(T obj)
        {
            _context.Set<T>().Add(obj);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T obj)
        {
            _context.Set<T>().Update(obj);

            await _context.SaveChangesAsync();
        }
        public async Task RemoveAsync(T obj)
        {
            try
            {
                _context.Set<T>().Remove(obj);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                throw new IntegrityException(ex.Message);
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> pred) 
        {
            return await _context.Set<T>().AnyAsync(pred);
        }

        public async Task<ICollection<T>> GetByDateAsync(DateTime? dateInitial, DateTime? dateFinal, Expression<Func<T, bool>> funcInitial, Expression<Func<T, bool>> funcFinal, Expression<Func<T, Object>> includeFuncSeller, Expression<Func<T, Object>> includeFuncDepartment, Expression<Func<T, DateTime>> orderFunc)
        {
            var result = from obj in _context.Set<T>() select obj;

            if (dateInitial.HasValue)
            {
                result = result.Where(funcInitial);
            }
            if (dateFinal.HasValue)
            {
                result = result.Where(funcFinal);
            }

            return await result.Include(includeFuncSeller).Include(includeFuncDepartment).OrderByDescending(orderFunc).ToListAsync();
        }

        public async Task<ICollection<IGrouping<object, T>>> GetByDateByGroupAsync(DateTime? dateInitial, DateTime? dateFinal, Expression<Func<T, bool>> funcInitial, Expression<Func<T, bool>> funcFinal, Expression<Func<T, Object>> includeFuncSeller, Expression<Func<T, Object>> includeFuncDepartment, Expression<Func<T, DateTime>> orderFunc)
        {
            var result = from obj in _context.Set<T>() select obj;

            if (dateInitial.HasValue)
            {
                result = result.Where(funcInitial);
            }
            if (dateFinal.HasValue)
            {
                result = result.Where(funcFinal);
            }

            return await result.Include(includeFuncSeller).Include(includeFuncDepartment).OrderByDescending(orderFunc).GroupBy(includeFuncDepartment).ToListAsync();
        }
    }
}
