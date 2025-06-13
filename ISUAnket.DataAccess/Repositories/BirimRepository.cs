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
    public class BirimRepository: GenericRepository<Birim>, IBirimRepository
    {
        private readonly ISUAnketContext _context;
        public BirimRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Birim>> AktifBirimleriGetirAsync()
        {
            return await _context.Birimler.Where(x => x.AktifMi == true).ToListAsync();
        }

        public async Task<List<Birim>> PasifBirimleriGetirAsync()
        {
            return await _context.Birimler.Where(x => x.AktifMi == false).ToListAsync();
        }
    }
}
