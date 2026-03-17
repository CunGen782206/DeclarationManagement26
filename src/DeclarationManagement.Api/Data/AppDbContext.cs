using DeclarationManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeclarationManagement.Api.Data;

/// <summary>
/// 系统数据库上下文，统一管理实体映射与关系约束。
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// 构造函数。
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Users 数据集。
    /// </summary>
    public DbSet<User> Users => Set<User>();
    /// <summary>
    /// Departments 数据集。
    /// </summary>
    public DbSet<Department> Departments => Set<Department>();
    /// <summary>
    /// ProjectCategories 数据集。
    /// </summary>
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    /// <summary>
    /// UserPreReviewDepartments 数据集。
    /// </summary>
    public DbSet<UserPreReviewDepartment> UserPreReviewDepartments => Set<UserPreReviewDepartment>();
    /// <summary>
    /// UserInitialReviewCategories 数据集。
    /// </summary>
    public DbSet<UserInitialReviewCategory> UserInitialReviewCategories => Set<UserInitialReviewCategory>();
    /// <summary>
    /// DeclarationTasks 数据集。
    /// </summary>
    public DbSet<DeclarationTask> DeclarationTasks => Set<DeclarationTask>();
    /// <summary>
    /// Declarations 数据集。
    /// </summary>
    public DbSet<Declaration> Declarations => Set<Declaration>();
    /// <summary>
    /// DeclarationAttachments 数据集。
    /// </summary>
    public DbSet<DeclarationAttachment> DeclarationAttachments => Set<DeclarationAttachment>();
    /// <summary>
    /// DeclarationReviewRecords 数据集。
    /// </summary>
    public DbSet<DeclarationReviewRecord> DeclarationReviewRecords => Set<DeclarationReviewRecord>();
    /// <summary>
    /// DeclarationFlowLogs 数据集。
    /// </summary>
    public DbSet<DeclarationFlowLog> DeclarationFlowLogs => Set<DeclarationFlowLog>();

    /// <summary>
    /// OnModelCreating 方法。
    /// </summary>
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
            entity.HasOne(x => x.Department)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
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
            entity.HasOne(x => x.User)
                .WithMany(x => x.UserPreReviewDepartments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Department)
                .WithMany(x => x.UserPreReviewDepartments)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // UserInitialReviewCategories（复合主键）
        modelBuilder.Entity<UserInitialReviewCategory>(entity =>
        {
            entity.ToTable("UserInitialReviewCategories");
            entity.HasKey(x => new { x.UserId, x.ProjectCategoryId });
            entity.HasOne(x => x.User)
                .WithMany(x => x.UserInitialReviewCategories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.ProjectCategory)
                .WithMany(x => x.UserInitialReviewCategories)
                .HasForeignKey(x => x.ProjectCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // DeclarationTasks
        modelBuilder.Entity<DeclarationTask>(entity =>
        {
            entity.ToTable("DeclarationTasks");
            entity.Property(x => x.TaskName).HasMaxLength(200).IsRequired();
            entity.HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
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
            entity.HasOne(x => x.Task)
                .WithMany(x => x.Declarations)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.ApplicantUser)
                .WithMany()
                .HasForeignKey(x => x.ApplicantUserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Department)
                .WithMany(x => x.Declarations)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.ProjectCategory)
                .WithMany(x => x.Declarations)
                .HasForeignKey(x => x.ProjectCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
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
