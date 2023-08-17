using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsApi
{
    public class GetFileRequest
    {
        public string? featureID { get; set; }
        public string? fileID { get; set; }
        public string? fileName { get; set; }
        public string? folderID { get; set; }
        public string? folderName { get; set;}
        public string? id { get; set; }
        public string? layerID { get; set; }
    }
}
