IF NOT EXISTS (
    SELECT 1 
    FROM sys.schemas 
    WHERE name = N'task'
)
BEGIN
    EXEC('CREATE SCHEMA task');
END;

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[task].[Status]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [task].[Status] (
        [Id] INT PRIMARY KEY IDENTITY(1,1), -- Klucz g³ówny, autoinkrementacja
        [CreateUser_Id] INT NOT NULL, -- ID u¿ytkownika, który utworzy³ wpis
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(), -- Data utworzenia (domyœlnie bie¿¹ca data)
        [ModifyUser_Id] INT NULL, -- ID u¿ytkownika, który zmodyfikowa³ wpis
        [ModifyDate] DATETIME NULL, -- Data modyfikacji
        [IsActive] BIT NOT NULL DEFAULT 1, -- Czy status jest aktywny (1 = aktywny, 0 = nieaktywny)
        [Name] NVARCHAR(100) NOT NULL, -- Nazwa statusu
        [StrongName] NVARCHAR(100) NOT NULL -- Nazwa techniczna statusu (np. do u¿ycia w kodzie)
    );
END

GO

INSERT INTO [task].[Status] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName])
SELECT 1, GETDATE(), 1, 'Nowy', 'Backlog'
WHERE NOT EXISTS (SELECT 1 FROM [task].[Status] WHERE [StrongName] = 'Backlog');

INSERT INTO [task].[Status] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName])
SELECT 1, GETDATE(), 1, 'W toku', 'InProgress'
WHERE NOT EXISTS (SELECT 1 FROM [task].[Status] WHERE [StrongName] = 'InProgress');

INSERT INTO [task].[Status] ([CreateUser_Id], [CreateDate], [IsActive], [Name], [StrongName])
SELECT 1, GETDATE(), 1, 'Zakoñczony', 'Finished'
WHERE NOT EXISTS (SELECT 1 FROM [task].[Status] WHERE [StrongName] = 'Finished');

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[task].[Task]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [task].[Task] (
        [Id] INT PRIMARY KEY IDENTITY(1,1), -- Klucz g³ówny, autoinkrementacja
    	[CreateUser_Id] INT NOT NULL, -- ID u¿ytkownika, który utworzy³ wpis
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(), -- Data utworzenia (domyœlnie bie¿¹ca data)
        [ModifyUser_Id] INT NULL, -- ID u¿ytkownika, który zmodyfikowa³ wpis
        [ModifyDate] DATETIME NULL, -- Data modyfikacji
        [IsActive] BIT NOT NULL DEFAULT 1, -- Czy status jest aktywny (1 = aktywny, 0 = nieaktywny)
        [Title] NVARCHAR(255) NOT NULL, -- Tytu³ zadania
        [Deadline] DATETIME NULL, -- Termin wykonania zadania (mo¿e byæ NULL)
        [Status_Id] INT NOT NULL FOREIGN KEY REFERENCES [task].[Status](Id), -- ID statusu, klucz obcy do tabeli Status
        [User_Id] INT, -- ID u¿ytkownika, klucz obcy do tabeli User
        [EstimatedHours] DECIMAL(5,2) NULL, -- Liczba godzin przeznaczonych na zadanie
        [IsImportant] BIT NOT NULL DEFAULT 0 -- Flaga okreœlaj¹ca, czy zadanie jest wa¿ne (0 = nie, 1 = tak)
    );
END

GO

IF NOT EXISTS (
    SELECT 1 
    FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[task].[TaskFile]') 
      AND type = N'U'
)
BEGIN
    CREATE TABLE [task].[TaskFile] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
    	[CreateUser_Id] INT NOT NULL,
        [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifyUser_Id] INT NULL,
        [ModifyDate] DATETIME NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [Task_Id] INT NOT NULL FOREIGN KEY REFERENCES [task].[Task](Id),
        [Guid] UNIQUEIDENTIFIER NOT NULL,
        [FileName] NVARCHAR(MAX) NOT NULL,
        [User_Id] INT NULL,
        [RoleStrongName] NVARCHAR(250) NULL
    );
END

