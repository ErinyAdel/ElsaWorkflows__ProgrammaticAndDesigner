using Elsa.ActivityResults;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Workflows;
using Elsa.Workflows.Activities;

namespace ElsaServer.Activities
{
    public class CustomActivity : CodeActivity
    {
        protected override void Execute(Elsa.Workflows.ActivityExecutionContext context)
        {
            //var workflowInstanceId = context.WorkflowInstance.Id;

        }
    }
}
