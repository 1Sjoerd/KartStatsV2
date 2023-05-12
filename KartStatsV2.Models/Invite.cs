using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.Models
{
    public class Invite
    {
        public int InviteId { get; set; }
        public int GroupId { get; set; }
        public int FromUserId { get; set; } 
        public int ToUserId { get; set; } 
        public string Status { get; set; } 
    }

    public enum InviteStatus
    {
        Pending = 1,
        Accepted = 2,
        Declined = 3
    }
}
