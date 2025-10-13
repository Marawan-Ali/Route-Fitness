using GymSystemDAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        // GetById
        TEntity? GetById(int id);

        // GetAll
        IEnumerable<TEntity> GetAll(Func<TEntity ,bool> condition = null);

        // Add
        int Add(TEntity entity);

        // Update
        int Update(TEntity entity);

        // Delete
        int Delete(TEntity entity);
    }
}
