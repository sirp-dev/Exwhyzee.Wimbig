CREATE TABLE [dbo].[WimbankTransfer]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Amount] decimal not null,
	[DateOfTransaction] datetime2 not null,
	[UserId] nvarchar(100) not null,
	[ReceiverId] nvarchar(100) not null,
	[Note] nvarchar(1000) not null,
	[TransactionStatus] int not null,
	[ReceiverPhone] nvarchar(100) not null
)
