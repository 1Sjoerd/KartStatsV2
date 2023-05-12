using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.Models
{
    public class LapTime
    {
        public int UserId { get; set; }
        public int CircuitId { get; set; }
        public DateTime DateTime { get; set; }
        public double Time { get; set; }
    }
}
