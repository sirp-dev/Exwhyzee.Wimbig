CREATE TABLE [dbo].[Wallets]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] nvarchar(100) not null,
	[Balance] decimal not null,
	[DateUpdated] datetime not null
	
	
)