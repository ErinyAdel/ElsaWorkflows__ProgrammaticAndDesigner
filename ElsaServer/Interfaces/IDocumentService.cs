using ElsaServer.Models;

namespace ElsaServer.Interfaces
{
    public interface IDocumentService
    {
        Task<int> SaveDocumentAsync(DocumentModel document);
        Task<int> UpdateDocumentAsync(DocumentModel document);
        Task<bool> DocumentExistsAsync(int requestId);
    }
}
