IF NOT EXISTS (
    SELECT 1 
    FROM sys.schemas 
    WHERE name = N'bilboard'
)
BEGIN
    EXEC('CREATE SCHEMA bilboard');
END;
GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[bilboard].[BilboardItemStatus]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [bilboard].[BilboardItemStatus] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [CreateUser_Id] INT NOT NULL, 
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifyUser_Id] INT NULL,
        [ModifyDate] DATETIME NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [Name] NVARCHAR(100) NOT NULL,
        [StrongName] NVARCHAR(100) NOT NULL,
        [ExternalId] INT NOT NULL
    );
END

GO

INSERT INTO [bilboard].[BilboardItemStatus] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName],[ExternalId])
SELECT 1, GETDATE(), 1, 'Nowy', 'Backlog',1
WHERE NOT EXISTS (SELECT 1 FROM [bilboard].[BilboardItemStatus] WHERE [StrongName] = 'Backlog');

INSERT INTO [bilboard].[BilboardItemStatus] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName],[ExternalId])
SELECT 1, GETDATE(), 1, 'W toku', 'InProgress',2
WHERE NOT EXISTS (SELECT 1 FROM [bilboard].[BilboardItemStatus] WHERE [StrongName] = 'InProgress');

INSERT INTO [bilboard].[BilboardItemStatus] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName],[ExternalId])
SELECT 1, GETDATE(), 1, 'Zakoñczony', 'Finished',3
WHERE NOT EXISTS (SELECT 1 FROM [bilboard].[BilboardItemStatus] WHERE [StrongName] = 'Finished');

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[bilboard].[Bilboard]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [bilboard].[Bilboard] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [CreateUser_Id] INT NOT NULL,
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifyUser_Id] INT NULL,
        [ModifyDate] DATETIME NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [User_Id] INT NOT NULL
    );
END

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[bilboard].[Task]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [bilboard].[Task] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
    	[CreateUser_Id] INT NOT NULL,
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifyUser_Id] INT NULL,
        [ModifyDate] DATETIME NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [Title] NVARCHAR(255) NOT NULL,
        [Deadline] DATETIME NULL,
        [Status_Id] INT NOT NULL FOREIGN KEY REFERENCES [bilboard].[BilboardItemStatus](Id),   
    	[EstimatedHours] DECIMAL(5,2) NULL,
        [IsImportant] BIT NOT NULL DEFAULT 0,
        [External_Id] INT NOT NULL
    );
END

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[bilboard].[BilboardItem]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [bilboard].[BilboardItem] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
    	[CreateUser_Id] INT NOT NULL,
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifyUser_Id] INT NULL,
        [ModifyDate] DATETIME NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [Bilboard_Id] INT NOT NULL FOREIGN KEY REFERENCES [bilboard].[Bilboard](Id),
        [Hours] DECIMAL(5,2) NULL,
        [Task_Id] INT NOT NULL FOREIGN KEY REFERENCES [bilboard].[Task](Id),
    );
END

