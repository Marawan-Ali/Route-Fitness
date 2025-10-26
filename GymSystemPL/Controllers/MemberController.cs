using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        // Ask CLR to inject Object from MemberService
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        } // Register Service in Program.cs

        #region Get All Members

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region Get Member Details

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var memberDetails = _memberService.GetMemberDetails(id);
            if (memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        #endregion

        #region Get Health Record Details

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id cannot be 0 or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var healthRecordDetails = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecordDetails == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecordDetails);
        }

        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }

        // Add to DB
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data and Missing Fields !");
                return View("Create", createdMember);
            }

            bool Result = _memberService.CreateMembers(createdMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member !";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
