using ElsaServer.Interfaces;
using ElsaServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElsaServer.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
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
    }
}
