using DocumentService.Data;
using DocumentService.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Services
{
    public class DocumentServiceImp : IDocumentService
    {
        private readonly DocumentContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public DocumentServiceImp(DocumentContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return await _dbContext.Documents.ToListAsync();
        }

        public async Task<Document> GetDocumentByIdAsync(int documentId)
        {
            return await _dbContext.Documents.FindAsync(documentId);
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            document.CreatedAt = DateTime.UtcNow;
            _dbContext.Documents.Add(document);
            await _dbContext.SaveChangesAsync();
            return document;
        }

        public async Task<bool> UpdateDocumentAsync(Document document)
        {
            var existingDocument = await _dbContext.Documents.FindAsync(document.DocumentID);
            if (existingDocument == null)
                return false;
            existingDocument.FlightID = document.FlightID;
            existingDocument.DocumentType = document.DocumentType;
            existingDocument.Title = document.Title;
            existingDocument.Content = document.Content;
            existingDocument.UpdatedBy = document.UpdatedBy;
            existingDocument.UpdatedAt = DateTime.UtcNow;
            existingDocument.IsConfirmed = document.IsConfirmed;
            _dbContext.Entry(existingDocument).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _dbContext.Documents.FindAsync(documentId);
            if (document == null) return false;
            _dbContext.Documents.Remove(document);
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public async Task<bool> UploadDocumentAsync(Document document, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                document.FilePath = filePath;
                _dbContext.Documents.Add(document);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<(Document document, byte[] fileContent)> DownloadDocumentAsync(int documentId)
        {
            var document = await _dbContext.Documents.FindAsync(documentId);
            if (document == null || string.IsNullOrEmpty(document.FilePath))
                return (null, null);
            var fileContent = await File.ReadAllBytesAsync(document.FilePath);
            return (document, fileContent);
        }

        public async Task<IEnumerable<Document>> SearchDocumentsAsync(string keyword, string documentType, int? createdBy, DateTime? updatedDate)
        {
            var query = _dbContext.Documents.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(d => d.Title.Contains(keyword));
            if (!string.IsNullOrEmpty(documentType))
                query = query.Where(d => d.DocumentType == documentType);
            if (createdBy.HasValue)
                query = query.Where(d => d.CreatedBy == createdBy.Value);
            if (updatedDate.HasValue)
                query = query.Where(d => d.UpdatedAt.HasValue && d.UpdatedAt.Value.Date == updatedDate.Value.Date);
            return await query.ToListAsync();
        }
    }
}
