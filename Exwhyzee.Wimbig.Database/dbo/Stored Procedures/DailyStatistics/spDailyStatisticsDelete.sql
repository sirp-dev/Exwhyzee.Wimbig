-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spDailyStatisticsDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[DailyStatistics] WHERE 
	[Id] = @Id
END
