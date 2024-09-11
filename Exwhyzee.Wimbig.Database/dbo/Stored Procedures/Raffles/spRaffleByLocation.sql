﻿-- Get All Raffles

CREATE PROCEDURE [dbo].[spRaffleByLocation]
@status int = null,
@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Raffle].Id AS Id, [Name],[Description],NumberOfTickets,PricePerTicket,DeliveryType,HostedBy,StartDate, EndDate,[Status],DateCreated,TotalSold, [Raffle].Archived, [Raffle].PaidOut, [Raffle].SortOrder, [Raffle].Location, [Raffle].AreaInCity,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Raffle]  
	LEFT JOIN [AspNetUsers] ON [Raffle].HostedBy = [AspNetUsers].Id


	WHERE 1 = 1 
	AND (@dateStart Is NULL OR [DateCreated] >= @dateStart)
	AND (@dateEnd IS NULL OR [DateCreated] <= @dateEnd)
	AND (@status IS NULL OR [Status] = @status)
	AND (@searchString IS NULL OR [Location] Like @searchString)

	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Raffle] WITH(NOLOCK) WHERE ([Status] = 1)

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY DateCreated DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN


