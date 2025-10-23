using GymSystemBLL.Services.Interfaces;
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
            if (id <=0)
            {
                return RedirectToAction(nameof(Index));
            }

            var memberDetails = _memberService.GetMemberDetails(id);
            if (memberDetails == null)
            {
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
                return RedirectToAction(nameof(Index));
            }
            var healthRecordDetails = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecordDetails == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecordDetails);
        }

        #endregion
    }
}
