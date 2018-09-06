using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace DynamicsCustomWorkflows
{
    public class CreatePaymentRecords : CodeActivity
    {
        [RequiredArgument]
        [Input("Mortgage")]
        [ReferenceTarget("rev_mortgage")]
        public InArgument<EntityReference> Mortgage { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            EntityReference mortgageref = Mortgage.Get(executionContext);
            Entity mortgage = service.Retrieve(mortgageref.LogicalName, mortgageref.Id, new ColumnSet("modifiedon", "rev_mortgageterm", "rev_mortgagepayment"));

            RetrieveAttributeRequest retrieveAttrReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = "rev_mortgage",
                LogicalName = "rev_mortgageterm",
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttrRes = (RetrieveAttributeResponse)service.Execute(retrieveAttrReq);
            PicklistAttributeMetadata metadata = (PicklistAttributeMetadata)retrieveAttrRes.AttributeMetadata;
            OptionMetadata[] optionList = metadata.OptionSet.Options.ToArray();
            string selectedOptionLabel = string.Empty;
            foreach (OptionMetadata oMD in optionList)
            {
                if (oMD.Value == int.Parse(((OptionSetValue)mortgage.Attributes["rev_mortgageterm"]).Value.ToString()))
                    selectedOptionLabel = oMD.Label.UserLocalizedLabel.Label;
            }

            ExecuteMultipleRequest execMultReq = new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            DateTime dueDate = ((DateTime)mortgage.Attributes["modifiedon"]).AddMonths(1);

            for (int i = 1; i <= int.Parse(selectedOptionLabel) * 12; ++i)
            {
                Entity mortgagepaymentrecord = new Entity("rev_mortgagepaymentrecord");
                mortgagepaymentrecord.Attributes.Add("rev_name", "Payment Period " + i);
                mortgagepaymentrecord.Attributes.Add("rev_duedate", dueDate);
                mortgagepaymentrecord.Attributes.Add("rev_payment", (Money)mortgage.Attributes["rev_mortgagepayment"]);
                mortgagepaymentrecord.Attributes.Add("rev_mortgageid", mortgageref);
                CreateRequest createReq = new CreateRequest { Target = mortgagepaymentrecord };
                execMultReq.Requests.Add(createReq);
                dueDate = dueDate.AddMonths(1);
            }

            ExecuteMultipleResponse execMultRes = (ExecuteMultipleResponse)service.Execute(execMultReq);
        }
    }
}
