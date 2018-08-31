using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MortgageModel
    {
        public string MortgageNumber { get; set; }
        public string Region { get; set; }
        public double MortgageAmount { get; set; }
        public double MortgagePayment { get; set; }
        public int MortgageTermInMonths { get; set; }
        public string Approval { get; set; }
        public List<FileStream> IdentityDocuments { get; set; }
    }
}
