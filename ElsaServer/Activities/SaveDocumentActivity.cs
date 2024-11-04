using Elsa;
using Elsa.Services.Models;
using System.Threading.Tasks;
using Elsa.Services;
using Elsa.ActivityResults;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using ElsaServer.Models;
using Elsa.Extensions;

namespace ElsaServer.Activities
{
    //public class SaveDocumentActivity : Activity
    //{
    //    public Input<DocumentModel> Document { get; set; }
    //    public Input<bool> ApprovalStatus { get; set; }

    //    public override async ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
    //    {
    //        var document = Document.Get(context);
    //        var isApproved = ApprovalStatus.Get(context);

    //        // Implement your database saving logic here
    //        // Example:
    //        await SaveDocumentToDbAsync(new DocumentModel
    //        {
    //            Name = document.Name,
    //            IsAccepted = isApproved
    //        });

    //        return Done(); // Indicate that this activity is complete
    //    }

    //    private Task SaveDocumentToDbAsync(DocumentModel document)
    //    {
    //        // Your logic to save the document to the database goes here
    //        // This can be an EF Core context save operation
    //        return Task.CompletedTask;
    //    }
    //}
}
