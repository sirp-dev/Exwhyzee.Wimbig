CREATE PROCEDURE [dbo].[spTicketByTransactionId]

@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null,
@transactionId bigint = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Ticket].Id AS Id, [Ticket].UserId,[Ticket].RaffleId,AspNetUsers.PhoneNumber,[Ticket].IsWinner, [Ticket].PlayerName, [Ticket].Email, 
	Ticket.YourPhoneNumber,Ticket.IsSentToStat, Ticket.TicketStatus, Ticket.CurrentLocation,
	CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName,Ticket.TicketNumber AS TicketNumber, 
	Ticket.DatePurchased,Ticket.TransactionId,Ticket.Price,Ticket.DateWon AS DateWon, Raffle.[Name] AS RaffleName,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Ticket]  
	LEFT JOIN [AspNetUsers] ON [Ticket].UserId = [AspNetUsers].Id
	LEFT JOIN [Raffle] ON [Ticket].RaffleId = [Raffle].Id
	WHERE [Ticket].TransactionId = @transactionId
	

	AND (@dateStart Is NULL OR [DatePurchased] >= @dateStart)
	AND (@dateEnd IS NULL OR [DatePurchased] <= @dateEnd)
	AND (@searchString IS NULL OR [TicketNumber] Like @searchString OR AspNetUsers.UserName = @searchString)
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Ticket] WITH(NOLOCK) 

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY DatePurchased DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN