# 教学质量工程项目申报管理系统 - SQL Server 数据库设计

## 1. ER 图说明（文字版）

> 目标：满足关系规范化、审批多轮记录、附件管理、申报任务时间窗口、用户权限多选关联。

### 核心实体

1. **Users（用户）**
   - 存储登录账号、姓名、所属部门、密码哈希、是否超管、是否启用。

2. **Departments（部门）**
   - 统一部门字典，供用户所属部门、申报表单所属部门、预审权限关联复用。

3. **ProjectCategories（项目类别）**
   - 统一项目类别字典，供申报表单项目类别、初审权限关联复用。

4. **DeclarationTasks（申报任务）**
   - 申报批次/任务实体，包含开始时间和结束时间（时间窗口）、是否启用。

5. **Declarations（申报单）**
   - 申报主表；与申报任务、申报人、部门、项目类别关联。
   - 保存当前流程状态（如待预审、待初审、初审通过等）和当前流程节点。
   - 驳回后修改重提通过 `VersionNo` 支持版本递增。

6. **DeclarationAttachments（申报附件）**
   - 附件元数据（原文件名、存储路径、大小、上传人、上传时间）。
   - 一对多关联 Declarations。

7. **DeclarationReviewRecords（审核记录）**
   - 每次预审/初审都新增一条记录（不可覆盖），实现多轮审核累计。
   - 记录审核层级、动作（通过/不通过/驳回）、原因、认定信息等。

8. **DeclarationFlowLogs（流程日志）**
   - 每次状态变更都新增日志（from/to、操作人、动作、备注、时间）。
   - 与审核记录共同保证流程可追溯。

9. **UserPreReviewDepartments（用户预审权限关联）**
   - 用户与部门多对多（预审权限）。
   - 使用关联表，不使用逗号拼接。

10. **UserInitialReviewCategories（用户初审权限关联）**
    - 用户与项目类别多对多（初审权限）。
    - 使用关联表，不使用逗号拼接。

### 关系摘要

- Users (1) —— (N) Declarations（申报人）
- DeclarationTasks (1) —— (N) Declarations
- Departments (1) —— (N) Users（所属部门）
- Departments (1) —— (N) Declarations（表单所属部门）
- ProjectCategories (1) —— (N) Declarations
- Declarations (1) —— (N) DeclarationAttachments
- Declarations (1) —— (N) DeclarationReviewRecords
- Declarations (1) —— (N) DeclarationFlowLogs
- Users (N) —— (N) Departments（通过 UserPreReviewDepartments）
- Users (N) —— (N) ProjectCategories（通过 UserInitialReviewCategories）

---

## 2. SQL 建表语句（SQL Server）

