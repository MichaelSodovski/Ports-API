using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsApi
{
    public class FileContent
    {
        public byte[]? Content { get; set; }
        public string? MimeType { get; set; }
    }
}
