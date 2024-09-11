-- Get All Raffles

CREATE PROCEDURE [dbo].[spTransactionsGetAllByReferenceId]
@userId nvarchar(100) = null,
@walletId bigint = null,
@status int = null,
@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Transactions].Id AS Id, [Transactions].WalletdId,[Transactions].Amount,[Transactions].DateOfTransaction,[Transactions].TransactionReference,[Transactions].Status,[Transactions].TransactionType, [Transactions].Description, [Transactions].UserId,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Transactions]  
	LEFT JOIN [AspNetUsers] ON [Transactions].UserId = [AspNetUsers].Id


	WHERE 1 = 1 
	AND (@dateStart Is NULL OR [DateOfTransaction] >= @dateStart)
	AND (@dateEnd IS NULL OR [DateOfTransaction] <= @dateEnd)
	AND (@status IS NULL OR [Status] = @status)
	AND (@searchString IS NULL OR [UserName] Like @searchString)
	AND (@walletId IS NULL OR [WalletdId] = @walletId)
	AND (@userId IS NULL OR [UserId] = @userId)
	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Transactions] WITH(NOLOCK) WHERE ([Status] = 1)

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY DateOfTransaction DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN


