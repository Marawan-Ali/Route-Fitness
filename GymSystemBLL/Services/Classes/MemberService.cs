using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
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

        public bool CreateMembers(CreateMemberViewModel createdMember)
        {
            try
            {
                // Check if Email and Email are unique
                var EmailExists = _memberRepository.GetAll(m => m.Email == createdMember.Email).Any();
                var PhoneExists = _memberRepository.GetAll(m => m.Phone == createdMember.Phone).Any();
                if (EmailExists || PhoneExists) return false;

                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    DateOfBirth = createdMember.DateOfBirth,
                    Gender = createdMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        Street = createdMember.Street,
                        City = createdMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Weight = createdMember.HealthViewModel.Weight,
                        Height = createdMember.HealthViewModel.Height,
                        BloodType = createdMember.HealthViewModel.BloodType,
                        Note = createdMember.HealthViewModel.Note
                    }
                };
                return _memberRepository.Add(member) > 0;
            }
            catch (Exception)
            {
                return false;
            }
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
