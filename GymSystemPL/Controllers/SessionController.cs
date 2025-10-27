using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Get Sessions

        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        #endregion

        #region Get Session Details

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction("Index");
            }

            var session = _sessionService.GetSessionById(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction("Index");
            }

            return View(session);
        }   

        #endregion
    }
}
