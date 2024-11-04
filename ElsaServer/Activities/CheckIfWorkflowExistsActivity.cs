using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications;
using System.Threading.Tasks;
using ElsaServer.Specifications;

namespace ElsaServer.Activities
{
    [Activity(Category = "Custom", Description = "Checks if a workflow with a specific RequestId exists.")]
    public class CheckIfWorkflowExistsActivity : Activity
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public CheckIfWorkflowExistsActivity(IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowInstanceStore = workflowInstanceStore;
        }

        [ActivityInput(Hint = "The RequestId to check.")]
        public string RequestId { get; set; }

        [ActivityOutput(Hint = "Returns true if a workflow instance with the same RequestId exists, otherwise false.")]
        public bool WorkflowExists { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            var specification = new WorkflowInstanceSpecification(requestId: RequestId);
            var existingWorkflowInstance = await _workflowInstanceStore.FindAsync(specification, context.CancellationToken);

            WorkflowExists = existingWorkflowInstance != null;

            return Done();
        }
    }
}
