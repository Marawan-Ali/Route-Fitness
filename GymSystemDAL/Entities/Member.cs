using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Entities
{
    internal class Member : GymUser
    {
        // CreatedAt Column in BaseEntity
        // Will be used as JoinDate for Member => Configurations

        public string? Photo { get; set; }
    }
}
