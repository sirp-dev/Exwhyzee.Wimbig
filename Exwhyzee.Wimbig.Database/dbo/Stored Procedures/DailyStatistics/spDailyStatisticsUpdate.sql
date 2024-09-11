-- Update Category entity

CREATE PROCEDURE [dbo].[spDailyStatisticsUpdate]
	@Id bigint,
	@date datetime,
		@totalUsers int,
		@totalTickets int,
		@totalRaffle int,
		@totalCash decimal,
		@totalWalletCash decimal

	
AS
BEGIN
	UPDATE [dbo].[DailyStatistics] SET
	[Date]= @date,
	[TotalUsers] = @totalUsers,
	[TotalTickets] = @totalTickets,
	[TotalRaffle] = @totalRaffle,
	[TotalCash] = @totalCash,
	[TotalWalletCash] = @totalWalletCash

	WHERE [Id] = @Id
END