```sql
-- ========================================
-- 0) 基础设置
-- ========================================
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ========================================
-- 1) 字典表
-- ========================================
CREATE TABLE dbo.Departments (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(100) NOT NULL,
    SortOrder       INT NOT NULL CONSTRAINT DF_Departments_SortOrder DEFAULT(0),
    IsEnabled       BIT NOT NULL CONSTRAINT DF_Departments_IsEnabled DEFAULT(1),
    CreatedAt       DATETIME2(0) NOT NULL CONSTRAINT DF_Departments_CreatedAt DEFAULT(SYSUTCDATETIME()),
    CONSTRAINT UQ_Departments_Name UNIQUE (Name)
);
GO

CREATE TABLE dbo.ProjectCategories (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(100) NOT NULL,
    SortOrder       INT NOT NULL CONSTRAINT DF_ProjectCategories_SortOrder DEFAULT(0),
    IsEnabled       BIT NOT NULL CONSTRAINT DF_ProjectCategories_IsEnabled DEFAULT(1),
    CreatedAt       DATETIME2(0) NOT NULL CONSTRAINT DF_ProjectCategories_CreatedAt DEFAULT(SYSUTCDATETIME()),
    CONSTRAINT UQ_ProjectCategories_Name UNIQUE (Name)
);
GO

-- ========================================
-- 2) 用户与权限
-- ========================================
CREATE TABLE dbo.Users (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    JobNumber           NVARCHAR(50) NOT NULL,           -- 工号（账号）
    FullName            NVARCHAR(50) NOT NULL,
    DepartmentId        INT NOT NULL,
    PasswordHash        NVARCHAR(200) NOT NULL,
    PasswordSalt        NVARCHAR(200) NULL,
    IsSuperAdmin        BIT NOT NULL CONSTRAINT DF_Users_IsSuperAdmin DEFAULT(0),
    IsEnabled           BIT NOT NULL CONSTRAINT DF_Users_IsEnabled DEFAULT(1),
    LastLoginAt         DATETIME2(0) NULL,
    CreatedAt           DATETIME2(0) NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT(SYSUTCDATETIME()),
    UpdatedAt           DATETIME2(0) NULL,
    CONSTRAINT UQ_Users_JobNumber UNIQUE (JobNumber),
    CONSTRAINT FK_Users_Departments_DepartmentId FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(Id)
);
GO

-- 预审权限：用户 <-> 部门（多对多）
CREATE TABLE dbo.UserPreReviewDepartments (
    UserId              BIGINT NOT NULL,
    DepartmentId        INT NOT NULL,
    CreatedAt           DATETIME2(0) NOT NULL CONSTRAINT DF_UserPreReviewDepartments_CreatedAt DEFAULT(SYSUTCDATETIME()),
    CONSTRAINT PK_UserPreReviewDepartments PRIMARY KEY (UserId, DepartmentId),
    CONSTRAINT FK_UserPreReviewDepartments_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserPreReviewDepartments_Departments_DepartmentId FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(Id)
);
GO

-- 初审权限：用户 <-> 项目类别（多对多）
CREATE TABLE dbo.UserInitialReviewCategories (
    UserId              BIGINT NOT NULL,
    ProjectCategoryId   INT NOT NULL,
    CreatedAt           DATETIME2(0) NOT NULL CONSTRAINT DF_UserInitialReviewCategories_CreatedAt DEFAULT(SYSUTCDATETIME()),
    CONSTRAINT PK_UserInitialReviewCategories PRIMARY KEY (UserId, ProjectCategoryId),
    CONSTRAINT FK_UserInitialReviewCategories_Users_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserInitialReviewCategories_ProjectCategories_ProjectCategoryId FOREIGN KEY (ProjectCategoryId) REFERENCES dbo.ProjectCategories(Id)
);
GO

-- ========================================
-- 3) 申报任务
-- ========================================
CREATE TABLE dbo.DeclarationTasks (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    TaskName            NVARCHAR(200) NOT NULL,
    StartAt             DATETIME2(0) NOT NULL,
    EndAt               DATETIME2(0) NOT NULL,
    IsEnabled           BIT NOT NULL CONSTRAINT DF_DeclarationTasks_IsEnabled DEFAULT(1),
    CreatedByUserId     BIGINT NOT NULL,
    CreatedAt           DATETIME2(0) NOT NULL CONSTRAINT DF_DeclarationTasks_CreatedAt DEFAULT(SYSUTCDATETIME()),
    UpdatedAt           DATETIME2(0) NULL,
    CONSTRAINT CK_DeclarationTasks_TimeWindow CHECK (EndAt > StartAt),
    CONSTRAINT FK_DeclarationTasks_Users_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES dbo.Users(Id)
);
GO

-- ========================================
-- 4) 项目申报主表
-- ========================================
CREATE TABLE dbo.Declarations (
    Id                          BIGINT IDENTITY(1,1) PRIMARY KEY,
    TaskId                       BIGINT NOT NULL,
    ApplicantUserId              BIGINT NOT NULL,
    PrincipalName                NVARCHAR(100) NOT NULL,      -- 负责人
    ContactPhone                 NVARCHAR(50) NOT NULL,
    DepartmentId                 INT NOT NULL,
    ProjectName                  NVARCHAR(300) NOT NULL,
    ProjectCategoryId            INT NOT NULL,
    ProjectLevel                 TINYINT NOT NULL,            -- 1国家级 2市级 3行业/教指委级 4校级
    AwardLevel                   TINYINT NOT NULL,            -- 1一等第 2二等第 3三等第 4四等第 5无
    ParticipationType            TINYINT NOT NULL,            -- 1个人 2团队
    ApprovalDocumentName         NVARCHAR(300) NULL,
    SealUnitAndDate              NVARCHAR(300) NULL,
    ProjectContent               NVARCHAR(MAX) NULL,
    ProjectAchievement           NVARCHAR(MAX) NULL,

    CurrentStatus                TINYINT NOT NULL,            -- 流程状态枚举
    CurrentNode                  TINYINT NOT NULL,            -- 1申报 2预审 3初审 4结束
    VersionNo                    INT NOT NULL CONSTRAINT DF_Declarations_VersionNo DEFAULT(1),

    SubmittedAt                  DATETIME2(0) NULL,
    CreatedAt                    DATETIME2(0) NOT NULL CONSTRAINT DF_Declarations_CreatedAt DEFAULT(SYSUTCDATETIME()),
    UpdatedAt                    DATETIME2(0) NULL,

    CONSTRAINT FK_Declarations_DeclarationTasks_TaskId FOREIGN KEY (TaskId) REFERENCES dbo.DeclarationTasks(Id),
    CONSTRAINT FK_Declarations_Users_ApplicantUserId FOREIGN KEY (ApplicantUserId) REFERENCES dbo.Users(Id),
    CONSTRAINT FK_Declarations_Departments_DepartmentId FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(Id),
    CONSTRAINT FK_Declarations_ProjectCategories_ProjectCategoryId FOREIGN KEY (ProjectCategoryId) REFERENCES dbo.ProjectCategories(Id)
);
GO

-- ========================================
-- 5) 附件
-- ========================================
CREATE TABLE dbo.DeclarationAttachments (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    DeclarationId       BIGINT NOT NULL,
    OriginalFileName    NVARCHAR(260) NOT NULL,
    StorageFileName     NVARCHAR(260) NOT NULL,
    StoragePath         NVARCHAR(500) NOT NULL,
    ContentType         NVARCHAR(100) NULL,
    FileSizeBytes       BIGINT NOT NULL,
    UploadedByUserId    BIGINT NOT NULL,
    UploadedAt          DATETIME2(0) NOT NULL CONSTRAINT DF_DeclarationAttachments_UploadedAt DEFAULT(SYSUTCDATETIME()),
    IsDeleted           BIT NOT NULL CONSTRAINT DF_DeclarationAttachments_IsDeleted DEFAULT(0),
    CONSTRAINT FK_DeclarationAttachments_Declarations_DeclarationId FOREIGN KEY (DeclarationId) REFERENCES dbo.Declarations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_DeclarationAttachments_Users_UploadedByUserId FOREIGN KEY (UploadedByUserId) REFERENCES dbo.Users(Id)
);
GO

-- ========================================
-- 6) 审核记录（多轮累加）
-- ========================================
CREATE TABLE dbo.DeclarationReviewRecords (
    Id                          BIGINT IDENTITY(1,1) PRIMARY KEY,
    DeclarationId               BIGINT NOT NULL,
    ReviewStage                 TINYINT NOT NULL,        -- 1预审 2初审
    ReviewAction                TINYINT NOT NULL,        -- 1通过 2不通过 3驳回
    Reason                      NVARCHAR(1000) NULL,

    RecognizedProjectLevel      TINYINT NULL,
    RecognizedAwardLevel        TINYINT NULL,
    RecognizedAmount            DECIMAL(18,2) NULL,
    Remark                      NVARCHAR(1000) NULL,

    ReviewedByUserId            BIGINT NOT NULL,
    ReviewedAt                  DATETIME2(0) NOT NULL CONSTRAINT DF_DeclarationReviewRecords_ReviewedAt DEFAULT(SYSUTCDATETIME()),

    CONSTRAINT FK_DeclarationReviewRecords_Declarations_DeclarationId FOREIGN KEY (DeclarationId) REFERENCES dbo.Declarations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_DeclarationReviewRecords_Users_ReviewedByUserId FOREIGN KEY (ReviewedByUserId) REFERENCES dbo.Users(Id),
    CONSTRAINT CK_DeclarationReviewRecords_RecognizedAmount CHECK (RecognizedAmount IS NULL OR RecognizedAmount >= 0)
);
GO

-- ========================================
-- 7) 流程日志（状态流转）
-- ========================================
CREATE TABLE dbo.DeclarationFlowLogs (
    Id                          BIGINT IDENTITY(1,1) PRIMARY KEY,
    DeclarationId               BIGINT NOT NULL,
    FromStatus                  TINYINT NULL,
    ToStatus                    TINYINT NOT NULL,
    ActionType                  TINYINT NOT NULL,        -- 例如：1提交 2预审通过 3预审不通过 4预审驳回 5初审通过 6初审不通过 7初审驳回 8重提
    OperatorUserId              BIGINT NOT NULL,
    Note                        NVARCHAR(1000) NULL,
    CreatedAt                   DATETIME2(0) NOT NULL CONSTRAINT DF_DeclarationFlowLogs_CreatedAt DEFAULT(SYSUTCDATETIME()),

    CONSTRAINT FK_DeclarationFlowLogs_Declarations_DeclarationId FOREIGN KEY (DeclarationId) REFERENCES dbo.Declarations(Id) ON DELETE CASCADE,
    CONSTRAINT FK_DeclarationFlowLogs_Users_OperatorUserId FOREIGN KEY (OperatorUserId) REFERENCES dbo.Users(Id)
);
GO

-- ========================================
-- 8) 关键索引
-- ========================================
CREATE INDEX IX_Users_DepartmentId ON dbo.Users(DepartmentId);

CREATE INDEX IX_UserPreReviewDepartments_DepartmentId ON dbo.UserPreReviewDepartments(DepartmentId);
CREATE INDEX IX_UserInitialReviewCategories_ProjectCategoryId ON dbo.UserInitialReviewCategories(ProjectCategoryId);

CREATE INDEX IX_DeclarationTasks_TimeWindow ON dbo.DeclarationTasks(StartAt, EndAt, IsEnabled);

CREATE INDEX IX_Declarations_TaskId ON dbo.Declarations(TaskId);
CREATE INDEX IX_Declarations_ApplicantUserId ON dbo.Declarations(ApplicantUserId);
CREATE INDEX IX_Declarations_DepartmentId ON dbo.Declarations(DepartmentId);
CREATE INDEX IX_Declarations_ProjectCategoryId ON dbo.Declarations(ProjectCategoryId);
CREATE INDEX IX_Declarations_CurrentStatus_SubmittedAt ON dbo.Declarations(CurrentStatus, SubmittedAt DESC);

CREATE INDEX IX_DeclarationAttachments_DeclarationId ON dbo.DeclarationAttachments(DeclarationId);

CREATE INDEX IX_DeclarationReviewRecords_DeclarationId_ReviewedAt ON dbo.DeclarationReviewRecords(DeclarationId, ReviewedAt DESC);
CREATE INDEX IX_DeclarationReviewRecords_ReviewStage ON dbo.DeclarationReviewRecords(ReviewStage);

CREATE INDEX IX_DeclarationFlowLogs_DeclarationId_CreatedAt ON dbo.DeclarationFlowLogs(DeclarationId, CreatedAt DESC);
GO
```

