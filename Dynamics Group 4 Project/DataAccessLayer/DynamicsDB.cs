using System;
using System.Configuration;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Threading;
using DataAccessLayer.Models;

/*
 * Requirements:
 *  1. Apply for Mortgage (Create Mortgage)
 *  2. Make Payments (Update Mortgage Payment Record)
 *  3. Create Case
 *  4. View Cases
 *  5. Create Client
 *  
 *  revgroup4
 *  revatureGroup4!
 */
namespace DataAccessLayer
{
    public static class DynamicsDB
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);

        private static string URL, User, PW;

        static DynamicsDB()
        {
            URL = "https://revature4.crm.dynamics.com/";
            User = "mike@revature4.onmicrosoft.com";
            PW = "revatureGroup4!";
        }

        public static void CreateContact(string firstname, string lastname, string ssn)
        {
            try
            {
                CrmServiceClient client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
                IOrganizationService service = (IOrganizationService)
                    ((client.OrganizationWebProxyClient != null)
                    ? (IOrganizationService)client.OrganizationWebProxyClient
                    : (IOrganizationService)client.OrganizationServiceProxy);

                // Create new contact
                Entity newContact = new Entity("contact");
                newContact.Attributes.Add("firstname", $"{firstname}");
                newContact.Attributes.Add("lastname", $"{lastname}");
                newContact.Attributes.Add("rev_ssn", $"{ssn}");
                // Create request for contact creation
                CreateRequest request = new CreateRequest();
                request.Target = newContact;
                // Execute request
                CreateResponse resp = (CreateResponse)service.Execute(request);
                Guid contactGuid = (Guid)resp.Results["id"];
            }
            catch(Exception ex)
            {
                logger.Info(ex.Message);
            }
        }

        public static void CreateMortgage(Guid contactId, MortgageModel mortgageModel)
        {
            try
            {
                CrmServiceClient client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
                IOrganizationService service = (IOrganizationService)
                    ((client.OrganizationWebProxyClient != null)
                    ? (IOrganizationService)client.OrganizationWebProxyClient
                    : (IOrganizationService)client.OrganizationServiceProxy);

                // Create new mortgage
                Entity newMortgage = new Entity("rev_mortgage");
                newMortgage.Attributes.Add("rev_approval", $"{mortgageModel.Approval}");
                newMortgage.Attributes.Add("rev_customerid", $"{contactId}");
                newMortgage.Attributes.Add("rev_monthlypayment", $"{mortgageModel.MortgagePayment}");
                newMortgage.Attributes.Add("rev_mortgageamount", $"{mortgageModel.MortgageAmount}");
                newMortgage.Attributes.Add("rev_mortgagenumber", $"{mortgageModel.MortgageNumber}");
                newMortgage.Attributes.Add("rev_mortgageterm", $"{mortgageModel.MortgageTermInMonths}");
                newMortgage.Attributes.Add("rev_region", $"{mortgageModel.Region}");
                // Create request for contact creation
                CreateRequest request = new CreateRequest();
                request.Target = newMortgage;
                // Execute request
                CreateResponse resp = (CreateResponse)service.Execute(request);
                Guid contactGuid = (Guid)resp.Results["id"];
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
        }
    }
}
