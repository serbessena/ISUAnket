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
    public class MenuManager : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuManager(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<List<Menu>> GetListAllServiceAsync()
        {
            return await _menuRepository.GetListAllAsync();
        }

        public async Task<List<Menu>> GetAllServiceAsync(Expression<Func<Menu, bool>> predicate, params Expression<Func<Menu, object>>[] includes)
        {
            return await _menuRepository.GetAllAsync(predicate, includes);
        }

        public async Task<Menu> GetByIdServiceAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Menu entity)
        {
            await _menuRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Menu entity)
        {
            await _menuRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Menu entity)
        {
            await _menuRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _menuRepository.ChangeActivePasiveStatusAsync(id);
        }

        public async Task<List<Menu>> GetMenusByRolIdServiceAsync(int rolId)
        {
            return await _menuRepository.GetMenusByRolIdAsync(rolId);
        }

    }
}
