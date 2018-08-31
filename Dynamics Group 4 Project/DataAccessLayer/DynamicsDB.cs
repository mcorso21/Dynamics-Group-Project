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
using System.Collections.Generic;

/*
 * To-Do:
 *  - How to add documents to sharepoint from console?
 *  - Make Payments (Update Mortgage Payment Record)            -- Done, but not tested
 *  
 *  Done:
 *  - Apply for Mortgage (Create Mortgage)
 *  - Create Client
 *  - View Cases
 *  - Create Case
 *      - Need to finish attributes
 *             
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

        public static void CreateMortgage(MortgageModel mortgageModel)
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
                newMortgage.Attributes.Add("rev_customerid", new EntityReference("contact", mortgageModel.ContactId));
                //newMortgage.Attributes.Add("rev_customerid", mortgageModel.ContactId);
                newMortgage.Attributes.Add("rev_name", mortgageModel.Name);
                newMortgage.Attributes.Add("rev_region", new OptionSetValue((int)mortgageModel.Region));
                newMortgage.Attributes.Add("rev_approval", new OptionSetValue((int)mortgageModel.Approval));
                newMortgage.Attributes.Add("rev_mortgageamount", new Money(mortgageModel.MortgageAmount));
                newMortgage.Attributes.Add("rev_mortgageterm", mortgageModel.MortgageTermInMonths);
                // Create request for mortgage creation
                CreateRequest request = new CreateRequest();
                request.Target = newMortgage;
                // Execute request
                CreateResponse resp = (CreateResponse)service.Execute(request);
                Guid mortgageGuid = (Guid)resp.Results["id"];
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Info(ex.StackTrace);
            }
        }

        public static void CreateCase(MortgageCaseModel mortgageCaseModel)
        {
            try
            {
                CrmServiceClient client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
                IOrganizationService service = (IOrganizationService)
                    ((client.OrganizationWebProxyClient != null)
                    ? (IOrganizationService)client.OrganizationWebProxyClient
                    : (IOrganizationService)client.OrganizationServiceProxy);

                // Create new case
                Entity newCase = new Entity("incident");
                newCase.Attributes.Add("title", mortgageCaseModel.Title);
                newCase.Attributes.Add("customerid", new EntityReference("contact", mortgageCaseModel.ContactId));
                // ... Need to finish the attribute ...
                // Create request for case creation
                CreateRequest request = new CreateRequest();
                request.Target = newCase;
                // Execute request
                CreateResponse resp = (CreateResponse)service.Execute(request);
                Guid incidentGuid = (Guid)resp.Results["id"];
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
        }

        public static List<MortgageCaseModel> GetCases(Guid ContactId)
        {
            List<MortgageCaseModel> cases = new List<MortgageCaseModel>();
            try
            {
                CrmServiceClient client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
                IOrganizationService service = (IOrganizationService)
                    ((client.OrganizationWebProxyClient != null)
                    ? (IOrganizationService)client.OrganizationWebProxyClient
                    : (IOrganizationService)client.OrganizationServiceProxy);

                using (var context = new OrganizationServiceContext(service))
                {
                    var incidents = from incident in context.CreateQuery("incident")
                                where incident["customerid"].Equals(ContactId)
                                select new
                                {
                                    Title = incident["title"],
                                    Description = incident["description"],
                                    Priority = incident["prioritycode"]
                                    // HighPriorityReason is erroring, likely because the column doesn't exist/is not returned
                                    // Will ask Satish if there is a solution
                                    //HighPriorityReason = incident["rev_highpriorityreason"] ?? ""
                                    // Type?
                                };

                    foreach (var item in incidents)
                    {
                        cases.Add(new MortgageCaseModel()
                        {
                            ContactId = ContactId,
                            Title = item.Title.ToString(),
                            Description = item.Description.ToString(),
                            Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum), ((OptionSetValue)item.Priority).Value.ToString())
                            //Type = item.Type.ToString(),
                            //HighPriorityReason = item.HighPriorityReason.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
            return cases;
        }

        public static MortgagePaymentRecordModel GetNextPayment(Guid MortgageId)
        {
            MortgagePaymentRecordModel paymentRecord = null;
            try
            {
                CrmServiceClient client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
                IOrganizationService service = (IOrganizationService)
                    ((client.OrganizationWebProxyClient != null)
                    ? (IOrganizationService)client.OrganizationWebProxyClient
                    : (IOrganizationService)client.OrganizationServiceProxy);

                using (var context = new OrganizationServiceContext(service))
                {
                    var payments = from payment in context.CreateQuery("rev_mortgagepaymentrecord")
                                where payment["rev_mortgageid"].Equals(MortgageId)
                                orderby payment["rev_duedate"] 
                                select new
                                {
                                    Mortgage = payment["rev_mortgageid"],
                                    DueDate = payment["rev_duedate"],
                                    Amount = payment["rev_payment"],
                                    Status = payment["rev_status"]
                                };

                    foreach (var item in payments)
                    {
                        paymentRecord = new MortgagePaymentRecordModel()
                        {
                            MortgageId = (Guid)item.Mortgage,
                            DueDate = (DateTime)item.DueDate,
                            Amount = ((Money)item.Amount).Value,
                            PaymentStatus = (PaymentStatusEnum)Enum.Parse(typeof(PaymentStatusEnum), item.Status.ToString())
                        };
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
            return paymentRecord;
        }
    }
}
