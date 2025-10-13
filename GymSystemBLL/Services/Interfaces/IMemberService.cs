using GymSystemBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMembers(CreateMemberViewModel createdMember);

        MemberViewModel? GetMemberDetails(int memberId);
    }
}
