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
    public class KullaniciManager : IKullaniciService
    {
        private readonly IKullaniciRepository _kullaniciRepository;

        public KullaniciManager(IKullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<List<Kullanici>> GetListAllServiceAsync()
        {
            return await _kullaniciRepository.GetListAllAsync();
        }

        public async Task<List<Kullanici>> GetAllServiceAsync(Expression<Func<Kullanici, bool>> predicate)
        {
            return await _kullaniciRepository.GetAllAsync(predicate);
        }

        public async Task<Kullanici> GetByIdServiceAsync(int id)
        {
            return await _kullaniciRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Kullanici entity)
        {
            await _kullaniciRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _kullaniciRepository.ChangeActivePasiveStatusAsync(id);
        }

    }
}
