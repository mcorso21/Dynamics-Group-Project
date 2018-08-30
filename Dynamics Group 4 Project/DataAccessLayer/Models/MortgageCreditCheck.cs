using System;
using System.Runtime.Serialization;

namespace DataAccessLayer.Models
{
    [DataContract]
    public class MortgageCreditCheck
    {
        private Random random;
        [DataMember]
        public int RiskScore { get; set; }

        public MortgageCreditCheck()
        {
            random = new Random();
        }

        public int GetRiskScore()
        {
            // Risk score ranges from 1-100, 100 is worst score 
            return (int)((random.NextDouble() * 100) + 1);           
        }
    }
}
