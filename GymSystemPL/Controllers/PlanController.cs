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
    }
}
