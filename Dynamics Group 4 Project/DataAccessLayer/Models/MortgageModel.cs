using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Mortgage Number")]
        public string MortgageNumber { get; set; }
        [Display(Name = "Region")]
        [Range(283210000, 283210010, ErrorMessage = "Select a Region.")]
        public RegionEnum Region { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public decimal MortgageAmount { get; set; }
        [Required]
        [Display(Name = "Term (Months)")]
        public int MortgageTermInMonths { get; set; }
        [Display(Name = "Approval Status")]
        public ApprovalEnum Approval { get; set; }
        public List<FileStream> IdentityDocuments { get; set; }
    }
}
