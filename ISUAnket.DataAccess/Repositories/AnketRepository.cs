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
    public class AnketRepository: GenericRepository<Anket>, IAnketDal
    {
        public AnketRepository(ISUAnketContext context) : base(context)
        {
                
        }
    }
}
