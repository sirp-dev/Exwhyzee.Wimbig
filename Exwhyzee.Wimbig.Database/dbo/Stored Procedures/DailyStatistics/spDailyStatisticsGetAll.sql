-- Get Category

CREATE PROCEDURE [dbo].[spDailyStatisticsGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[DailyStatistics], [dbo].[Section] as section WHERE 1=1
	
	AND(@dateStart is null OR [DailyStatistics].Date >= @dateStart)
	AND (@dateEnd is null OR [DailyStatistics].Date <= @dateEnd)
	AND(@search is null  OR [DailyStatistics].Date like '%'+@search+'%')

	ORDER BY [DailyStatistics].Date ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
