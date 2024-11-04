using Elsa;
using Elsa.Services.Models;
using System.Threading.Tasks;
using Elsa.Services;
using Elsa.ActivityResults;
using Elsa.Workflows.Memory;

namespace ElsaServer.Activities
{
    //public class ApproveDocumentActivity : Activity
    //{
    //    // Change Input<bool> to Variable<bool>
    //    public Variable<bool> ApprovalStatus { get; set; }

    //    public override async ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
    //    {
    //        // Simulate admin approval process (you can replace this with actual logic)
    //        var isApproved = await SimulateApprovalAsync();

    //        // Set approval status in the workflow context using context.SetVariable
    //        context.SetVariable(ApprovalStatus, isApproved);

    //        return Done();
    //    }

    //    private Task<bool> SimulateApprovalAsync()
    //    {
    //        // Simulate some delay and return a random approval status
    //        return Task.FromResult(new Random().Next(0, 2) == 0); // Randomly approve or deny
    //    }
    //}
}