---

## 3. 表之间关系（外键级别）

1. `Users.DepartmentId -> Departments.Id`（用户所属部门）
2. `UserPreReviewDepartments.UserId -> Users.Id`
3. `UserPreReviewDepartments.DepartmentId -> Departments.Id`
4. `UserInitialReviewCategories.UserId -> Users.Id`
5. `UserInitialReviewCategories.ProjectCategoryId -> ProjectCategories.Id`
6. `DeclarationTasks.CreatedByUserId -> Users.Id`
7. `Declarations.TaskId -> DeclarationTasks.Id`
8. `Declarations.ApplicantUserId -> Users.Id`
9. `Declarations.DepartmentId -> Departments.Id`
10. `Declarations.ProjectCategoryId -> ProjectCategories.Id`
11. `DeclarationAttachments.DeclarationId -> Declarations.Id`
12. `DeclarationAttachments.UploadedByUserId -> Users.Id`
13. `DeclarationReviewRecords.DeclarationId -> Declarations.Id`
14. `DeclarationReviewRecords.ReviewedByUserId -> Users.Id`
15. `DeclarationFlowLogs.DeclarationId -> Declarations.Id`
16. `DeclarationFlowLogs.OperatorUserId -> Users.Id`

### 约束符合性说明

- **多轮审核**：`DeclarationReviewRecords` 每次审批新增记录，不更新历史。
- **状态追踪**：`DeclarationFlowLogs` 记录每次状态变化，满足流程审计。
- **附件支持**：`DeclarationAttachments` 与申报单一对多。
- **任务时间窗**：`DeclarationTasks` 提供 `StartAt/EndAt` 且有检查约束。
- **多选权限**：通过两张多对多关联表实现，不使用字符串拼接。

