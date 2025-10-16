using GymSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
    }
}
