-- Get Category

CREATE PROCEDURE [dbo].[spWinnerReportGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[WinnerReport] WHERE 1=1
	
	AND(@search is null  OR [WinnerReport].DateCreated like '%'+@search+'%')

	ORDER BY [WinnerReport].DateCreated ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
