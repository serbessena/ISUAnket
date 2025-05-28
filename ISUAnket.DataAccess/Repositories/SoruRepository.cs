using ISUAnket.DataAccess.Context;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Repositories
{
    public class SoruRepository: GenericRepository<Soru>, ISoruRepository
    {
        public SoruRepository(ISUAnketContext context) : base(context)
        {
                
        }
    }
}
