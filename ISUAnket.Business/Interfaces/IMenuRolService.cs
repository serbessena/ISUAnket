using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IMenuRolService:IGenericService<MenuRol>
    {
        Task<List<MenuRol>> GetByRolIdServiceAsync(int rolId);
    }
}
