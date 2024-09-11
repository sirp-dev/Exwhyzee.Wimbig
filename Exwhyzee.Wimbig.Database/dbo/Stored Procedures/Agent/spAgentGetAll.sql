-- Get Category

CREATE PROCEDURE [dbo].[spAgentGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[Agent] WHERE 1=1
	
	AND(@dateStart is null OR [Agent].DateCreated >= @dateStart)
	AND (@dateEnd is null OR [Agent].DateCreated <= @dateEnd)
	AND(@search is null  OR [Agent].DateCreated like '%'+@search+'%')

	ORDER BY [Agent].DateCreated ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
