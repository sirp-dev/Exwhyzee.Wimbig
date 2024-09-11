CREATE TABLE [dbo].[MapRaffleToCategory]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CategoryId] int NOT NULL,
	[CategoryName] NVARCHAR(200) NULL,
	[RaffleId] BIGINT NOT NULL,
	[RaffleName] NVARCHAR(250) NULL,
	[DateCreated] DATETIME NOT NULL
)
