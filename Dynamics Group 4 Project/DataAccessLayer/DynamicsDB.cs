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
using System.Net;

/*
 * To-Do:
 *  - How to add documents to sharepoint from console?
 *             
 */
namespace DataAccessLayer
{
    public static class DynamicsDB
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();     //logger.Info(e.Message);

        private static CrmServiceClient client;
        private static IOrganizationService service;

        static DynamicsDB()
        {
            string URL = "https://revature4.crm.dynamics.com/";
            string User = "mike@revature4.onmicrosoft.com";
            string PW = "revatureGroup4!";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client = new CrmServiceClient($"Url={URL}; Username={User}; Password={PW}; authtype=Office365");
            service = (IOrganizationService)
                ((client.OrganizationWebProxyClient != null)
                ? (IOrganizationService)client.OrganizationWebProxyClient
                : (IOrganizationService)client.OrganizationServiceProxy);
        }

        public static void Test()
        {
            try
            {
                using (var context = new OrganizationServiceContext(service))
                {
                    var cases = (from i in context.CreateQuery("incident")
                                 select i).ToList();

                    var associates = (from u in context.CreateQuery("systemuser")
                                 where ((EntityReference)u["positionid"]).Id == new Guid("3AD119C4-4FAD-E811-A96B-000D3A1CA723")
                                 select new
                                 {
                                     UserId = u.Id
                                 }).ToList();

                    var casesByOwner = cases.GroupBy(u => u["ownerid"])
                                    .Select(grp => grp.ToList())
                                    .ToList().OrderBy(c => c.Count());

                    //var t1 = cases.GroupBy(u => u["ownerid"]);

                    //var t2 = t1.Select(grp => grp.ToList());

                    //var t3 = t2.ToList();
                    
                    //var t4 = t3.OrderBy(c => c.Count());


                    foreach (var c in casesByOwner)
                    {
                        if (associates.Any(a => a.UserId.Equals(((EntityReference)c[0]["ownerid"]).Id)))
                        {
                            Console.WriteLine($"Assigned to user {((EntityReference)c[0]["ownerid"]).Id} with {c.Count()} cases.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
        }

        public static Guid CreateContact(string firstname, string lastname, string ssn)
        {
            Guid contactGuid = Guid.Empty;
            try
            {
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
                contactGuid = (Guid)resp.Results["id"];
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($@"CreateContact failed: {ex.Message}\n{ex.StackTrace}");
                logger.Info(ex.Message);
            }
            return contactGuid;
        }

        public static void CreateMortgage(MortgageModel mortgageModel)
        {
            try
            {
                // Create new mortgage
                Entity newMortgage = new Entity("rev_mortgage");
                newMortgage.Attributes.Add("rev_customerid", new EntityReference("contact", mortgageModel.ContactId));
                // Get State
                Guid StateId = Guid.Empty;
                if (mortgageModel.State == null) mortgageModel.State = "Canada";
                using (var context = new OrganizationServiceContext(service))
                {
                    var ms = (from m in context.CreateQuery("rev_salestax")
                                where m["rev_name"].Equals(mortgageModel.State)
                                select m).FirstOrDefault();

                    if (ms.Id != null) newMortgage.Attributes.Add("rev_salestaxid", new EntityReference("rev_salestax", ms.Id));
                }
                newMortgage.Attributes.Add("rev_name", mortgageModel.Name);
                newMortgage.Attributes.Add("rev_region", new OptionSetValue((int)mortgageModel.Region));
                newMortgage.Attributes.Add("rev_approval", new OptionSetValue(283210000));      // Set to New
                newMortgage.Attributes.Add("rev_mortgageamount", new Money(mortgageModel.MortgageAmount));
                newMortgage.Attributes.Add("rev_mortgageterm", new OptionSetValue((int)mortgageModel.MortgageTermInYears));
                // State
                // Documents
                /* */


                // Query for State Name selected, get id and associate
                //newMortgage.Attributes.Add("rev_salestaxid", new EntityReference("rev_salestax", mortgageModel.ContactId));

                /* */
                // Create request for mortgage creation
                CreateRequest request = new CreateRequest();
                request.Target = newMortgage;
                // Execute request
                CreateResponse resp = (CreateResponse)service.Execute(request);
                //Guid mortgageGuid = (Guid)resp.Results["id"];
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
                // Create new case
                Entity newCase = new Entity("incident");
                newCase.Attributes.Add("customerid", new EntityReference("contact", mortgageCaseModel.ContactId));
                newCase.Attributes.Add("rev_mortgagecaseid", new EntityReference("rev_mortgage", mortgageCaseModel.MortgageId));
                newCase.Attributes.Add("title", mortgageCaseModel.Title);
                newCase.Attributes.Add("description", mortgageCaseModel.Description);
                newCase.Attributes.Add("prioritycode", new OptionSetValue((int)mortgageCaseModel.Priority));
                newCase.Attributes.Add("rev_highpriorityreason", mortgageCaseModel.HighPriorityReason);
                newCase.Attributes.Add("rev_type", new OptionSetValue((int)mortgageCaseModel.Type));
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

        public static List<MortgageModel> GetMortgages(Guid ContactId)
        {
            List<MortgageModel> mortgages = new List<MortgageModel>();
            try
            {
                using (var context = new OrganizationServiceContext(service))
                {
                    var ms = from m in context.CreateQuery("rev_mortgage")
                             where m["rev_customerid"].Equals(ContactId)
                             select m;

                    foreach (var item in ms)
                    {
                        mortgages.Add(new MortgageModel()
                        {
                            MortgageId = item.Id,
                            Name = (item.Contains("rev_name")) ? item["rev_name"].ToString() : "N/A",
                            Region = (item.Contains("rev_region"))
                                ? (RegionEnum)Enum.Parse(typeof(RegionEnum), ((OptionSetValue)item["rev_region"]).Value.ToString())
                                : RegionEnum.US,
                            State = (item.Contains("rev_salestaxid")) ? ((EntityReference)item["rev_salestaxid"]).Name : "N/A",
                            Approval = (item.Contains("rev_approval"))
                                ? (ApprovalEnum)Enum.Parse(typeof(ApprovalEnum), ((OptionSetValue)item["rev_approval"]).Value.ToString())
                                : ApprovalEnum.Review,
                            MortgageAmount = (item.Contains("rev_mortgageamount"))
                                ? ((Money)item["rev_mortgageamount"]).Value : 0,
                            MortgageTermInYears = (item.Contains("rev_mortgageterm"))
                                ? (TermEnum)Enum.Parse(typeof(TermEnum), ((OptionSetValue)item["rev_mortgageterm"]).Value.ToString())
                                : TermEnum.Thirty,
                            MortgageNumber = (item.Contains("rev_mortgagenumber")) ? item["rev_mortgagenumber"].ToString() : "N/A",
                            Apr = (item.Contains("rev_finalapr")) ? (decimal)item["rev_finalapr"] : 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
            return mortgages;
        }

        public static List<MortgageCaseModel> GetCases(Guid ContactId)
        {
            List<MortgageCaseModel> cases = new List<MortgageCaseModel>();
            try
            {
                using (var context = new OrganizationServiceContext(service))
                {
                    var incidents = from incident in context.CreateQuery("incident")
                                    where incident["customerid"].Equals(ContactId)
                                    select incident;

                    foreach (var item in incidents)
                    {
                        if ((item.Contains("rev_mortgagecaseid")))
                        {
                            var mortgage = (EntityReference)item["rev_mortgagecaseid"];
                            var keys = ((EntityReference)item["rev_mortgagecaseid"]).KeyAttributes;

                            foreach (var v in ((EntityReference)item["rev_mortgagecaseid"]).KeyAttributes)
                            {
                                System.Diagnostics.Debug.WriteLine($"key={v.Key}");
                            }
                        }
                        cases.Add(new MortgageCaseModel()
                        {
                            // Would have to perform a separate query to get the MortgageNumber
                            //MortgageNumber = (item.Contains("rev_mortgagecaseid"))
                            //    ? (((EntityReference)item["rev_mortgagecaseid"]).KeyAttributes.Contains("rev_mortgagenumber")
                            //        ? "Has rev_mortgagecaseid AND rev_mortgagenumber"
                            //        : "Has rev_mortgagecaseid; Does NOT have rev_mortgagenumber") 
                            //    : "Does NOT have rev_mortgagecaseid",
                            MortgageId = (item.Contains("rev_mortgagecaseid")) 
                                ? ((EntityReference)item["rev_mortgagecaseid"]).Id : Guid.Empty,
                            MortgageName = (item.Contains("rev_mortgagecaseid"))
                                ? ((EntityReference)item["rev_mortgagecaseid"]).Name : "N/A",
                            Title = (item.Contains("title")) ? item["title"].ToString() : "N/A",
                            Description = (item.Contains("description")) ? item["description"].ToString() : "N/A",
                            Priority = (item.Contains("prioritycode")) 
                                ? (PriorityEnum)Enum.Parse(typeof(PriorityEnum), ((OptionSetValue)item["prioritycode"]).Value.ToString()) 
                                : PriorityEnum.Normal,
                            HighPriorityReason = (item.Contains("rev_highpriorityreason")) 
                                ? item["rev_highpriorityreason"].ToString() : "N/A",
                            Type = (item.Contains("rev_type"))
                                ? (TypeEnum)Enum.Parse(typeof(TypeEnum), ((OptionSetValue)item["rev_type"]).Value.ToString())
                                : TypeEnum.Other
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

        public static List<MortgagePaymentRecordModel> GetPayments(List<MortgageModel> mortgages)
        {
            List< MortgagePaymentRecordModel> paymentRecords = new List<MortgagePaymentRecordModel>();
            try
            {
                using (var context = new OrganizationServiceContext(service))
                {
                    //////////////////////

                    //var payments = (from payment in context.CreateQuery("rev_mortgagepaymentrecord")
                    //                where mortgages.Any(m => m.MortgageId.Equals(payment["rev_mortgageid"]))
                    //                    && ((OptionSetValue)payment["rev_status"]).Value == 283210000
                    //                orderby payment["rev_duedate"]
                    //                select payment).ToList();

                    //////////////////////

                    foreach (MortgageModel mortgage in mortgages)
                    {
                        var payments = (from payment in context.CreateQuery("rev_mortgagepaymentrecord")
                                        where payment["rev_mortgageid"].Equals(mortgage.MortgageId)
                                        && ((OptionSetValue)payment["rev_status"]).Value == 283210000
                                        orderby payment["rev_duedate"]
                                        select payment).ToList();

                        if (payments.Count > 0)
                        {
                            var p = payments[0];
                            paymentRecords.Add(new MortgagePaymentRecordModel()
                            {
                                Id = p.Id,
                                MortgageId = mortgage.MortgageId,
                                MortgageNumber = mortgage.MortgageNumber,
                                Name = (p.Contains("rev_name")) ? p["rev_name"].ToString() : "N/A",
                                PaymentStatus = PaymentStatusEnum.Pending,
                                Amount = (p.Contains("rev_payment")) ? ((Money)p["rev_payment"]).Value : 0,
                                DueDate = (p.Contains("rev_duedate")) ? ((DateTime)p["rev_duedate"]) : DateTime.Now
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Info(ex.StackTrace);
            }
            return paymentRecords;
        }

        public static void MakePayment(Guid paymentRecordId)
        {
            try
            {
                //paymentRecord.Attributes.Add("rev_status", 283210001);
                //Entity paymentRecord = service.Retrieve("rev_mortgagepaymentrecord",
                //                                paymentRecordId,
                //                                new ColumnSet(new String[] { "rev_status" }));

                Entity paymentRecord = new Entity("rev_mortgagepaymentrecord");
                paymentRecord.Id = paymentRecordId;
                paymentRecord["rev_status"] = new OptionSetValue(283210001);
                service.Update(paymentRecord);
            }
            catch(Exception ex)
            {
                logger.Info(ex.Message);
                logger.Info(ex.StackTrace);
            }
        }
    }
}
