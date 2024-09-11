-- Get Category

CREATE PROCEDURE [dbo].[spMessageStoreGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[MessageStore] 
	WHERE [MessageChannel] = 20
	
	AND(@search is null OR [MessageStore].PhoneNumber like @search OR [MessageStore].EmailAddress like '%'+@search+'%')

	ORDER BY [MessageStore].Id DESC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
