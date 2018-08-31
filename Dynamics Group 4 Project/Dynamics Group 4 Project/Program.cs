using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Dynamics_Group_4_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Contact
            DynamicsDB.CreateContact("Test Contact", "1", "123-45-6789");


        }
    }
}
