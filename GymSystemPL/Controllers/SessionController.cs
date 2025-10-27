using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns();
                return View(createdSession);
            }

            var Result = _sessionService.CreateSession(createdSession);
            if (!Result)
            {
                TempData["ErrorMessage"] = "Failed to Create Session";
                LoadDropDowns();
                return View(createdSession);
            }
            else
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                LoadDropDowns();
                return RedirectToAction("Index");
            }
        }

        #endregion

        private void LoadDropDowns()
        {
            var Trainers = _sessionService.GetTrainerForSessions();
            var Categories = _sessionService.GetCategoryForSessions();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
    }
}
