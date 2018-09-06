using System.Runtime.Serialization;

namespace DynamicsPlugins
{
    [DataContract]
    public class MortgageRiskScore
    {
        [DataMember]
        public int RiskScore { get; set; }
    }
}
