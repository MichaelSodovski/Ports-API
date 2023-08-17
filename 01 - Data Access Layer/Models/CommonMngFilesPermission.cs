using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngFilesPermission
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public bool? CanView { get; set; }

    public bool? CanEdit { get; set; }

    public int? UserId { get; set; }

    public int? LayerId { get; set; }

    public virtual CommonMngLayer? Layer { get; set; }

    public virtual CommonMngUser? User { get; set; }
}
