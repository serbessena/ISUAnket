using ISUAnket.DataAccess.Context;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Repositories
{
    public class MenuRolRepository:GenericRepository<MenuRol>,IMenuRolRepository
    {
        private readonly ISUAnketContext _context;

        public MenuRolRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MenuRol>> GetByRolIdAsync(int rolId)
        {
            return await _context.MenuRoller
                                 .Where(x=>x.RolId==rolId)
                                 .Include(x=>x.Menu)
                                 .ToListAsync();
        }
    }
}
