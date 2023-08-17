using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortsApi; // Add a semicolon here
using PortsApi.Models;

namespace PortsApi
{
    public partial class TESTLog
    {
        public Guid Id { get; set; }
        public string? User { get; set; }
        public DateTime CreationDate { get; set; }
        public string? EventDetails { get; set; }
        public string? EventType { get; set; }
    }
}

