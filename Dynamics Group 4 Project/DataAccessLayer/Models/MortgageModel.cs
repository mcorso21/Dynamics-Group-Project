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

    public enum TermEnum { Ten = 283210000, Fifteen = 283210001, Twenty = 283210002, Thirty = 283210003 };

    public class MortgageModel
    {
        public Guid ContactId { get; set; }
        public Guid MortgageId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Mortgage Number")]
        public string MortgageNumber { get; set; }
        [Display(Name = "Region")]
        [Range(283210000, 283210010, ErrorMessage = "Select a Region.")]
        public RegionEnum Region { get; set; }
        public string State { get; set; }
        [Required]
        [Display(Name = "Amount")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [Range(1, 2000000000, ErrorMessage = "Mortgage amount must be between $1 and $2,000,000,000")]
        public decimal MortgageAmount { get; set; }
        [Required]
        //[Range(283210000, 283210005, ErrorMessage = "Select a Term.")]
        [Display(Name = "Apr")]
        public decimal Apr { get; set; }
        [Display(Name = "Term (Years)")]
        public TermEnum MortgageTermInYears { get; set; }
        [Display(Name = "Approval Status")]
        public ApprovalEnum Approval { get; set; }
        public List<FileStream> IdentityDocuments { get; set; }

    }
}
