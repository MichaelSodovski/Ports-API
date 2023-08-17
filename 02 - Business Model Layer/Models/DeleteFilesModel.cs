using PortsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortsApi
{
    public class DeleteFilesModel
    {
        public File[]? files { get; set; }
        public int featureId { get; set; }
    }
}
