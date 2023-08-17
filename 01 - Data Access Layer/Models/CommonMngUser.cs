using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngUser
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<CommonMngFilesPermission> CommonMngFilesPermissions { get; } = new List<CommonMngFilesPermission>();
}
