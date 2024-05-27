using DocumentService.Models;

namespace DocumentService.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> GetDocumentByIdAsync(int documentId);
        Task<Document> CreateDocumentAsync(Document document);
        Task<bool> UpdateDocumentAsync(Document document);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<IEnumerable<Document>> SearchDocumentsAsync(string keyword, string documentType, int? createdBy, DateTime? updatedDate);
        Task<bool> UploadDocumentAsync(Document document, IFormFile file);
        Task<(Document document, byte[] fileContent)> DownloadDocumentAsync(int documentId);
    }
}
