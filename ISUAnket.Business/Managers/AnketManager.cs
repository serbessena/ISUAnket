using ISUAnket.Business.Interfaces;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Managers
{
    public class AnketManager : IAnketService
    {
        private readonly IAnketRepository _anketRepository;

        public AnketManager(IAnketRepository anketRepository)
        {
            _anketRepository = anketRepository;
        }

        public async Task<List<Anket>> GetListAllServiceAsync()
        {
            return await _anketRepository.GetListAllAsync();
        }

        public async Task<List<Anket>> GetAllServiceAsync(Expression<Func<Anket, bool>> predicate)
        {
            return await _anketRepository.GetAllAsync(predicate);
        }

        public async Task<Anket> GetByIdServiceAsync(int id)
        {
            return await _anketRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Anket entity)
        {
            await _anketRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Anket entity)
        {
            await _anketRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Anket entity)
        {
            await _anketRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _anketRepository.ChangeActivePasiveStatusAsync(id);
        }
    }
}
