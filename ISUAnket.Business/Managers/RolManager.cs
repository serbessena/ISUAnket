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
    public class RolManager : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolManager(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<List<Rol>> GetListAllServiceAsync()
        {
            return await _rolRepository.GetListAllAsync();
        }

        public async Task<List<Rol>> GetAllServiceAsync(Expression<Func<Rol, bool>> predicate)
        {
            return await _rolRepository.GetAllAsync(predicate);
        }

        public async Task<Rol> GetByIdServiceAsync(int id)
        {
            return await _rolRepository.GetByIdAsync(id);
        }

        public async Task AddServiceAsync(Rol entity)
        {
            await _rolRepository.AddAsync(entity);
        }

        public async Task UpdateServiceAsync(Rol entity)
        {
            await _rolRepository.UpdateAsync(entity);
        }

        public async Task DeleteServiceAsync(Rol entity)
        {
            await _rolRepository.DeleteAsync(entity);
        }

        public async Task ChangeActivePasiveStatusServiceAsync(int id)
        {
            await _rolRepository.ChangeActivePasiveStatusAsync(id);
        }
        
    }
}
