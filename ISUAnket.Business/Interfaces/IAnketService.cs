using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IAnketService:IGenericService<Anket>
    {
        Task<List<Anket>> GetAllServiceAsync();

        /// <summary>
        /// anket linki oluşturur
        /// </summary>
        /// <param name="anket"></param>
        /// <returns></returns>
        Task<int> AnketBaglantisiOlusturServiceAsyn(Anket anket);
    }
}
