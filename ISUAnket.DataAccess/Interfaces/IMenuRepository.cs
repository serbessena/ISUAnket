using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface IMenuRepository:IGenericRepository<Menu>
    {
        /// <summary>
        /// rolId'ye göre menüleri getirir
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        Task<List<Menu>> GetMenusByRolIdAsync(int rolId);
    }
}
