using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IRolService:IGenericService<Rol>
    {
        Task<List<Rol>> AktifRolleriGetirServiceAsync();
        Task<List<Rol>> PasifRolleriGetirServiceAsync();
    }
}
