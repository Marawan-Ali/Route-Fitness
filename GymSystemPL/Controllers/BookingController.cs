using GymSystemBLL.Services.Interfaces;
using GymSystemDAL.Entities;
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
            var members = _bookingService.GetAllMembersForSession(id);
            return View(members);
        }
        public ActionResult GetMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetAllMembersForSession(id);
            ViewBag.SessionId = id;
            return View(members);
        }

        [HttpPost]
        public ActionResult MarkAttendance(int sessionId,int memberId)
        {
            _bookingService.MarkMemberAttendance(memberId, sessionId);
            return RedirectToAction("GetMembersForOngoingSession", new { id = sessionId });
        }
    }
}
