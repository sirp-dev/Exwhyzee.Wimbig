-- Get Category

CREATE PROCEDURE [dbo].[spPayOutReportGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT [PayOutReport].Id, [PayOutReport].Date, [PayOutReport].Amount, [PayOutReport].PercentageAmount, [PayOutReport].StartDate, [PayOutReport].EndDate, [PayOutReport].UserId,
	[PayOutReport].Percentage, [PayOutReport].Reference, [PayOutReport].Status,
	[PayOutReport].Note, AspNetUsers.UserName AS UserName
	FROM [dbo].[PayOutReport] 
	LEFT JOIN [AspNetUsers] ON [AspNetUsers].Id = [PayOutReport].UserId
	WHERE 1=1
	
	AND(@dateStart is null OR [PayOutReport].StartDate >= @dateStart)
	AND (@dateEnd is null OR [PayOutReport].EndDate <= @dateEnd)
	AND(@search is null  OR [PayOutReport].UserId like '%'+@search+'%')

	ORDER BY [PayOutReport].Date ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
