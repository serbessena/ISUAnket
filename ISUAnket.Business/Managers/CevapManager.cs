using ISUAnket.Business.Interfaces;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.DataAccess.Repositories;
using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Managers
{
    public class CevapManager : ICevapService
    {
        private readonly ICevapRepository _cevapRepository;

        public CevapManager(ICevapRepository cevapRepository)
        {
            _cevapRepository = cevapRepository;
        }

        public Task<List<Cevap>> GetListAllServiceAsync()
        {
            return _cevapRepository.GetListAllAsync();
        }

        public Task<List<Cevap>> GetAllServiceAsync(Expression<Func<Cevap, bool>> predicate, params Expression<Func<Cevap, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<Cevap> GetByIdServiceAsync(int id)
        {

            return await _cevapRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Cevap entity)
        {
            await _cevapRepository.AddAsync(entity);    
        }

        public async Task UpdateServiceAsync(Cevap entity)
        {
            await _cevapRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Cevap entity)
        {
            await _cevapRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _cevapRepository.ChangeActivePasiveStatusAsync(id);
        }
        
    }
}
