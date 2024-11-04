namespace ElsaServer.Models
{
    public class DocumentModel
    {
        public string Name { get; set; }
        public bool IsAccepted { get; set; }
        public int? RequestId { get; set; }
        public string UserId { get; set; }
    }
}
