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
    public class SoruManager : ISoruService
    {
        private readonly ISoruRepository _soruRepository;

        public SoruManager(ISoruRepository soruRepository)
        {
            _soruRepository = soruRepository;
        }

        public async Task<List<Soru>> GetListAllServiceAsync()
        {
            return await _soruRepository.GetListAllAsync();
        }

        public async Task<Soru> GetByIdServiceAsync(int id)
        {
            return await _soruRepository.GetByIdAsync(id);
        }

        public Task<List<Soru>> GetAllServiceAsync(Expression<Func<Soru, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task AddServiceAsync(Soru entity)
        {
            await _soruRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Soru entity)
        {
            await _soruRepository.UpdateAsync(entity);
        }

        

        public async Task DeleteServiceAsync(Soru entity)
        {
            await _soruRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _soruRepository.ChangeActivePasiveStatusAsync(id);
        }
        
    }
}
