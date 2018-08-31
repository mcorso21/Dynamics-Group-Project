using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public enum RegionEnum { US = 283210000, Canada = 283210001 };

    public enum ApprovalEnum { New = 283210000, Review = 283210001, Approve = 283210002 };

    public class MortgageModel
    {
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        //public string MortgageNumber { get; set; }
        public RegionEnum Region { get; set; }
        public decimal MortgageAmount { get; set; }
        //public decimal MortgagePayment { get; set; }
        public int MortgageTermInMonths { get; set; }
        public ApprovalEnum Approval { get; set; }
        public List<FileStream> IdentityDocuments { get; set; }
    }
}
