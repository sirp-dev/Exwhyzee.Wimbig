CREATE TABLE [dbo].[Agent]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Fullname] NVARCHAR(MAX) NOT NULL, 
    [EmailAddress] NVARCHAR(50) NOT NULL ,
	[State] NVARCHAR(50) NOT NULL,
	[LGA] NVARCHAR(50) NOT NULL,
    [AreYouNewToWimbig] NVARCHAR(MAX) NULL, 
    [ContactAddress] NVARCHAR(MAX) NULL DEFAULT 0, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [CurrentOccupation] NVARCHAR(MAX) NULL, 
    [PhoneNumber] NVARCHAR(MAX) NULL,
	[ShopLocation] NVARCHAR(MAX) NULL , 
    [Gender] NCHAR(10) NULL, 
    [Status] INT NULL
)
