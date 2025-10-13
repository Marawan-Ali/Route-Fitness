using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            #region First Way Of Mapping

            //    //var members = _memberRepository.GetAll() ?? [];
            //    var Members = _memberRepository.GetAll() ?? [];
            //    if (Members is null || Members.Any()) return [];

            //    var MemberViewModels = new List<MemberViewModel>();
            //    foreach (var Member in Members)
            //    {
            //        var memberViewModel = new MemberViewModel()
            //        {
            //            Id = Member.Id,
            //            Photo = Member.Photo,
            //            Name = Member.Name,
            //            Email = Member.Email,
            //            Phone = Member.Phone,
            //            Gender = Member.Gender.ToString();
            //        };
            //        MemberViewModels.Add(memberViewModel);
            //    }
            //    return MemberViewModels;

            #endregion

            var Members = _memberRepository.GetAll();
            if (Members is null || !Members.Any()) return [];

            var MemberViewModels = Members.Select(Member => new MemberViewModel()
            {
                Id = Member.Id,
                Photo = Member.Photo,
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString()
            });
            return MemberViewModels;
        }
    }
}
