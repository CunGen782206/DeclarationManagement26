IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Departments] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [SortOrder] int NOT NULL,
    [IsEnabled] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);

CREATE TABLE [ProjectCategories] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [SortOrder] int NOT NULL,
    [IsEnabled] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ProjectCategories] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [JobNumber] nvarchar(50) NOT NULL,
    [FullName] nvarchar(50) NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [PasswordHash] nvarchar(200) NOT NULL,
    [PasswordSalt] nvarchar(200) NULL,
    [IsSuperAdmin] bit NOT NULL,
    [IsEnabled] bit NOT NULL,
    [LastLoginAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [DeclarationTasks] (
    [Id] bigint NOT NULL IDENTITY,
    [TaskName] nvarchar(200) NOT NULL,
    [StartAt] datetime2 NOT NULL,
    [EndAt] datetime2 NOT NULL,
    [IsEnabled] bit NOT NULL,
    [CreatedByUserId] bigint NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_DeclarationTasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeclarationTasks_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [UserInitialReviewCategories] (
    [UserId] bigint NOT NULL,
    [ProjectCategoryId] bigint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_UserInitialReviewCategories] PRIMARY KEY ([UserId], [ProjectCategoryId]),
    CONSTRAINT [FK_UserInitialReviewCategories_ProjectCategories_ProjectCategoryId] FOREIGN KEY ([ProjectCategoryId]) REFERENCES [ProjectCategories] ([Id]),
    CONSTRAINT [FK_UserInitialReviewCategories_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [UserPreReviewDepartments] (
    [UserId] bigint NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_UserPreReviewDepartments] PRIMARY KEY ([UserId], [DepartmentId]),
    CONSTRAINT [FK_UserPreReviewDepartments_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
    CONSTRAINT [FK_UserPreReviewDepartments_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [Declarations] (
    [Id] bigint NOT NULL IDENTITY,
    [TaskId] bigint NOT NULL,
    [ApplicantUserId] bigint NOT NULL,
    [PrincipalName] nvarchar(100) NOT NULL,
    [ContactPhone] nvarchar(50) NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [ProjectName] nvarchar(300) NOT NULL,
    [ProjectCategoryId] bigint NOT NULL,
    [ProjectLevel] int NOT NULL,
    [AwardLevel] int NOT NULL,
    [ParticipationType] int NOT NULL,
    [ApprovalDocumentName] nvarchar(300) NULL,
    [SealUnitAndDate] nvarchar(300) NULL,
    [ProjectContent] nvarchar(max) NULL,
    [ProjectAchievement] nvarchar(max) NULL,
    [CurrentStatus] int NOT NULL,
    [CurrentNode] int NOT NULL,
    [VersionNo] int NOT NULL,
    [SubmittedAt] datetime2 NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Declarations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Declarations_DeclarationTasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [DeclarationTasks] ([Id]),
    CONSTRAINT [FK_Declarations_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
    CONSTRAINT [FK_Declarations_ProjectCategories_ProjectCategoryId] FOREIGN KEY ([ProjectCategoryId]) REFERENCES [ProjectCategories] ([Id]),
    CONSTRAINT [FK_Declarations_Users_ApplicantUserId] FOREIGN KEY ([ApplicantUserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [DeclarationAttachments] (
    [Id] bigint NOT NULL IDENTITY,
    [DeclarationId] bigint NOT NULL,
    [OriginalFileName] nvarchar(260) NOT NULL,
    [StorageFileName] nvarchar(260) NOT NULL,
    [StoragePath] nvarchar(500) NOT NULL,
    [ContentType] nvarchar(100) NULL,
    [FileSizeBytes] bigint NOT NULL,
    [UploadedByUserId] bigint NOT NULL,
    [UploadedAt] datetime2 NOT NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_DeclarationAttachments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeclarationAttachments_Declarations_DeclarationId] FOREIGN KEY ([DeclarationId]) REFERENCES [Declarations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DeclarationAttachments_Users_UploadedByUserId] FOREIGN KEY ([UploadedByUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DeclarationFlowLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [DeclarationId] bigint NOT NULL,
    [FromStatus] int NULL,
    [ToStatus] int NOT NULL,
    [ActionType] int NOT NULL,
    [OperatorUserId] bigint NOT NULL,
    [Note] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_DeclarationFlowLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeclarationFlowLogs_Declarations_DeclarationId] FOREIGN KEY ([DeclarationId]) REFERENCES [Declarations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DeclarationFlowLogs_Users_OperatorUserId] FOREIGN KEY ([OperatorUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DeclarationReviewRecords] (
    [Id] bigint NOT NULL IDENTITY,
    [DeclarationId] bigint NOT NULL,
    [ReviewStage] int NOT NULL,
    [ReviewAction] int NOT NULL,
    [Reason] nvarchar(1000) NULL,
    [RecognizedProjectLevel] int NULL,
    [RecognizedAwardLevel] int NULL,
    [RecognizedAmount] decimal(18,2) NULL,
    [Remark] nvarchar(1000) NULL,
    [ReviewedByUserId] bigint NOT NULL,
    [ReviewedAt] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_DeclarationReviewRecords] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DeclarationReviewRecords_Declarations_DeclarationId] FOREIGN KEY ([DeclarationId]) REFERENCES [Declarations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DeclarationReviewRecords_Users_ReviewedByUserId] FOREIGN KEY ([ReviewedByUserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_DeclarationAttachments_DeclarationId] ON [DeclarationAttachments] ([DeclarationId]);

CREATE INDEX [IX_DeclarationAttachments_UploadedByUserId] ON [DeclarationAttachments] ([UploadedByUserId]);

CREATE INDEX [IX_DeclarationFlowLogs_DeclarationId] ON [DeclarationFlowLogs] ([DeclarationId]);

CREATE INDEX [IX_DeclarationFlowLogs_OperatorUserId] ON [DeclarationFlowLogs] ([OperatorUserId]);

CREATE INDEX [IX_DeclarationReviewRecords_DeclarationId] ON [DeclarationReviewRecords] ([DeclarationId]);

CREATE INDEX [IX_DeclarationReviewRecords_ReviewedByUserId] ON [DeclarationReviewRecords] ([ReviewedByUserId]);

CREATE INDEX [IX_Declarations_ApplicantUserId] ON [Declarations] ([ApplicantUserId]);

CREATE INDEX [IX_Declarations_DepartmentId] ON [Declarations] ([DepartmentId]);

CREATE INDEX [IX_Declarations_ProjectCategoryId] ON [Declarations] ([ProjectCategoryId]);

CREATE INDEX [IX_Declarations_TaskId] ON [Declarations] ([TaskId]);

CREATE INDEX [IX_DeclarationTasks_CreatedByUserId] ON [DeclarationTasks] ([CreatedByUserId]);

CREATE UNIQUE INDEX [IX_Departments_Name] ON [Departments] ([Name]);

CREATE UNIQUE INDEX [IX_ProjectCategories_Name] ON [ProjectCategories] ([Name]);

CREATE INDEX [IX_UserInitialReviewCategories_ProjectCategoryId] ON [UserInitialReviewCategories] ([ProjectCategoryId]);

CREATE INDEX [IX_UserPreReviewDepartments_DepartmentId] ON [UserPreReviewDepartments] ([DepartmentId]);

CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);

CREATE UNIQUE INDEX [IX_Users_JobNumber] ON [Users] ([JobNumber]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317085607_InitialCreateV2', N'9.0.0');

COMMIT;
GO

