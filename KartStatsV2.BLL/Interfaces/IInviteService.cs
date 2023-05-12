using KartStatsV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.BLL.Interfaces
{
    public interface IInviteService
    {
        void CreateInvite(Invite invite);
        List<Invite> GetInvitesByToUserId(int toUserId);
        void UpdateInviteStatus(int inviteId, string status);
        Invite GetInvite(int inviteId);
        bool UpdateInvite(Invite invite);
    }
}
