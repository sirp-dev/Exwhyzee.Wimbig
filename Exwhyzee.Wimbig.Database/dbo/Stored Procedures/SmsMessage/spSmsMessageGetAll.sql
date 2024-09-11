-- Get Category

CREATE PROCEDURE [dbo].[spSmsMessageGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT *
	FROM [dbo].[SmsMessage] 
	WHERE 1=1
	
	AND(@dateStart is null OR [SmsMessage].DateCreated >= @dateStart)
	AND (@dateEnd is null OR [SmsMessage].DateCreated <= @dateEnd)
	AND(@search is null  OR [SmsMessage].Message like '%'+@search+'%')

	ORDER BY [SmsMessage].DateCreated DESC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
