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
    public class RolRepository: GenericRepository<Rol>, IRolRepository
    {
        private readonly ISUAnketContext _context;

        public RolRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Rol>> AktifRolleriGetirAsync()
        {
            return await _context.Roller.Where(x => x.AktifMi == true).ToListAsync();
        }

        public async Task<List<Rol>> PasifRolleriGetirAsync()
        {
            return await _context.Roller.Where(x=>x.AktifMi==false).ToListAsync();
        }
    }
}
