-- Get All Raffles

CREATE PROCEDURE [dbo].[spRaffleByArchieved]
@achieved BIT = null,
@status int = null,

@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null

	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Raffle].Id AS Id, [Name],[Description],NumberOfTickets,PricePerTicket,DeliveryType,HostedBy,StartDate, EndDate,[Status],DateCreated,TotalSold, [Raffle].SortOrder, [Raffle].Archived, [Raffle].PaidOut, [Raffle].Location, [Raffle].AreaInCity,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Raffle]  
	LEFT JOIN [AspNetUsers] ON [Raffle].HostedBy = [AspNetUsers].Id


	WHERE 1 = 1 
	AND (@status IS NULL OR [Status] = @status)
	AND (@searchString IS NULL OR [Name] Like @searchString)
	AND (@achieved IS NULL OR [Raffle].Archived Like @achieved)

	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Raffle] WITH(NOLOCK) WHERE ([Status] = 1)

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY SortOrder ASC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN


