﻿CREATE PROCEDURE [dbo].[spTicketsReadAllByReferenceId]
@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null,
@referenceId nvarchar(200) = null,
@isWinner bit = null,
@isSentToStat bit = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Ticket].Id AS Id, [Ticket].DatePurchased, [Ticket].UserId,[Ticket].RaffleId, [Ticket].TicketStatus AS TicketStatus,AspNetUsers.PhoneNumber, [Ticket].IsWinner,[Ticket].PlayerName, [Ticket].Email, Ticket.[DatePurchased] AS Date, Ticket.[TicketStatus] AS Status,
	Ticket.YourPhoneNumber,Ticket.IsSentToStat, Ticket.CurrentLocation,
	CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName,Ticket.TicketNumber AS TicketNumber,Ticket.TransactionId,Ticket.Price,Ticket.DateWon AS DateWon, Raffle.[Name] AS RaffleName,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Ticket]  
	LEFT JOIN [AspNetUsers] ON [Ticket].UserId = [AspNetUsers].Id
	LEFT JOIN [Raffle] ON [Ticket].RaffleId = [Raffle].Id
	left join [AspNetUserRoles] on [AspNetUsers].Id = [AspNetUserRoles].UserId
	
	LEFT JOIN [AspNetRoles] on [AspNetUserRoles].RoleId = [AspNetRoles].Id
	
	 
	WHERE 1=1 
	AND (PlayerName <> UserName)
	AND ([AspNetRoles].Name = 'Agent' OR [AspNetRoles].Name = 'DGAs')
	AND (@dateStart Is NULL OR [DatePurchased] >= @dateStart)
	AND (@dateEnd IS NULL OR [DatePurchased] <= @dateEnd)
	AND (@referenceId IS NULL OR [AspNetUsers].ReferenceId = @referenceId)
	AND (@searchString IS NULL OR AspNetUsers.UserName = @searchString)
	AND (@isWinner IS NULL OR [IsWinner] Like @isWinner)
	AND (@isSentToStat IS NULL OR [IsSentToStat] Like @isSentToStat)
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
