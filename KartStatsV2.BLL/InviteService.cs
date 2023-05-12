using KartStatsV2.BLL.Interfaces;
using KartStatsV2.DAL.Interfaces;
using KartStatsV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.BLL
{
    public class InviteService : IInviteService
    {
        private readonly IInviteRepository _inviteRepository;

        public InviteService(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public void CreateInvite(Invite invite)
        {
            _inviteRepository.CreateInvite(invite);
        }

        public List<Invite> GetInvitesByToUserId(int toUserId)
        {
            return _inviteRepository.GetInvitesByToUserId(toUserId);
        }

        public void UpdateInviteStatus(int inviteId, string status)
        {
            _inviteRepository.UpdateInviteStatus(inviteId, status);
        }

        public Invite GetInvite(int inviteId)
        {
            return _inviteRepository.GetInvite(inviteId);
        }

        public bool UpdateInvite(Invite invite)
        {
            return _inviteRepository.UpdateInvite(invite);
        }
    }
}
