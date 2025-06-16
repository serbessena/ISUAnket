using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface ISoruRepository : IGenericRepository<Soru>
    {
        /// <summary>
        /// ankete bağlı soruları liste şeklinde getirir
        /// </summary>
        /// <param name="anketId"></param>
        /// <returns></returns>
        Task<List<Soru>> GetSorularByAnketIdAsync(int anketId);
    }
}
