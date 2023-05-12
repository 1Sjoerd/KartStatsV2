using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KartStatsV2.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int AdminUserId { get; set; }
    }

}