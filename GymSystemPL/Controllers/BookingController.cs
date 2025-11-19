using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class BookingController(IBookingService bookingService) : Controller
    {
        public ActionResult Index()
        {
            var sessions = bookingService.GetAllSessionsWithTrainerAndCategories();
            return View(sessions);
        }
    }
}
