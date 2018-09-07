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

            //// Create Mortgage
            //MortgageModel mortgageModel = new MortgageModel()
            //{
            //    ContactId = new Guid("05dab347-58b0-e811-a96d-000d3a1ca7d6"),
            //    Name = "Mike Test 3",
            //    Region = RegionEnum.US,
            //    State = "Alaska",
            //    MortgageAmount = 1000000,
            //    MortgageTermInYears = TermEnum.Fifteen
            //};

            //DynamicsDB.CreateMortgage(mortgageModel);

            // Create Case
            //MortgageCaseModel caseModel = new MortgageCaseModel()
            //{
            //    ContactId = new Guid("05dab347-58b0-e811-a96d-000d3a1ca7d6"),
            //    Title = "Web App Test Title #2",
            //    Description = "Test Description",
            //    Type = TypeEnum.Mortgage,
            //    Priority = PriorityEnum.High,
            //    HighPriorityReason = "High priorirty reaso nand stuff "
            //};

            //DynamicsDB.CreateCase(caseModel);

            // Get Cases
            //var cases = DynamicsDB.GetCases(new Guid("05dab347-58b0-e811-a96d-000d3a1ca7d6"));
            //foreach (MortgageCaseModel c in cases)
            //{
            //    Console.WriteLine($"title={c.Title} Desc={c.Description} Prio={c.Priority} mortgageId={c.MortgageId} mortgageName={c.MortgageName}");
            //}


            // Get Mortgage Payments
            //Guid clientId = new Guid("63A0E5B9-88DF-E311-B8E5-6C3BE5A8B200");
            //List<MortgageModel> mortgages = new List<MortgageModel>
            //{
            //    new MortgageModel
            //    {
            //        MortgageId = new Guid("DE8DFE95-45B1-E811-A96C-000D3A1CAE35")
            //    }
            //};
            //List<MortgagePaymentRecordModel> payments = DynamicsDB.GetPayments(mortgages);

            //foreach (MortgagePaymentRecordModel p in payments)
            //{
            //    Console.WriteLine(p.Name + " " + p.Amount);
            //}


            DynamicsDB.Test();

            Console.ReadLine();
        }
    }
}
