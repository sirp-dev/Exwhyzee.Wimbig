/*
The database must have a MEMORY_OPTIMIZED_DATA filegroup
before the memory optimized object can be created.

The bucket count should be set to about two times the 
maximum expected number of distinct values in the 
index key, rounded up to the nearest power of two.
*/

CREATE TABLE [dbo].[DailyStatistics]
(
	[Id] BIGINT NOT NULL PRIMARY KEY Identity(1,1),
	[Date] datetime not null,
	[TotalUsers] int null,
	[TotalTickets] int null,
	[TotalRaffle] int null,
	[TotalCash] decimal null,
	[TotalWalletCash] decimal null

)
