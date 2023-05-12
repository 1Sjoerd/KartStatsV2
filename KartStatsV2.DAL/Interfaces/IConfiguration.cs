using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartStatsV2.DAL.Interfaces
{
    public interface IConfiguration
    {
        string ConnectionString { get; }
    }

    public class Configuration : IConfiguration
    {
        public string ConnectionString { get; private set; }

        public Configuration(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
