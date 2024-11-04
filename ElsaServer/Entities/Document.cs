namespace ElsaServer.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAcccepted { get; set; }
        public int RequestId { get; set; }
    }
}
