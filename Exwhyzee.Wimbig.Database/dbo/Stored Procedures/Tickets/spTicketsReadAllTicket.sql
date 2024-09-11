CREATE PROCEDURE [dbo].[spTicketsReadAllTicket]
@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null,
@raffleId bigint = null,
@isWinner bit = 1,
@isSentToStat bit = null
	-- Get All Raffles

AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Ticket].Id AS Id, [Ticket].UserId, [Ticket].IsSentToStat, [Ticket].RaffleId, [Ticket].DatePurchased,[Ticket].Email, [Ticket].IsWinner, [Ticket].PlayerName, [Ticket].Price, [Ticket].TicketNumber, [Ticket].TicketStatus, [Ticket].YourPhoneNumber, CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName, [AspNetUsers].UserName AS UserName, [Ticket].TransactionId, [Ticket].DateWon AS DateWon, Raffle.[Name] AS RaffleName, Ticket.CurrentLocation
	INTO #TempTableFiltered FROM [Ticket]  
	LEFT JOIN [AspNetUsers] ON [Ticket].UserId = [AspNetUsers].Id
	LEFT JOIN [Raffle] ON [Ticket].RaffleId = [Raffle].Id

	WHERE @isWinner IS NULL OR [Ticket].IsWinner Like @isWinner
	
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


