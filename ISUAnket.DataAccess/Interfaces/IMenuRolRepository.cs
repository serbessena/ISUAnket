﻿using ISUAnket.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.DataAccess.Interfaces
{
    public interface IMenuRolRepository:IGenericRepository<MenuRol>
    {
        Task<List<MenuRol>> GetByRolIdAsync(int rolId);
    }
}
