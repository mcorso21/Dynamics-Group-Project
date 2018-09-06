using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;

namespace DailyUpdateConsoleWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // variables
            WebRequest request;
            WebResponse response = null;
            StreamReader reader = null;
            string responseString = String.Empty;
            JObject jsonResponse;
            Guid CADGuid = new Guid("A52BFC0C-B1AC-E811-A96C-000D3A1CA612");
            Guid BaseAprGuid = new Guid("4C767953-36AD-E811-A968-000D3A1CA7D6");

            try
            {
                CrmServiceClient client = new CrmServiceClient("Url=https://revature4.crm.dynamics.com; UserName=yuyuan@revature4.onmicrosoft.com; Password=Ilikecurry123; AuthType=Office365");
                IOrganizationService service = (IOrganizationService)client.OrganizationWebProxyClient ?? client.OrganizationServiceProxy;

                #region creating auto-number for mortgage entity
                //// auto-numbering for mortgage number
                //// using advance auto-numbering, this template is for creating custom field using OrganizationRequest
                //CreateAttributeRequest widgetSerialNumberAttributeRequest = new CreateAttributeRequest
                //{
                //    EntityName = "rev_mortgage",
                //    Attribute = new StringAttributeMetadata
                //    {
                //        //Define the format of the attribute
                //        AutoNumberFormat = "{DATETIMEUTC:yyyyMM}{SEQNUM:6}",
                //        LogicalName = "rev_mortgagenumber",
                //        SchemaName = "rev_mortgagenumber",
                //        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                //        MaxLength = 100, // The MaxLength defined for the string attribute must be greater than the length of the AutoNumberFormat value, that is, it should be able to fit in the generated value.
                //        DisplayName = new Label("Mortgage Number", 1033),
                //        Description = new Label("Mortgage Number of the widget.", 1033)
                //    }
                //};
                //service.Execute(widgetSerialNumberAttributeRequest);
                //Console.WriteLine("successfully created auto number field for mortgage");
                #endregion

                #region updating base APR
                // updating base APR record stored in configurations
                request = WebRequest.Create("http://revgroup4app.azurewebsites.net/api/mortgageapr");
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                jsonResponse = JObject.Parse(responseString);
                // example response: {"Date":"2018-08-31T00:00:00+00:00","Apr":4.298}
                decimal apr = (decimal)jsonResponse["Apr"];

                Entity config = service.Retrieve("rev_configuration", BaseAprGuid,
                    new ColumnSet(new string[] { "rev_value" })); // getting the record for base APR
                config.Attributes["rev_value"] = (apr / 100).ToString(); // rev_value is of type text in dynamics
                service.Update(config);
                Console.WriteLine("Updated base APR");
                #endregion

                #region updating USD to CAD exchange rate
                // updating exchange rate for Canadian dollar
                // reference for currency entity: https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/entities/transactioncurrency
                request = WebRequest.Create("https://free.currencyconverterapi.com/api/v6/convert?q=USD_CAD&compact=ultra");
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                jsonResponse = JObject.Parse(responseString);
                // example response: {"USD_CAD":1.31765}
                decimal rate = (decimal)jsonResponse["USD_CAD"];

                Entity currency = service.Retrieve("transactioncurrency", CADGuid,
                    new ColumnSet(new string[] { "exchangerate" })); // getting the record for Base APR
                currency.Attributes["exchangerate"] = rate; // exchangerate is of type decimal in dynamics
                service.Update(currency);
                Console.WriteLine("Updated exchange rate");
                #endregion

                #region creating dummy config for webjobs automation
                Entity dummyconfig = new Entity("rev_configuration");
                dummyconfig.Attributes.Add("rev_key", "dummy key");
                dummyconfig.Attributes.Add("rev_value", "dummy value");
                service.Create(dummyconfig);
                Console.WriteLine("Created dummy variable");
                #endregion
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                throw;
            }
            finally
            {
                reader.Close();
                response.Close();
            }
        }
    }
}
