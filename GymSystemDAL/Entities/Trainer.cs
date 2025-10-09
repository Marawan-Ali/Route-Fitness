using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Trainer : GymUser
    {
        // CreatedAt Column in BaseEntity
        // Will be used as HireDate for Trainer => Configurations

        public Specialities Specialities { get; set; }
    }
}
