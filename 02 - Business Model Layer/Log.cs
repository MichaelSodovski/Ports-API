namespace PortsApi
{
    public class Log
    {
        public Guid Id { get; set; }
        public string? User { get; set; }
        public DateTime CreationDate { get; set; }
        public string? EventDetails { get; set; }
        public string? EventType { get; set; }
    }
}
