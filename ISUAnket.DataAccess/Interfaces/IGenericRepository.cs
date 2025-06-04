using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// veritabanına veri ekler
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(T entity);

        /// <summary>
        /// id değerine göre veriyi günceller
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// id değerine göre veriyi siler
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// id değerine göre bir adet veriye ait detayları getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// verileri liste şeklinde getirir
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAllAsync();

        /// <summary>
        /// verileri filtreli olarak listeler
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Verinin aktiflik durumunu değiştirir (true/false toggle).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ChangeActivePasiveStatusAsync(int id);

    }
}
