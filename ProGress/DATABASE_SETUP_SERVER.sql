-- SQL Script to create tables for ProGress on Server
-- Safe version: Checks if tables exist before creating

USE [testwefl_ProGress];
GO

-- 1. Table Users (for local login and cloned users)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL UNIQUE,
        Email NVARCHAR(255),
        FullName NVARCHAR(255),
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table Users created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Users already exists.';
END
GO

-- 2. Table Technicians
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Technicians]') AND type in (N'U'))
BEGIN
    CREATE TABLE Technicians (
        Id NVARCHAR(50) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255),
        Phone NVARCHAR(50),
        IsActive BIT DEFAULT 1
    );
    PRINT 'Table Technicians created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Technicians already exists.';
END
GO

-- 3. Table SaleManagers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaleManagers]') AND type in (N'U'))
BEGIN
    CREATE TABLE SaleManagers (
        Id NVARCHAR(50) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        IsActive BIT DEFAULT 1
    );
    PRINT 'Table SaleManagers created successfully.';
END
ELSE
BEGIN
    PRINT 'Table SaleManagers already exists.';
END
GO

-- 4. Table Software
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Software]') AND type in (N'U'))
BEGIN
    CREATE TABLE Software (
        Id NVARCHAR(50) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        IsActive BIT DEFAULT 1
    );
    PRINT 'Table Software created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Software already exists.';
END
GO

-- 5. Table Tasks
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tasks]') AND type in (N'U'))
BEGIN
    CREATE TABLE Tasks (
        Id NVARCHAR(50) PRIMARY KEY,
        Tag NVARCHAR(100),
        Description NVARCHAR(MAX),
        CustomerName NVARCHAR(255),
        FileReceivedDate DATETIME,
        SupportPlatform INT, -- Enum: 0=Zalo, 1=MemberSupport, 2=CustomerContactSale
        SaleManagerId NVARCHAR(50),
        SoftwareId NVARCHAR(50),
        AssignedTo NVARCHAR(50),
        Status INT, -- Enum: 0=Pending, 1=InProgress, 2=Waiting, 3=Completed, 4=Paused
        ExpectedCompletionDate DATETIME,
        CompletedDate DATETIME,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME,
        Solution NVARCHAR(MAX),
        ResponseToCustomer NVARCHAR(MAX),
        IsSaved BIT DEFAULT 0,
        CONSTRAINT FK_Tasks_SaleManager FOREIGN KEY (SaleManagerId) REFERENCES SaleManagers(Id),
        CONSTRAINT FK_Tasks_Software FOREIGN KEY (SoftwareId) REFERENCES Software(Id),
        CONSTRAINT FK_Tasks_Technician FOREIGN KEY (AssignedTo) REFERENCES Technicians(Id)
    );
    PRINT 'Table Tasks created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Tasks already exists.';
END
GO

-- 6. Table TaskAttachments
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskAttachments]') AND type in (N'U'))
BEGIN
    CREATE TABLE TaskAttachments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TaskId NVARCHAR(50) NOT NULL,
        Url NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_TaskAttachments_Task FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
    );
    PRINT 'Table TaskAttachments created successfully.';
END
ELSE
BEGIN
    PRINT 'Table TaskAttachments already exists.';
END
GO

-- 7. Table TaskImages
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskImages]') AND type in (N'U'))
BEGIN
    CREATE TABLE TaskImages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TaskId NVARCHAR(50) NOT NULL,
        Url NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_TaskImages_Task FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
    );
    PRINT 'Table TaskImages created successfully.';
END
ELSE
BEGIN
    PRINT 'Table TaskImages already exists.';
END
GO

-- 8. Table TaskHistory
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE TaskHistory (
        Id NVARCHAR(50) PRIMARY KEY,
        TaskId NVARCHAR(50) NOT NULL,
        Action NVARCHAR(100),
        Description NVARCHAR(MAX),
        ChangedBy NVARCHAR(100),
        ChangedByName NVARCHAR(255),
        ChangedDate DATETIME DEFAULT GETDATE(),
        OldValue NVARCHAR(MAX),
        NewValue NVARCHAR(MAX),
        CONSTRAINT FK_TaskHistory_Task FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
    );
    PRINT 'Table TaskHistory created successfully.';
