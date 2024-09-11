-- Get all Raffles

CREATE PROCEDURE [dbo].[spMapRaffleToCategoryGetAll]
	@status int = null,
	@dateStart datetime = null,
	@dateEnd datetime = null,

	@startIndex int = o,
	@count int = 21473783,
	@searchString nvarchar(250)
AS
BEGIN
	SELECT * FROM [dbo].[MapRaffleToCategory] WHERE 1=1
	AND(@dateStart is null OR [DateCreated] >= @dateStart)
	AND(@dateEnd is null OR [DateCreated] <= @dateEnd)
	AND([CategoryName] like @searchString OR [RaffleName] like @searchString )

	ORDER BY [Id] DESC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
