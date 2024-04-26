using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackJob.Worker
{
    public class JobOptions
    {
        public string LineId { get; set; }

        public string Corn { get; set; }

        public bool Enabled { get; set; }

        public string DataSourceName { get; set; }
    }
}
