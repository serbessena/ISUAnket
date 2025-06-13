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
    public class BirimManager : IBirimService
    {
        private readonly IBirimRepository _birimRepository;

        public BirimManager(IBirimRepository birimRepository)
        {
            _birimRepository = birimRepository;
        }

        public async Task AddServiceAsync(Birim entity)
        {
            await _birimRepository.AddAsync(entity);
        }

        

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _birimRepository.ChangeActivePasiveStatusAsync(id);
        }

        public async Task DeleteServiceAsync(Birim entity)
        {
            await _birimRepository.DeleteAsync(entity);
        }

        public Task<List<Birim>> GetAllServiceAsync(Expression<Func<Birim, bool>> predicate, params Expression<Func<Birim, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<Birim> GetByIdServiceAsync(int id)
        {
            return await _birimRepository.GetByIdAsync(id);
        }

        public async Task<List<Birim>> GetListAllServiceAsync()
        {
            return await _birimRepository.GetListAllAsync();
        }

        public async Task<List<Birim>> AktifBirimleriGetirServiceAsync()
        {
            return await _birimRepository.AktifBirimleriGetirAsync();
        }

        public async Task<List<Birim>> PasifBirimleriGetirServiceAsync()
        {
            return await _birimRepository.PasifBirimleriGetirAsync();
        }

        public async Task UpdateServiceAsync(Birim entity)
        {
            await _birimRepository.UpdateAsync(entity);
        }

        
    }
}
