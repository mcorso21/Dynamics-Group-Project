using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public enum PriorityEnum { High = 1, Normal = 2, Low = 3 };

    public enum TypeEnum { Mortgage = 283210000, Other = 283210001 }

    public class MortgageCaseModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Mortgage")]
        public Guid MortgageId { get; set; }
        [Display(Name = "Mortgage Number")]
        public string MortgageNumber { get; set; }
        [Display(Name = "Mortgage Name")]
        public string MortgageName { get; set; }
        [Display(Name = "Dynamics Contact ID")]
        public Guid ContactId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Select a Priority.")]
        [Display(Name = "Priority")]
        public PriorityEnum Priority { get; set; }
        [Display(Name = "High Priority Reason")]
        public string HighPriorityReason { get; set; }
        [Range(283210000, 283210010, ErrorMessage = "Select a Type.")]
        [Display(Name = "Type")]
        public TypeEnum Type { get; set; }
    }
}
