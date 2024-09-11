CREATE TABLE [dbo].[Transactions]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[WalletdId] bigint not null,
	[UserId] nvarchar(100) not null,
	[Amount] decimal not null,
	[TransactionType] int not null,
	[DateOfTransaction] datetime2 not null,
	[Status] int not null, 
    [TransactionReference] NVARCHAR(50) NULL,
	[Sender] nvarchar(100) null, 
    [Description] NVARCHAR(MAX) NULL
	
	
)