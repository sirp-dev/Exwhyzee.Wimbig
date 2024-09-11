-- Get All Raffles

CREATE PROCEDURE [dbo].[spWimbankGetAll]

	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null
AS
BEGIN
	SELECT [Wimbank].Id, [Wimbank].TransactionStatus, [Wimbank].DateOfTransaction, [Wimbank].Amount, [Wimbank].Balance, [Wimbank].TransactionStatus, [AspNetUsers].PhoneNumber AS PhoneNumber,
	[AspNetUsers].UserName AS Username
	FROM [dbo].[Wimbank] 
	left join [AspNetUsers] on [Wimbank].ReceiverId = [AspNetUsers].Id
	
	
	
	
	WHERE 1=1
	AND(@dateStart is null OR [Wimbank].[DateOfTransaction] >= @dateStart)
	AND (@dateEnd is null OR [Wimbank].[DateOfTransaction] <= @dateEnd)
	AND(@search is null OR [Wimbank].[Amount] like @search OR [Wimbank].[TransactionStatus] like @search)

	ORDER BY [Wimbank].[DateOfTransaction] DESC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY
END
