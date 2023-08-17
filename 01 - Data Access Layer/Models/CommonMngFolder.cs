using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngFolder
{
    public int Id { get; set; }

    public string? FolderName { get; set; }

    public int? LayerId { get; set; }

    public int? FeatureId { get; set; }

    public virtual ICollection<CommonMngFile> CommonMngFiles { get; } = new List<CommonMngFile>();
}
