using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {
        public ActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainerAndCategories();
            return View(sessions);
        }

        public ActionResult GetMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetAllMembersForUpcomingSessions(id);
            return View(members);
        }
    }
}
