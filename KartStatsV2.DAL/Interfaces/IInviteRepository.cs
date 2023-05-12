using KartStatsV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.DAL.Interfaces
{
    public interface IInviteRepository
    {
        void CreateInvite(Invite invite);
        List<Invite> GetInvitesByToUserId(int toUserId);
        void UpdateInviteStatus(int inviteId, string status);
        void InviteUserToGroup(int groupId, string fromUserId, string toUserId);
        void AcceptInvite(int inviteId);
        void DeclineInvite(int inviteId);
        Invite GetInvite(int inviteId);
        bool UpdateInvite(Invite invite);
    }
}
