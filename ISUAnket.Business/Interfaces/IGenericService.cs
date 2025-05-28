using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Business.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        /// <summary>
        /// veriyi veritabanına ekler
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddServiceAsync(T entity);

        /// <summary>
        /// veriyi günceller
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateServiceAsync(T entity);

        /// <summary>
        /// veriyi veritabanından siler
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteServiceAsync(T entity);

        /// <summary>
        /// id değerine göre veriye ait detayları getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdServiceAsync(int id);
        Task<List<T>> GetListAllServiceAsync();

        /// <summary>
        /// verileri filtreli olarak listeler
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> GetAllServiceAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Verinin aktiflik durumunu değiştirir (true/false toggle).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ChangeActivePasiveStatusServiceAsync(int id);
    }
}
