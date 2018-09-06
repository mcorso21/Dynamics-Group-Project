using System.Runtime.Serialization;

namespace DynamicsPlugins
{
    [DataContract]
    class MortgageRiskScore
    {
        [DataMember]
        public int RiskScore { get; set; }
    }
}
