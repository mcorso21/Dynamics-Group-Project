using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class UserMapModel
    {
        public Guid Id { get; set; }
        public Guid UserWebAppId { get; set; }
        public Guid ClientDynamicsId { get; set; }
        public Guid UserDynamicsId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
    }
}
