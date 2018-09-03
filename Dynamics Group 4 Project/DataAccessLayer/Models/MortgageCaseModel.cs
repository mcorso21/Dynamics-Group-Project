﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public enum PriorityEnum { High = 1, Normal = 2, Low = 3 };

    public class MortgageCaseModel
    {
        public string Title { get; set; }
        public Guid ContactId { get; set; }
        public string Description { get; set; }
        public PriorityEnum Priority { get; set; }
        public string HighPriorityReason { get; set; }
        public string Type { get; set; }
    }
}