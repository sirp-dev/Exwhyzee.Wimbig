CREATE TABLE [dbo].[WimPayHistory]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Amount] decimal not null,
	[DateOfTransaction] datetime2 not null,
	[SenderId] nvarchar(100) not null,
	[ReceiverId] nvarchar(100) not null,
	[Note] nvarchar(1000) not null,
	[Status] int not null
)
