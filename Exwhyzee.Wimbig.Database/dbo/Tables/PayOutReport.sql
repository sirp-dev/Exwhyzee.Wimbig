
CREATE TABLE [dbo].[PayOutReport]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[Date] datetime not null,
	[Amount] decimal null,
	[PercentageAmount] decimal null,
	[Percentage] int null,
	[Reference] int null,
	[Status] int null,
	[Note] nvarchar(max) null,
	[StartDate] datetime null,
	[EndDate] datetime null,
	[UserId] nvarchar(250) null
	

)
