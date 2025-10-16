using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (Sessions == null || !Sessions.Any()) return [];

            return Sessions.Select(s => new SessionViewModel
            {
                Id = s.Id,
                CategoryName = s.SessionCategory.CategoryName,
                Description = s.Description,
                TrainerName = s.SessionTrainer.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id)
            });
        }
    }
}
