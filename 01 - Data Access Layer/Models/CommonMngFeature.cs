using System;
using System.Collections.Generic;

namespace PortsApi.Models;

public partial class CommonMngFeature
{
    public int Objectid { get; set; }

    public double? MaaganId { get; set; }

    public double? DocsNum { get; set; }

    public string? PlanDate { get; set; }

    public double? PlanNum { get; set; }

    public string? ProjDate { get; set; }

    public double? ProjectId { get; set; }

    public double? Version { get; set; }

    public string? MgnUpd { get; set; }

    public double? UserId { get; set; }

    public double? MgnType { get; set; }

    public double? AreaId { get; set; }

    public double? TifulType { get; set; }

    public string? TifulName { get; set; }

    public string? Usage { get; set; }

    public double? CovPrjId { get; set; }

    public double? GeomArea { get; set; }

    public double? GeomLen { get; set; }

    public string? AsMade { get; set; }

    public string? Constrct { get; set; }

    public string? Status { get; set; }

    public string? UpdDate { get; set; }

    public string? GroupList { get; set; }

    public virtual ICollection<CommonMngFile> CommonMngFiles { get; } = new List<CommonMngFile>();
}
