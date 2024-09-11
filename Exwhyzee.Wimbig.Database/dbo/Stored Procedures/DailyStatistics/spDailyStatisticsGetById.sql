-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spDailyStatisticsGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[DailyStatistics] WHERE [Id] = @id
END
