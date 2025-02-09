using Elsa.Services;
using Elsa.Workflows.Models;
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
        //private readonly IWorkflowInstanceDispatcher _workflowInstanceDispatcher;
        private readonly IWorkflowRuntime _workflowRuntime;

        public DocumentController(IDocumentService documentService, /*IWorkflowInstanceDispatcher workflowInstanceDispatcher*/ IWorkflowRuntime workflowRuntime)
        {
            _documentService = documentService;
            //_workflowInstanceDispatcher = workflowInstanceDispatcher ?? throw new ArgumentNullException(nameof(workflowInstanceDispatcher));
            _workflowRuntime = workflowRuntime;
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

            //var dispatchRequest = new ExecuteWorkflowInstanceRequest(instanceId)
            //{
            //    Input = null
            //};

            //await _workflowInstanceDispatcher.DispatchAsync(dispatchRequest);

            return Ok(new { message = "Workflow resumed successfully" });
        }
    }
}
