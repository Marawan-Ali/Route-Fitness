using AutoMapper;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check If Trainer exists
                // Check If Category exists
                // Check If StartDate < EndDate
                if (!IsTrainerExists(createdSession.TrainerId)) return false;
                if (!IsCategoryExists(createdSession.CategoryId)) return false;
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
                if (createdSession.Capacity < 0 || createdSession.Capacity > 25) return false;

                var SessionEntity = _mapper.Map<Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
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

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (Session == null) return null;

            ///return new SessionViewModel
            ///{
            ///    Id = Session.Id,
            ///    CategoryName = Session.SessionCategory.CategoryName,
            ///    Description = Session.Description,
            ///    TrainerName = Session.SessionTrainer.Name,
            ///    StartDate = Session.StartDate,
            ///    EndDate = Session.EndDate,
            ///    Capacity = Session.Capacity,
            ///    AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id)
            ///};

            // Allow AutoMapper
            var MappedSession = _mapper.Map<SessionViewModel>(Session);
            MappedSession.AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);
            return MappedSession;
        }

        #region HelperMethods

        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) != null;
        }

        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) != null;
        }

        private bool IsDateTimeValid(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate;
        }

        #endregion
    }
}
