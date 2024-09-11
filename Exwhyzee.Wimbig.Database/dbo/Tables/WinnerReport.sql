CREATE TABLE [dbo].[WinnerReport]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[WinnerName] nvarchar(MAX) NOT NULL,
	[WinnerPhoneNumber] nvarchar(50) null,
	[WinnerEmail] NVARCHAR(50) null,
	[WinnerLocation] NVARCHAR(MAX) null,
	[AmountPlayed] DECIMAL(8, 2) null,
	[RaffleName] NVARCHAR(50),
	[RaffleId] BIGINT null,
	[TicketNumber] INT null,
	[ItemCost] DECIMAL(8, 2),
	[DateCreated] datetime not null, 
	[DateWon] datetime not null, 
	[DateDelivered] datetime not null, 
	[DeliveredBy] NVARCHAR(MAX) null,
	[DeliveredPhone] NVARCHAR(50) null,
	[DeliveryAddress] NVARCHAR(50) NULL ,
	[TotalAmountPlayed] DECIMAL(8, 2) NULL DEFAULT 0,
	[UserId] NVARCHAR(50) NOT NULL,
	[Status] int,
	

)
