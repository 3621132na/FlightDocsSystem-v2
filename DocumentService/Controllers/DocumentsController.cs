using DocumentService.Models;
using DocumentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocumentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DocumentsController(IDocumentService documentService, IHttpContextAccessor httpContextAccessor)
        {
            _documentService = documentService;
            _httpContextAccessor = httpContextAccessor;
        }
        [Authorize(Roles = "Admin, GO")]
        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();
            return Ok(document);
        }
        [Authorize(Roles = "Admin, GO")]
        [HttpPost]
        public async Task<IActionResult> CreateDocument(Document document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdDocument = await _documentService.CreateDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.DocumentID }, createdDocument);
        }
        [Authorize(Roles = "Admin, GO")]
        [HttpPut("{id}/grant-edit-permission")]
        public async Task<IActionResult> GrantEditPermission(int id, bool canEdit)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();
            document.CanEdit = canEdit;
            var result = await _documentService.UpdateDocumentAsync(document);
            if (result)
                return NoContent();
            return BadRequest("Unable to update document permission.");
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, Document document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != document.DocumentID)
                return BadRequest();
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var doc = await _documentService.GetDocumentByIdAsync(id);
            if (doc == null)
                return NotFound();
            if ((userRole == "Pilot" || userRole == "Crew") && !doc.CanEdit)
                return Forbid("You do not have permission to edit this document.");
            var result = await _documentService.UpdateDocumentAsync(document);
            if (result)
                return NoContent();
            return NotFound();
        }
        [Authorize(Roles = "Admin, GO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (result)
                return NoContent();
            return NotFound();
        }
        [Authorize(Roles = "Admin, GO")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument(Document document, IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _documentService.UploadDocumentAsync(document, file);
            if (result)
                return Ok(document);
            return BadRequest("Unable to upload document.");
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var (document, fileContent) = await _documentService.DownloadDocumentAsync(id);
            if (document == null || fileContent == null)
                return NotFound();

            return File(fileContent, "application/octet-stream", Path.GetFileName(document.FilePath));
        }
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchDocuments([FromQuery] string keyword, [FromQuery] string documentType, [FromQuery] int? createdBy, [FromQuery] DateTime? updatedDate)
        {
            var documents = await _documentService.SearchDocumentsAsync(keyword, documentType, createdBy, updatedDate);
            return Ok(documents);
        }
    }
}
