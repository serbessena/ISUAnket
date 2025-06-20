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
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        private readonly ISUAnketContext _context;

        public MenuRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetMenusByRolIdAsync(int rolId)
        {
            return await _context.MenuRoller
                                 .Where(x => x.RolId == rolId && x.Menu.AktifMi)
                                 .Select(x => x.Menu)
                                 .OrderBy(x => x.Sira)
                                 .ToListAsync();
        }
    }
}
