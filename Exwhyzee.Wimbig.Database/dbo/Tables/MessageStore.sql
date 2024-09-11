CREATE TABLE [dbo].[MessageStore]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [EmailAddress] NVARCHAR(50) NULL, 
    [MessageStatus] INT NOT NULL DEFAULT 20,
	[MessageChannel] INT NOT NULL DEFAULT 0,
	[MessageType] INT NULL,
    [Message] NVARCHAR(MAX) NULL, 
    [Retries] INT NOT NULL DEFAULT 0, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [DateSent] DATETIME2 NULL, 
    [PhoneNumber] NVARCHAR(15) NULL,
	[AddressType] INT NOT NULL DEFAULT 10, 
    [ImageUrl] NVARCHAR(MAX) NULL, 
    [Response] NVARCHAR(MAX) NULL
)
