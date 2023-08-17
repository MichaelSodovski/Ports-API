namespace PortsApi
{
    public class User
    {
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
    }
}

