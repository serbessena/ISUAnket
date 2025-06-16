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
    public class SoruRepository: GenericRepository<Soru>, ISoruRepository
    {
        private readonly ISUAnketContext _context;
        public SoruRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Soru>> GetSorularByAnketIdAsync(int anketId)
        {
            return await _context.Sorular
                                 .Where(x => x.AnketId == anketId)
                                 .OrderBy(x => x.Id)
                                 .ToListAsync();
        }
    }
}
