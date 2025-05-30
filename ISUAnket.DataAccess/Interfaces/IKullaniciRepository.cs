using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface IKullaniciRepository : IGenericRepository<Kullanici>
    {
        /// <summary>
        /// kullanıcıları ve bağlı olduğu rolleri liste şeklinde getirir
        /// </summary>
        /// <returns></returns>
        Task<List<Kullanici>> KullanicilaraGoreRolListesiAsync();
    }
}
