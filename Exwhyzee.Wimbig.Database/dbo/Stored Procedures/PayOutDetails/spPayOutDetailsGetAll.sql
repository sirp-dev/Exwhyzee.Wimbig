-- Get Category

CREATE PROCEDURE [dbo].[spPayOutDetailsGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[PayOutDetails] WHERE 1=1
	
	
	AND(@search is null  OR [PayOutDetails].Percentage like '%'+@search+'%')

	ORDER BY [PayOutDetails].Id ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
