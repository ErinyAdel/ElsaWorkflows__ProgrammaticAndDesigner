using ElsaServer.Data;
using ElsaServer.Entities;
using ElsaServer.Interfaces;
using ElsaServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ElsaServer.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<int> SaveDocumentAsync(DocumentModel document)
        {
            try
            {
                var isExisted = _context.Documents.Where(d => d.RequestId == document.RequestId).FirstOrDefault();

                if (isExisted == null) { 

                    Document mappedDoc = new Document()
                    {
                        Name = document.Name,
                        IsAcccepted = document.IsAccepted
                    };
                    _context.Documents.Add(mappedDoc);
                }
                else
                {
                    Document mappedDoc = new Document()
                    {
                        Name = document.Name,
                    };
                    _context.Documents.Update(mappedDoc);
                }
                await _context.SaveChangesAsync();

                return 1;
            }
            catch { return 0; }
        }
        
        public async Task<int> UpdateDocumentAsync(DocumentModel document)
        {
            try
            {
                Document mappedDoc = new Document()
                {
                    Name = document.Name,
                };

                _context.Documents.Update(mappedDoc);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch { return 0; }
        }

        public async Task<bool> DocumentExistsAsync(int requestId)
        {
            // Replace with actual logic to check if the document exists
            return await _context.Documents.AnyAsync(d => d.RequestId == requestId);
        }
    }
}
