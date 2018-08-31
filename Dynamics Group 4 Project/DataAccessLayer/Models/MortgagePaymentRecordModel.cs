﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public enum PaymentStatusEnum { Pending = 283210000, Completed = 283210001 };

    public class MortgagePaymentRecordModel
    {
        public Guid MortgageId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
    }
}
