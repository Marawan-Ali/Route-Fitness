using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemBLL.ViewModels.MemberViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Classes;
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
        #region Fields

        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRepository;
        private readonly IGenericRepository<MemberSession> _memberSessionRepository;

        #endregion

        public MemberService(IGenericRepository<Member> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IPlanRepository planRepository,
            IGenericRepository<HealthRecord> healthRepository,
            IGenericRepository<MemberSession> memberSessionRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _healthRepository = healthRepository;
            _memberSessionRepository = memberSessionRepository;
        }

        public bool CreateMembers(CreateMemberViewModel createdMember)
        {
            try
            {
                // Check if Email and Email are unique
                if (IsEmailExists(createdMember.Email) || IsPhoneExists(createdMember.Phone)) return false;

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

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            // IPlanRepository
            // Inject for PlanRepo and MembershipRepo
            var Member = _memberRepository.GetById(memberId);
            if (Member is null) return null;

            var viewModel = new MemberViewModel()
            {
                Id = Member.Id,
                Photo = Member.Photo,
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber}, {Member.Address.Street}, {Member.Address.City}",
            };

            var ActiveMembership = _membershipRepository
                .GetAll(m => m.MemberId == memberId && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership is not null) // StartDate , EndDate
            {
                viewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                viewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

                // Plans
                var Plan = _planRepository.GetById(ActiveMembership.PlanId);
                viewModel.PlanName = Plan?.Name;
            }
            return viewModel;
        }

        public HealthViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var MemberHealthRecord = _healthRepository.GetById(memberId);
            if (MemberHealthRecord is null) return null;

            return new HealthViewModel()
            {
                Weight = MemberHealthRecord.Weight,
                Height = MemberHealthRecord.Height,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var Member = _memberRepository.GetById(memberId);
            if (Member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Photo = Member.Photo,
                Email = Member.Email,
                Phone = Member.Phone,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City
            };
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel updatedMember)
        {
            try
            {
                if (IsEmailExists(updatedMember.Email) || IsPhoneExists(updatedMember.Phone)) return false;

                var Member = _memberRepository.GetById(id);
                if (Member is null) return false;

                Member.Email = updatedMember.Email;
                Member.Phone = updatedMember.Phone;
                Member.Address.BuildingNumber = updatedMember.BuildingNumber;
                Member.Address.Street = updatedMember.Street;
                Member.Address.City = updatedMember.City;
                Member.UpdatedAt = DateTime.Now;
                return _memberRepository.Update(Member) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveMember(int memberId)
        {
            var Member = _memberRepository.GetById(memberId);
            if (Member is null) return false;

            // Check if member has active sessions or not
            var HasActiveMemberSessions = _memberSessionRepository
                .GetAll(ms => ms.MemberId == memberId && ms.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions) return false;

            // Remove
            // Handle to Cascade Action in Code
            var Membership = _membershipRepository.GetAll(m => m.MemberId == memberId);
            try
            {
                if (Membership.Any())
                {
                    foreach (var membership in Membership)
                    {
                        _membershipRepository.Delete(membership);
                    }
                }
                return _memberRepository.Delete(Member) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Helper Methods

        private bool IsEmailExists(string email)
        {
            return _memberRepository.GetAll(m => m.Email == email).Any();
        }

        private bool IsPhoneExists(string phone)
        {
            return _memberRepository.GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