END
ELSE
BEGIN
    PRINT 'Table TaskHistory already exists.';
END
GO

-- 9. Table Questions
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Questions]') AND type in (N'U'))
BEGIN
    CREATE TABLE Questions (
        Id NVARCHAR(50) PRIMARY KEY,
        TechnicianId NVARCHAR(50),
        Question NVARCHAR(MAX),
        Answer NVARCHAR(MAX),
        Status INT, -- Enum: 0=Pending, 1=Answered
        CreatedDate DATETIME DEFAULT GETDATE(),
        AnsweredDate DATETIME,
        CONSTRAINT FK_Questions_Technician FOREIGN KEY (TechnicianId) REFERENCES Technicians(Id)
    );
    PRINT 'Table Questions created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Questions already exists.';
END
GO

-- 10. Table QuestionAttachments
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionAttachments]') AND type in (N'U'))
BEGIN
    CREATE TABLE QuestionAttachments (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        QuestionId NVARCHAR(50) NOT NULL,
        Url NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_QuestionAttachments_Question FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
    );
    PRINT 'Table QuestionAttachments created successfully.';
END
ELSE
BEGIN
    PRINT 'Table QuestionAttachments already exists.';
END
GO

-- 11. Table QuestionImages
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuestionImages]') AND type in (N'U'))
BEGIN
    CREATE TABLE QuestionImages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        QuestionId NVARCHAR(50) NOT NULL,
        Url NVARCHAR(MAX) NOT NULL,
        CONSTRAINT FK_QuestionImages_Question FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
    );
    PRINT 'Table QuestionImages created successfully.';
END
ELSE
BEGIN
    PRINT 'Table QuestionImages already exists.';
END
GO

-- Insert Default Data (only if tables are empty)
IF NOT EXISTS (SELECT * FROM Technicians)
BEGIN
    INSERT INTO Technicians (Id, Name, Email, IsActive) VALUES 
    ('tech1', N'Đinh Trọng Đạt', 'dinh.trong.dat@example.com', 1),
    ('tech2', N'Nguyễn Đình Việt', 'nguyen.dinh.viet@example.com', 1),
    ('tech3', N'Ngô Mạnh Hà', 'ngo.manh.ha@example.com', 1),
    ('tech4', N'Trịnh Tiến Anh', 'trinh.tien.anh@example.com', 1);
    PRINT 'Default Technicians data inserted.';
END
GO

IF NOT EXISTS (SELECT * FROM SaleManagers)
BEGIN
    INSERT INTO SaleManagers (Id, Name, IsActive) VALUES 
    ('sale1', N'Ms. Nguyệt', 1),
    ('sale2', N'Ms. Trang', 1),
    ('sale3', N'Ms. Như', 1),
    ('sale4', N'Ms. Huyền', 1),
    ('sale5', N'Ms. Hiền', 1);
    PRINT 'Default SaleManagers data inserted.';
END
GO

IF NOT EXISTS (SELECT * FROM Software)
BEGIN
    INSERT INTO Software (Id, Name, IsActive) VALUES 
    ('soft1', 'WDL', 1), ('soft2', 'PBC', 1), ('soft3', 'PFD 2025', 1), 
    ('soft4', 'IFD', 1), ('soft5', 'RCBC', 1), ('soft6', 'EtabsGen', 1), 
    ('soft7', 'RCSC', 1), ('soft8', 'WLA', 1), ('soft9', 'PFD2015', 1), 
    ('soft10', 'RCC', 1), ('soft11', 'QS Smart', 1), ('soft12', 'PFD', 1), 
    ('soft13', 'RCB', 1), ('soft14', 'DG', 1), ('soft15', 'KSD', 1), 
    ('soft16', 'PBC 2025', 1);
    PRINT 'Default Software data inserted.';
END
GO

PRINT 'Database setup completed successfully!';
GO

