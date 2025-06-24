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
        private readonly IAnketRepository _anketRepository;

        public SoruManager(ISoruRepository soruRepository, IAnketRepository anketRepository)
        {
            _soruRepository = soruRepository;
            _anketRepository = anketRepository;
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
            #region Ankete yayınlanma durumuna göre soru eklememe kontrolü

            if (entity.AnketId==null)
            {
                throw new ArgumentException(nameof(entity.AnketId), "Anket bilgisi boş olamaz!");
            }
            var anket=await _anketRepository.GetByIdAsync(entity.AnketId.Value);
            if (anket?.AnketDurumu==EntityLayer.Enums.AnketDurumuEnum.Yayınlandı)
            {
                throw new InvalidOperationException("Yayınlanmış ankete soru eklenemez!");
            }

            #endregion

            await _soruRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Soru entity)
        {
            #region Yayınlanmış ankete soru ekleyemem kontrolü

            if (entity.AnketId==null)
            {
                throw new ArgumentNullException(nameof(entity.AnketId), "Anket bilgisi boş olamaz!");
            }

            var anket = await _anketRepository.GetByIdAsync(entity.AnketId.Value);

            if (anket?.AnketDurumu==EntityLayer.Enums.AnketDurumuEnum.Yayınlandı)
            {
                throw new InvalidOperationException("Yayınlanmış anketteki soru güncellenemez!");
            }

            #endregion

            await _soruRepository.UpdateAsync(entity);
        }

        

        public async Task DeleteServiceAsync(Soru entity)
        {
            #region Yayınlanmış ankete soru silememe kontrolü

            if (entity.AnketId==null)
            {
                throw new ArgumentNullException(nameof(entity.AnketId), "Anket numarası boş olamaz!");
            }

            var anket = await _anketRepository.GetByIdAsync(entity.AnketId.Value);

            if (anket?.AnketDurumu==EntityLayer.Enums.AnketDurumuEnum.Yayınlandı)
            {
                throw new InvalidOperationException("Yayınlanmış anketteki soru silinemez!");
            }

            #endregion

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
