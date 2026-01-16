-- SQL Script to create tables for ProGress in SSMS

-- 1. Table Users (for local login and cloned users)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255),
    FullName NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 2. Table Technicians
CREATE TABLE Technicians (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255),
    Phone NVARCHAR(50),
    IsActive BIT DEFAULT 1
);

-- 3. Table SaleManagers
CREATE TABLE SaleManagers (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1
);

-- 4. Table Software
CREATE TABLE Software (
    Id NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    IsActive BIT DEFAULT 1
);

-- 5. Table Tasks
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

-- 6. Table TaskAttachments
CREATE TABLE TaskAttachments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId NVARCHAR(50) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_TaskAttachments_Task FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
);

-- 7. Table TaskImages
CREATE TABLE TaskImages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId NVARCHAR(50) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_TaskImages_Task FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE
);

-- 8. Table TaskHistory
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

-- 9. Table Questions
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

-- 10. Table QuestionAttachments
CREATE TABLE QuestionAttachments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestionId NVARCHAR(50) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_QuestionAttachments_Question FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
);

-- 11. Table QuestionImages
CREATE TABLE QuestionImages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    QuestionId NVARCHAR(50) NOT NULL,
    Url NVARCHAR(MAX) NOT NULL,
    CONSTRAINT FK_QuestionImages_Question FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
);

-- Insert Default Data (as currently in FirebaseService)
INSERT INTO Technicians (Id, Name, Email, IsActive) VALUES 
('tech1', N'Đinh Trọng Đạt', 'dinh.trong.dat@example.com', 1),
('tech2', N'Nguyễn Đình Việt', 'nguyen.dinh.viet@example.com', 1),
('tech3', N'Ngô Mạnh Hà', 'ngo.manh.ha@example.com', 1),
('tech4', N'Trịnh Tiến Anh', 'trinh.tien.anh@example.com', 1);

INSERT INTO SaleManagers (Id, Name, IsActive) VALUES 
('sale1', N'Ms. Nguyệt', 1),
('sale2', N'Ms. Trang', 1),
('sale3', N'Ms. Như', 1),
('sale4', N'Ms. Huyền', 1),
('sale5', N'Ms. Hiền', 1);

INSERT INTO Software (Id, Name, IsActive) VALUES 
('soft1', 'WDL', 1), ('soft2', 'PBC', 1), ('soft3', 'PFD 2025', 1), 
('soft4', 'IFD', 1), ('soft5', 'RCBC', 1), ('soft6', 'EtabsGen', 1), 
('soft7', 'RCSC', 1), ('soft8', 'WLA', 1), ('soft9', 'PFD2015', 1), 
('soft10', 'RCC', 1), ('soft11', 'QS Smart', 1), ('soft12', 'PFD', 1), 
('soft13', 'RCB', 1), ('soft14', 'DG', 1), ('soft15', 'KSD', 1), 
('soft16', 'PBC 2025', 1);

