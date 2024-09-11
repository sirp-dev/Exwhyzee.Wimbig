-- Get all Section with pagination and search params

CREATE PROCEDURE [dbo].[spSectionGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null
AS
BEGIN
	SELECT * FROM [dbo].[Section] WHERE 1=1
	AND(@dateStart is null OR [DateCreated] >= @dateStart)
	AND (@dateEnd is null OR [DateCreated] <= @dateEnd)
	AND(@search is null OR [Name] like @search OR [Description] like @search)

	ORDER BY [Name] ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY
END
