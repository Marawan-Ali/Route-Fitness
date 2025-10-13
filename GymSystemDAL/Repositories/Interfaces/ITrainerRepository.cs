using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    internal interface ITrainerRepository
    {
        // GetAll Trainers
        IEnumerable<Trainer> GetAll();

        // Get Trainer by ID
        Trainer GetById(int id);

        // Add Trainer
        int Add(Trainer trainer);

        // Delete Trainer
        int Delete(int id);

        // Update Trainer
        int Update(Trainer trainer);
    }
}
