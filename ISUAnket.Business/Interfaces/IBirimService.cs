using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IBirimService:IGenericService<Birim>
    {
        Task<List<Birim>> AktifBirimleriGetirServiceAsync();
        Task<List<Birim>> PasifBirimleriGetirServiceAsync();

    }
}
