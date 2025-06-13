using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface IBirimRepository : IGenericRepository<Birim>
    {
        Task<List<Birim>> AktifBirimleriGetirAsync();
        Task<List<Birim>> PasifBirimleriGetirAsync();
    }
}
