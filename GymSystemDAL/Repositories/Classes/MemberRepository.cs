using GymSystemDAL.Data.Contexts;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemDAL.Repositories.Classes
{
    internal class MemberRepository : IMemberRepository
    {
        // Connection with DB
        //private readonly GymSystemDbContext _dbContext = new GymSystemDbContext();

        // Connection Dynamic With DB => CLR Generate

        private readonly GymSystemDbContext _dbContext;

        public MemberRepository(GymSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = _dbContext.Members.Find(id);
            if (member is null) return 0;

            _dbContext.Members.Remove(member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll()
        {
            return _dbContext.Members.ToList();
        }

        public Member? GetById(int id)
        {
            return _dbContext.Members.Find(id);
        }

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
