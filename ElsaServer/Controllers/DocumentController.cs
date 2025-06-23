using Elsa;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Workflows.Runtime.Contracts;
using ElsaServer.Interfaces;
using ElsaServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElsaServer.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly IWorkflowRuntime _workflowRuntime;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;

        public DocumentController(IDocumentService documentService, IWorkflowRuntime workflowRuntime, IWorkflowInstanceStore workflowInstanceStore)
        {
            _documentService = documentService;
            _workflowRuntime = workflowRuntime;
            _workflowInstanceStore = workflowInstanceStore;
        }

        [HttpPost("SaveDocumentAsync")]
        public async Task<IActionResult> SaveDocumentAsync([FromBody] DocumentModel doc)
        {
            var res = await _documentService.SaveDocumentAsync(doc);

            return res == 1 ? Ok("Document saved successfully.") : BadRequest("Document failed to be saved.");
        }
        
        [HttpPost("UpdateDocumentAsync")]
        public async Task<IActionResult> UpdateDocumentAsync([FromBody] DocumentModel doc)
        {
            var res = await _documentService.UpdateDocumentAsync(doc);

            return res == 1 ? Ok("Document saved successfully.") : BadRequest("Document failed to be saved.");
        }
        
        [HttpPost("Test")]
        public async Task<IActionResult> Test()
        {
            return Ok(1000);
        }

        [HttpPost("{instanceId}/execute")]
        public async Task<IActionResult> ResumeWorkflow(string instanceId)
        {
            await _workflowRuntime.ResumeWorkflowAsync(instanceId, null);

            return Ok(new { message = "Workflow resumed successfully" });
        }

        //[HttpPost("start")]
        //public async Task<IActionResult> StartWorkflow(string workflowDefinitionId)
        //{
        //    // Generate a unique Correlation ID
        //    var correlationId = Guid.NewGuid().ToString();

        //    // Start the workflow with the Correlation ID
        //    var startWorkflowOptions = new StartWorkflowOptions
        //    {
        //        CorrelationId = correlationId
        //    };

        //    var workflowInstance = await _workflowRuntime.StartWorkflowAsync(workflowDefinitionId, startWorkflowOptions);

        //    return Ok(new
        //    {
        //        message = "Workflow started successfully",
        //        workflowInstanceId = workflowInstance.Id,
        //        correlationId = workflowInstance.CorrelationId
        //    });
        //}

        //[HttpPost("resume")]
        //public async Task<IActionResult> ResumeWorkflow(string correlationId, [FromBody] WorkflowInput input)
        //{
        //    await _workflowRuntime.ResumeWorkflowAsync(instanceId: null, correlationId: correlationId, input: input);

        //    return Ok(new { message = "Workflow resumed successfully" });
        //}

        //[HttpPost("{instanceId}/execute")]
        //public async Task<IActionResult> ResumeWorkflow(string instanceId)
        //{
        //    var workflow = await _workflowInstanceStore.FindByIdAsync(instanceId);

        //    if (workflow == null)
        //        return NotFound(new { errorMessage = "No workflow found with the given instanceId." });

        //    await _workflowRuntime.ResumeWorkflowAsync(instanceId, null);

        //    return Ok(new { message = "Workflow resumed successfully", instanceId });
        //}
    }

    public class WorkflowInput
    {
        public string Id { get; set; }
        public bool IsAccepted { get; set; }
        public string EventResponse { get; set; }
    }
}
