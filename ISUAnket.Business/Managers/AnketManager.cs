using ISUAnket.Business.Interfaces;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration; //anket linki oluşturmak için tanımlandı

        public AnketManager(IAnketRepository anketRepository, IConfiguration configuration)
        {
            _anketRepository = anketRepository;
            _configuration = configuration; //anket linki oluşturmak için tanımlandı
        }

        public async Task<List<Anket>> GetListAllServiceAsync()
        {
            return await _anketRepository.GetListAllAsync();
        }

        public async Task<List<Anket>> GetAllServiceAsync()
        {
            return await _anketRepository.GetAllAsync(
                a => a.AktifMi == true || a.AktifMi==false,
                a => a.OlusturanKullanici
            );
        }

        public async Task<List<Anket>> GetAllServiceAsync(Expression<Func<Anket, bool>> predicate, params Expression<Func<Anket, object>>[] includes)
        {
            return await _anketRepository.GetAllAsync(
                            a => a.AktifMi == true,
                            a => a.OlusturanKullanici 
                        );
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

        public async Task<int> AnketBaglantisiOlusturServiceAsyn(Anket anket)
        {
            await _anketRepository.AddAsync(anket);

            var siteUrl = _configuration["SiteUrl"] ?? ""; //appsetting.json içerisinden alınıyor
            anket.Link = $"{siteUrl}/Home/AnketDoldur?anketId={anket.Id}";

            await _anketRepository.UpdateAsync(anket);

            return anket.Id;
        }
    }
}
