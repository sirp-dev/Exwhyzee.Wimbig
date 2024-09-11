CREATE TABLE [dbo].[Ticket]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] nvarchar(100) not null,
	[RaffleId] bigint not null,
	[TicketNumber] nvarchar(100) not null,
	[DatePurchased] datetime not null,
	[TransactionId] bigint not null,
	[Price] decimal(8,2) not null, 
    [IsWinner] BIT NOT NULL DEFAULT 0,

	[YourPhoneNumber] nvarchar(13) NOT NULL ,
	[IsSentToStat] BIT NOT NULL DEFAULT 0,
	[TicketStatus] int,
	[DateWon] datetime null,
	[Email] nvarchar(200) null,
	[PlayerName] nvarchar(200) null, 
	[PaidOut] BIT NOT NULL DEFAULT 0,
	[CurrentLocation] nvarchar(500) null

	
)