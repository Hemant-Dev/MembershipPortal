﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipPortal.IRepositories
{
    public interface IRepository <T> where T : class
    {

        Task<IEnumerable<T>> GetAsyncAll();

        Task<T> GetAsyncById(int id);
        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

       


    }
}
