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
    public class MenuRolManager : IMenuRolService
    {
        private readonly IMenuRolRepository _menuRolRepository;

        public MenuRolManager(IMenuRolRepository menuRolRepository)
        {
            _menuRolRepository = menuRolRepository;
        }

        public async Task<List<MenuRol>> GetListAllServiceAsync()
        {
            return await _menuRolRepository.GetListAllAsync();
        }

        public async Task<List<MenuRol>> GetAllServiceAsync(Expression<Func<MenuRol, bool>> predicate, params Expression<Func<MenuRol, object>>[] includes)
        {
            return await _menuRolRepository.GetAllAsync(predicate, includes);
        }

        public async Task<MenuRol> GetByIdServiceAsync(int id)
        {
            return await _menuRolRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(MenuRol entity)
        {
            await _menuRolRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(MenuRol entity)
        {
            await _menuRolRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(MenuRol entity)
        {
            await _menuRolRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _menuRolRepository.ChangeActivePasiveStatusAsync(id);
        }

        public async Task<List<MenuRol>> GetByRolIdServiceAsync(int rolId)
        {
            return await _menuRolRepository.GetByRolIdAsync(rolId);
        }
    }
}
