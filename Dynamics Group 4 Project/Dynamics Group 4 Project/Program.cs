using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace Dynamics_Group_4_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Contact
            //DataAccessLayer.DynamicsDB.CreateContact("WebApp", "Contact2", "123-45-6789");

            // Create Mortgage
            //MortgageModel mortgageModel = new MortgageModel()
            //{
            //    ContactId = new Guid("97D8FA4E-5DAD-E811-A96A-000D3A1CA939"),
            //    Name = "WebAppTest",
            //    Region = RegionEnum.US,
            //    MortgageAmount = 1000000,
            //    MortgageTermInMonths = 360,
            //    Approval = ApprovalEnum.New
            //};

            //DynamicsDB.CreateMortgage(mortgageModel);

            // Create Case
            MortgageCaseModel caseModel = new MortgageCaseModel()
            {
                ContactId = new Guid("05dab347-58b0-e811-a96d-000d3a1ca7d6"),
                Title = "Web App Test Title #2",
                Description = "Test Description",
                Type = TypeEnum.Mortgage,
                Priority = PriorityEnum.High,
                HighPriorityReason = "High priorirty reaso nand stuff "
            };

            DynamicsDB.CreateCase(caseModel);

            // Get Cases
            //var cases = DynamicsDB.GetCases(new Guid("05dab347-58b0-e811-a96d-000d3a1ca7d6"));
            //foreach (MortgageCaseModel c in cases)
            //{
            //    Console.WriteLine($@"title={c.Title} Desc={c.Description} Prio={c.Priority}");
            //}
            //Console.ReadLine();
        }
    }
}
