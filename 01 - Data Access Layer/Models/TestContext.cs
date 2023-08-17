using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PortsApi.Models;

public partial class TestContext : DbContext
{
    public TestContext()
    {
    }

    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CommonMngFeature> CommonMngFeatures { get; set; }

    public virtual DbSet<CommonMngFile> CommonMngFiles { get; set; }

    public virtual DbSet<CommonMngFilesPermission> CommonMngFilesPermissions { get; set; }

    public virtual DbSet<CommonMngFolder> CommonMngFolders { get; set; }

    public virtual DbSet<CommonMngLayer> CommonMngLayers { get; set; }

    public virtual DbSet<CommonMngUser> CommonMngUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PORTS;Database=TEST;User Id=sa;Password=A@a123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommonMngFeature>(entity =>
        {
            entity.HasKey(e => e.Objectid).HasName("PK__COMMON_M__F4B70D850EA47FFE");

            entity.ToTable("COMMON_MNG_FEATURES");

            entity.Property(e => e.Objectid)
                .ValueGeneratedNever()
                .HasColumnName("OBJECTID");
            entity.Property(e => e.AreaId).HasColumnName("AREA_ID");
            entity.Property(e => e.AsMade)
                .HasMaxLength(64)
                .HasColumnName("AS_MADE");
            entity.Property(e => e.Constrct)
                .HasMaxLength(64)
                .HasColumnName("CONSTRCT");
            entity.Property(e => e.CovPrjId).HasColumnName("COV_PRJ_ID");
            entity.Property(e => e.DocsNum).HasColumnName("DOCS_NUM");
            entity.Property(e => e.GeomArea).HasColumnName("GEOM_AREA");
            entity.Property(e => e.GeomLen).HasColumnName("GEOM_LEN");
            entity.Property(e => e.GroupList)
                .HasMaxLength(255)
                .HasColumnName("GROUP_LIST");
            entity.Property(e => e.MaaganId).HasColumnName("MAAGAN_ID");
            entity.Property(e => e.MgnType).HasColumnName("MGN_TYPE");
            entity.Property(e => e.MgnUpd)
                .HasMaxLength(16)
                .HasColumnName("MGN_UPD");
            entity.Property(e => e.PlanDate)
                .HasMaxLength(16)
                .HasColumnName("PLAN_DATE");
            entity.Property(e => e.PlanNum).HasColumnName("PLAN_NUM");
            entity.Property(e => e.ProjDate)
                .HasMaxLength(16)
                .HasColumnName("PROJ_DATE");
            entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(64)
                .HasColumnName("STATUS");
            entity.Property(e => e.TifulName)
                .HasMaxLength(24)
                .HasColumnName("TIFUL_NAME");
            entity.Property(e => e.TifulType).HasColumnName("TIFUL_TYPE");
            entity.Property(e => e.UpdDate)
                .HasMaxLength(32)
                .HasColumnName("UPD_DATE");
            entity.Property(e => e.Usage)
                .HasMaxLength(25)
                .HasColumnName("USAGE");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.Version).HasColumnName("VERSION");
        });

        modelBuilder.Entity<CommonMngFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COMMON_M__3214EC27C3C87AC1");

            entity.ToTable("COMMON_MNG_FILES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.FileId).HasColumnName("FileID");
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.FolderId).HasColumnName("FolderID");
            entity.Property(e => e.FolderName).HasMaxLength(500);
            entity.Property(e => e.LayerId).HasColumnName("LayerID");

            entity.HasOne(d => d.Feature).WithMany(p => p.CommonMngFiles)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("FK__COMMON_MN__Featu__31EC6D26");

            entity.HasOne(d => d.Folder).WithMany(p => p.CommonMngFiles)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("FK__COMMON_MN__Folde__300424B4");

            entity.HasOne(d => d.Layer).WithMany(p => p.CommonMngFiles)
                .HasForeignKey(d => d.LayerId)
                .HasConstraintName("FK__COMMON_MN__Layer__30F848ED");
        });

        modelBuilder.Entity<CommonMngFilesPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COMMON_M__3214EC27643AEBF9");

            entity.ToTable("COMMON_MNG_FILES_PERMISSIONS");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.LayerId).HasColumnName("LayerID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Layer).WithMany(p => p.CommonMngFilesPermissions)
                .HasForeignKey(d => d.LayerId)
                .HasConstraintName("FK_CommonMngFilesPermissions_CommonMngLayers");

            entity.HasOne(d => d.User).WithMany(p => p.CommonMngFilesPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__COMMON_MN__UserI__29572725");
        });

        modelBuilder.Entity<CommonMngFolder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COMMON_M__3214EC2736F76125");

            entity.ToTable("COMMON_MNG_FOLDERS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.FolderName).HasMaxLength(500);
            entity.Property(e => e.LayerId).HasColumnName("LayerID");
        });

        modelBuilder.Entity<CommonMngLayer>(entity =>
        {
            entity.HasKey(e => e.LayerId).HasName("PK__layers_t__83790DA20E2DE535");

            entity.ToTable("COMMON_MNG_LAYERS");

            entity.Property(e => e.LayerId)
                .ValueGeneratedNever()
                .HasColumnName("LayerID");
            entity.Property(e => e.LayerName).HasMaxLength(255);
        });

        modelBuilder.Entity<CommonMngUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__COMMON_M__CB9A1CDF1BAB6A38");

            entity.ToTable("COMMON_MNG_USERS");

            entity.HasIndex(e => e.UserName, "UQ__COMMON_M__C9F284560392A68E").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
