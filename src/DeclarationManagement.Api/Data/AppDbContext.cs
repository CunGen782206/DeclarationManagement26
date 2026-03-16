using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Data;

/// <summary>
/// 系统数据库上下文，统一管理实体映射与关系约束。
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    public DbSet<UserPreReviewDepartment> UserPreReviewDepartments => Set<UserPreReviewDepartment>();
    public DbSet<UserInitialReviewCategory> UserInitialReviewCategories => Set<UserInitialReviewCategory>();
    public DbSet<DeclarationTask> DeclarationTasks => Set<DeclarationTask>();
    public DbSet<Declaration> Declarations => Set<Declaration>();
    public DbSet<DeclarationAttachment> DeclarationAttachments => Set<DeclarationAttachment>();
    public DbSet<DeclarationReviewRecord> DeclarationReviewRecords => Set<DeclarationReviewRecord>();
    public DbSet<DeclarationFlowLog> DeclarationFlowLogs => Set<DeclarationFlowLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasIndex(x => x.JobNumber).IsUnique();
            entity.Property(x => x.JobNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.FullName).HasMaxLength(50).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PasswordSalt).HasMaxLength(200);
        });

        // Departments
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Departments");
            entity.HasIndex(x => x.Name).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        // ProjectCategories
        modelBuilder.Entity<ProjectCategory>(entity =>
        {
            entity.ToTable("ProjectCategories");
            entity.HasIndex(x => x.Name).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        // UserPreReviewDepartments（复合主键）
        modelBuilder.Entity<UserPreReviewDepartment>(entity =>
        {
            entity.ToTable("UserPreReviewDepartments");
            entity.HasKey(x => new { x.UserId, x.DepartmentId });
        });

        // UserInitialReviewCategories（复合主键）
        modelBuilder.Entity<UserInitialReviewCategory>(entity =>
        {
            entity.ToTable("UserInitialReviewCategories");
            entity.HasKey(x => new { x.UserId, x.ProjectCategoryId });
        });

        // DeclarationTasks
        modelBuilder.Entity<DeclarationTask>(entity =>
        {
            entity.ToTable("DeclarationTasks");
            entity.Property(x => x.TaskName).HasMaxLength(200).IsRequired();
        });

        // Declarations
        modelBuilder.Entity<Declaration>(entity =>
        {
            entity.ToTable("Declarations");
            entity.Property(x => x.PrincipalName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.ContactPhone).HasMaxLength(50).IsRequired();
            entity.Property(x => x.ProjectName).HasMaxLength(300).IsRequired();
            entity.Property(x => x.ApprovalDocumentName).HasMaxLength(300);
            entity.Property(x => x.SealUnitAndDate).HasMaxLength(300);
        });

        // DeclarationAttachments
        modelBuilder.Entity<DeclarationAttachment>(entity =>
        {
            entity.ToTable("DeclarationAttachments");
            entity.Property(x => x.OriginalFileName).HasMaxLength(260).IsRequired();
            entity.Property(x => x.StorageFileName).HasMaxLength(260).IsRequired();
            entity.Property(x => x.StoragePath).HasMaxLength(500).IsRequired();
            entity.Property(x => x.ContentType).HasMaxLength(100);
        });

        // DeclarationReviewRecords
        modelBuilder.Entity<DeclarationReviewRecord>(entity =>
        {
            entity.ToTable("DeclarationReviewRecords");
            entity.Property(x => x.Reason).HasMaxLength(1000);
            entity.Property(x => x.Remark).HasMaxLength(1000);
            entity.Property(x => x.RecognizedAmount).HasColumnType("decimal(18,2)");
        });

        // DeclarationFlowLogs
        modelBuilder.Entity<DeclarationFlowLog>(entity =>
        {
            entity.ToTable("DeclarationFlowLogs");
            entity.Property(x => x.Note).HasMaxLength(1000);
        });
    }
}
