using AutoMapper;
using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.BookingViewModels;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberForSessionViewModel> GetAllMembersForUpcomingSessions(int id)
        {
            var bookingRepository = _unitOfWork.BookingRepository;
            var bookings = bookingRepository.GetSessionById(id);
            var memberViewModels = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(bookings);
            return memberViewModels;
        }

        public IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategories()
        {
            var sessionRepository = _unitOfWork.SessionRepository;
            var sessions = sessionRepository.GetAllSessionsWithTrainerAndCategory();
            var sessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in sessionViewModels)
            {
                session.AvailableSlots = session.Capacity - sessionRepository.GetCountOfBookedSlots(session.Id);
            }
            return sessionViewModels;
        }
    }
}
