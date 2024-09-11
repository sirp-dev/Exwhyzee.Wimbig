
CREATE TABLE [dbo].[PayOutDetails]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[Percentage] int null,
	[AccountNumber] nvarchar(max) null,
	[AccountName] nvarchar(max) null,
	[BankName] nvarchar(max) null,
	[UserId] nvarchar(250) null
	

)
