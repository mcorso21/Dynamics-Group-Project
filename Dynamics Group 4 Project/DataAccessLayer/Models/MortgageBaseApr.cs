using System;
using System.Runtime.Serialization;

namespace DataAccessLayer.Models
{
    [DataContract]
    public class MortgageBaseApr
    {
        public int? AccessKey;
        private DateTime LastModified;
        private double Rate;
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public double Apr { get; set; }

        public MortgageBaseApr()
        {
            AccessKey = 123456;
            LastModified = DateTime.Today;
            Rate = 4.298;
        }

        public DateTime GetLastModified()
        {
            return LastModified;
        }

        public double GetRate()
        {
            if (LastModified < DateTime.Today)
            {
                Random random = new Random();
                Rate = (random.NextDouble() * 2) + 3;           // Rate ranges from 3 to ~5
            }
            return Rate;
        }

        public void SetRate(int key, double rate)
        {
            if (key == AccessKey)
                Rate = rate;
        }
    }
}
