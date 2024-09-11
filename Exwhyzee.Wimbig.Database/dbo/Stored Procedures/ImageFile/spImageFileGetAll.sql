-- Get all images

Create PROCEDURE [dbo].[spImageFileGetAll]	
	@extension nvarchar(10) = null,

	@dateStart datetime = null,
	@dateStop datetime = null,

	@startIndex int = 0,
	@count int = 2147483647
AS
BEGIN 
	SELECT * From [dbo].[ImageFile] WHere 1=1 

	AND (@extension IS NULL or [ImageExtension] = @extension)
	AND(@dateStart IS NULL OR [DateCreated] >= @dateStart)
	AND (@dateStop IS NULL OR [DateCreated] <= @dateStop)

	ORDER BY [DateCreated] ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END