namespace PortsApi
{
    public class FilePermission
    {
        public int? ID { get; set; }
        public string? UserName { get; set; }
        public bool? CanView { get; set; }
        public bool? CanEdit { get; set; }
        public int? UserID { get; set; }
        public User? User { get; set; }
        public int? LayerID { get; set; }

}
}

