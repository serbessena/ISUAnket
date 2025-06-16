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

        public async Task<List<Soru>> GetAllServiceAsync()
        {
            return await _soruRepository.GetAllAsync(
                a => a.AktifMi == true || a.AktifMi == false,
                a => a.Anket,
                a => a.OlusturanKullanici
            );
        }
        
        public async Task<List<Soru>> GetAllServiceAsync(Expression<Func<Soru, bool>> predicate, params Expression<Func<Soru, object>>[] includes)
        {
            return await _soruRepository.GetAllAsync(
                             a => a.AktifMi == true,
                             a => a.OlusturanKullanici
                         );
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

        public async Task<List<Soru>> GetSorularByAnketIdServiceAsync(int anketId)
        {
            return await _soruRepository.GetSorularByAnketIdAsync(anketId);
        }
    }
}
