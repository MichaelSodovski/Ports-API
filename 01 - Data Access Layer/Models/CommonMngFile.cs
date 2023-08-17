using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngFile
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public int? FolderId { get; set; }

    public int? LayerId { get; set; }

    public int? FeatureId { get; set; }

    public string? FileId { get; set; }

    public string? FolderName { get; set; }

    public virtual CommonMngFeature? Feature { get; set; }

    public virtual CommonMngFolder? Folder { get; set; }

    public virtual CommonMngLayer? Layer { get; set; }
}
