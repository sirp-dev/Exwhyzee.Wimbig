﻿CREATE TABLE [dbo].[MapImageToRaffle]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RaffleId] BIGINT NOT NULL,
	[ImageId] BIGINT NOT NULL,
	[DateCreated] DATETIME NOT NULL
)
