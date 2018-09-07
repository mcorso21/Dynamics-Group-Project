using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;

namespace DynamicsPlugins
{
    public class MortgageUpdate : IPlugin // preoperation plugin to be triggered on mortgage update
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity mortgage = (Entity)context.InputParameters["Target"];

                try
                {
                    WebRequest request = WebRequest.Create("http://revgroup4app.azurewebsites.net/api/mortgageriskscore");
                    WebResponse response = request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseString = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                    MortgageRiskScore mrs = Helper.JsonDeserialize<MortgageRiskScore>(responseString);
                    // example response: {"RiskScore":30}
                    decimal RiskScore = mrs.RiskScore;

                    Guid BaseAprGuid = new Guid("4C767953-36AD-E811-A968-000D3A1CA7D6");
                    Guid MarginGuid = new Guid("D6EF9022-7EAF-E811-A968-000D3A1CABCE");
                    Entity config = service.Retrieve("rev_configuration", BaseAprGuid,
                        new ColumnSet("rev_value")); // getting the record for Base APR
                    decimal BaseApr = decimal.Parse(config.Attributes["rev_value"].ToString());
                    config = service.Retrieve("rev_configuration", MarginGuid,
                        new ColumnSet("rev_value")); // getting the record for Margin
                    decimal Margin = decimal.Parse(config.Attributes["rev_value"].ToString());

                    Entity oldmortgage = context.PreEntityImages["PreImage"]; // need to retrieve salestax, mortgageterm, mortgageamount

                    Entity SalesTax = mortgage.Attributes.Contains("rev_salestaxid") ?
                        service.Retrieve("rev_salestax", ((EntityReference)mortgage.Attributes["rev_salestaxid"]).Id, new ColumnSet("rev_rate")) :
                        service.Retrieve("rev_salestax", ((EntityReference)oldmortgage.Attributes["rev_salestaxid"]).Id, new ColumnSet("rev_rate"));
                    decimal Tax = decimal.Parse(SalesTax.Attributes["rev_rate"].ToString());

                    RetrieveAttributeRequest req = new RetrieveAttributeRequest
                    {
                        EntityLogicalName = "rev_mortgage",
                        LogicalName = "rev_mortgageterm",
                        RetrieveAsIfPublished = true
                    };
                    RetrieveAttributeResponse res = (RetrieveAttributeResponse)service.Execute(req);
                    PicklistAttributeMetadata metadata = (PicklistAttributeMetadata)res.AttributeMetadata;
                    OptionMetadata[] optionList = metadata.OptionSet.Options.ToArray();
                    string selectedOptionLabel = string.Empty;
                    int selectedValue = mortgage.Attributes.Contains("rev_mortgageterm") ?
                        int.Parse(((OptionSetValue)mortgage.Attributes["rev_mortgageterm"]).Value.ToString()) :
                        int.Parse(((OptionSetValue)oldmortgage.Attributes["rev_mortgageterm"]).Value.ToString());
                    foreach (OptionMetadata oMD in optionList)
                    {
                        if (oMD.Value == selectedValue)
                            selectedOptionLabel = oMD.Label.UserLocalizedLabel.Label;
                    }

                    decimal IP = 12; // interest period per year, should be 12 since we are calculating monthly payment
                    decimal APR = BaseApr + Margin + (decimal)(Math.Log10((double)RiskScore) / 100) + Tax;
                    decimal PV = mortgage.Attributes.Contains("rev_value") ?
                        ((Money)mortgage.Attributes["rev_mortgageamount"]).Value :
                        ((Money)oldmortgage.Attributes["rev_mortgageamount"]).Value; // present value
                    decimal R = APR / IP; // periodic interest rate
                    decimal N = IP * decimal.Parse(selectedOptionLabel); // interest periods for overall time period
                    decimal P = PV * R / (1 - (decimal)Math.Pow((double)(1 + R), (double)-N)); // monthly payment
                    Money MonthlyPayment = new Money(P);

                    mortgage.Attributes.Add("rev_finalapr", APR);
                    mortgage.Attributes.Add("rev_mortgagepayment", MonthlyPayment);
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    //throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                    throw new InvalidPluginExecutionException(ex.Message + " " + ex.StackTrace);
                }

                catch (Exception ex)
                {
                    //tracingService.Trace("MyPlugin: {0}", ex.Message + " " + ex.StackTrace);
                    throw new InvalidPluginExecutionException(ex.Message + " " + ex.StackTrace);
                    //throw;
                }
            }
        }
    }
}
