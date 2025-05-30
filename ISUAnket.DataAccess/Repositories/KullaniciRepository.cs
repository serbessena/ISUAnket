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
    public class KullaniciRepository: GenericRepository<Kullanici>, IKullaniciRepository
    {
        private readonly ISUAnketContext _context;

        public KullaniciRepository(ISUAnketContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// kullanıcıların ve bağlı olduğu rolleri liste şeklinde getirir
        /// </summary>
        /// <returns></returns>
        public async Task<List<Kullanici>> KullanicilaraGoreRolListesiAsync()
        {
            return await _context.Kullanicilar
                                 .Include(k => k.Rol)
                                 .ToListAsync();
        }
    }
}
