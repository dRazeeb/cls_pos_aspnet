CREATE TABLE ProductCategory
(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Define ID as the primary key
    Name NVARCHAR(150) NOT NULL,
    ParentId INT NULL,
    Description NVARCHAR(255) NULL,
    Status BIT DEFAULT 1,
    CreatedBy INT NULL, -- Foreign key field
    CONSTRAINT FK_CreatedBy_Core_UserInfo_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Core_UserInfo(ID),
    UpdatedBy INT NULL, -- Foreign key field
    CONSTRAINT FK_UpdatedBy_Core_UserInfo_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Core_UserInfo(ID),
    UpdatedAt DATETIME NULL,
    CreatedAt DATETIME DEFAULT GETDATE() -- Set a default value for UpdatedAt or make it nullable
);