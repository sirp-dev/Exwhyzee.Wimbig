CREATE TABLE [dbo].[Wimbank]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Amount] decimal not null,
	[DateOfTransaction] datetime2 not null,
	[UserId] nvarchar(100) not null,
	[Balance] decimal not null,
	[Note] nvarchar(1000) null,
	[TransactionStatus] int not null,
	[ReceiverId] nvarchar(100) null
)
