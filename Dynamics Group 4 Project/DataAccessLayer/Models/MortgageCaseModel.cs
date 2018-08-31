using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MortgageCaseModel
    {
        public string ProblemDescription { get; set; }
        public string Priority { get; set; }
        public string HighPriorityReason { get; set; }
        public string Type { get; set; }
    }
}
