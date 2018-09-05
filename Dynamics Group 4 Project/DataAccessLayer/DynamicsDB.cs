﻿using System;
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
 *  - Create Mortgage
 *  - Make Payment
 *  
 *  - Get Payments
 *  
 *  Done:
 *  - Create Contact
 *  - Create Case
 *  
 *  - Get Cases
 *  - Get Mortgages
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
                if (mortgageModel.State != "")
                {
                    try
                    {
                        using (var context = new OrganizationServiceContext(service))
                        {
                            var ms = (from m in context.CreateQuery("rev_salestax")
                                      where m["rev_name"].Equals(mortgageModel.State)
                                      select m).First();

                            if (ms.Id != null) StateId = ms.Id;
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Info(ex.Message);
                        logger.Info(ex.StackTrace);
                    }
                }
                if (StateId != Guid.Empty) newMortgage.Attributes.Add("rev_salestaxid", new EntityReference("rev_salestax", StateId));
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
                // Create new case
                Entity newCase = new Entity("incident");
                newCase.Attributes.Add("customerid", new EntityReference("contact", mortgageCaseModel.ContactId));
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
                        cases.Add(new MortgageCaseModel()
                        {
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
            List< MortgagePaymentRecordModel> paymentRecords = null;
            try
            {
                using (var context = new OrganizationServiceContext(service))
                {
                    foreach (MortgageModel mortgage in mortgages)
                    {
                        var payments = (from payment in context.CreateQuery("rev_mortgagepaymentrecord")
                                        where payment["rev_mortgageid"].Equals(mortgage.MortgageId)
                                        && ((OptionSetValue)payment["rev_status"]).Value == 283210000
                                        orderby payment["rev_duedate"]
                                        select payment).ToList();

                        foreach (var p in payments)
                        {
                            Console.WriteLine($@"Name={p["rev_name"]},Amount={((Money)p["rev_payment"]).Value},Due={p["rev_duedate"]},Status={p["rev_status"]}");
                        }
                    }

                    // rev_name
                    // rev_status
                    // rev_payment
                    // rev_mortgageid
                    // rev_duedate

                    //foreach (var item in payments)
                    //{
                    //    paymentRecord = new MortgagePaymentRecordModel()
                    //    {
                    //        MortgageId = (Guid)item.Mortgage,
                    //        DueDate = (DateTime)item.DueDate,
                    //        Amount = ((Money)item.Amount).Value,
                    //        PaymentStatus = (PaymentStatusEnum)Enum.Parse(typeof(PaymentStatusEnum), item.Status.ToString())
                    //    };
                    //    break;
                    //}
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
            }
            return paymentRecords;
        }
    }
}
