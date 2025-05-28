using ISUAnket.DataAccess.Interfaces;
using ISUAnket.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ISUAnket.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ISUAnketContext _context;

        public GenericRepository(ISUAnketContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeActivePasiveStatusAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                throw new Exception("Veri bulunamadı.");

            var property = typeof(T).GetProperty("AktifMi");
            if (property != null && property.PropertyType == typeof(bool))
            {
                bool currentValue = (bool)property.GetValue(entity);
                property.SetValue(entity, !currentValue);

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("AktifMi özelliği tanımlı değil.");
            }
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetListAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
