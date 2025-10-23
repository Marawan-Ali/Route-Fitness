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
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
    }
}
