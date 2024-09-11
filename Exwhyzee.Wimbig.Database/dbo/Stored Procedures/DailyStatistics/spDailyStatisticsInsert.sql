--Create Category Entity

CREATE PROCEDURE [dbo].[spDailyStatisticsInsert]
		@date datetime,
		@totalUsers int,
		@totalTickets int,
		@totalRaffle int,
		@totalCash decimal,
		@totalWalletCash decimal

		
AS
BEGIN
	INSERT INTO [dbo].[DailyStatistics] ([Date],[TotalUsers],[TotalTickets],[TotalRaffle],[TotalCash],[TotalWalletCash])
	output inserted.Id

	VALUES (@date,@totalUsers,@totalTickets,@totalRaffle,@totalCash,@totalWalletCash)
END
