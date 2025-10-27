using GymSystemBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        } // Register Service in Program.cs

        #region Get All Plans

        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        #endregion

        #region Get Plan Details

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number !";
                return RedirectToAction("Index");
            }
            var Plan = _planService.GetPlanById(id);
            if(Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction("Index");
            }
            return View(Plan);
        }

        #endregion
    }
}
