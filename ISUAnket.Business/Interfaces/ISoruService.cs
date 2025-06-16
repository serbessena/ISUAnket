using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface ISoruService:IGenericService<Soru>
    {
        Task<List<Soru>> GetAllServiceAsync();

        /// <summary>
        /// anket içerisindeki soruları getirir
        /// </summary>
        /// <param name="anketId"></param>
        /// <returns></returns>
        Task<List<Soru>> GetSorularByAnketIdServiceAsync(int anketId);
    }
}
