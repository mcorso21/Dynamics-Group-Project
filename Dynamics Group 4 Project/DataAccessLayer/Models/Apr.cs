using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Models
{
    public class Apr
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Key { get; set; }
        public DateTime LastModified { get; set; }
        public double Rate { get; set; }
    }
}
