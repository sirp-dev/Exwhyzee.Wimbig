CREATE TABLE [dbo].[Raffle]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(250) NULL,
	[Description] nvarchar(Max) null,
	[NumberOfTickets] int null,
	[PricePerTicket] decimal null,
	[HostedBy] NVARCHAR(250) null,
	[DeliveryType] int,
	[StartDate] datetime null,
	[EndDate] datetime null,
	[Status] int,
	[DateCreated] datetime null, 
	[TotalSold] int null,
	[SortOrder] int null,
	[Archived] BIT NOT NULL DEFAULT 0,
	[PaidOut] BIT NOT NULL DEFAULT 0,
	[Location] nvarchar(500) null, 
    [AreaInCity] NVARCHAR(500) NULL
	

)
