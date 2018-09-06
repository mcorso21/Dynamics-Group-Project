using System;
using System.Runtime.Serialization;

namespace DataAccessLayer.Models
{
    [DataContract]
    public class MortgageApr
    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public double Apr { get; set; }
    }
    public static class MortgageBaseApr
    {
        public static int? AccessKey;
        private static DateTime LastModified;
        private static double Rate;

        static MortgageBaseApr()
        {
            AccessKey = 123456;
            LastModified = DateTime.Today;
            Rate = 4.298;
        }

        public static DateTime GetLastModified()
        {
            return LastModified;
        }

        public static double GetRate()
        {
            if (LastModified < DateTime.Today)
            {
                Random random = new Random();
                Rate = (random.NextDouble() * 2) + 3;           // Rate ranges from 3 to ~5
            }
            return Rate;
        }

        public static void SetRate(int key, double rate)
        {
            if (key == AccessKey)
                Rate = rate;
        }
    }
}
