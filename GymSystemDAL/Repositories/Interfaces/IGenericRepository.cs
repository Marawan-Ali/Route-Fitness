using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    internal interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        // GetById
        TEntity? GetById(int id);

        // GetAll
        IEnumerable<TEntity> GetAll();

        // Add
        int Add(TEntity entity);

        // Update
        int Update(TEntity entity);

        // Delete
        int Delete(int id);
    }
}
