using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngLayer
{
    public int LayerId { get; set; }

    public string? LayerName { get; set; }

    public virtual ICollection<CommonMngFile> CommonMngFiles { get; } = new List<CommonMngFile>();

    public virtual ICollection<CommonMngFilesPermission> CommonMngFilesPermissions { get; } = new List<CommonMngFilesPermission>();
}
