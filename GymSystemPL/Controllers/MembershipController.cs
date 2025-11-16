using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public ActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships);
        }

        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateMembershipViewModel createdMembership)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Result = _membershipService.CreateMembership(createdMembership);
                    if (Result)
                    {
                        TempData["SuccessMessage"] = "Membership created successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to create membership. Please try again.";
                        return RedirectToAction("Index");
                    }
                }
                TempData["ErrorMessage"] = "Invalid data. Please correct the errors and try again.";
                LoadDropdowns();
                return View(createdMembership);
            }
            catch
            {
                LoadDropdowns();
                return View();
            }
        }

        #region Helper Methods

        public void LoadDropdowns()
        {
            var members = _membershipService.GetMembersForDropdown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
            var plans = _membershipService.GetPlansForDropdown();
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }

        #endregion
    }
}
