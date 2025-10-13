using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Interfaces
{
    internal interface IMemberRepository
    {
        // GetAll Members
        IEnumerable<Member> GetAll();

        // Get Member by ID
        Member GetById(int id);

        // Add Member
        int Add(Member member);

        // Delete Member
        int Delete(int id);

        // Update Member
        int Update(Member member);
    }
}
