
CREATE PROCEDURE [dbo].[spWimbankReadAll]
--@status int = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [Wimbank].Id AS Id, [Wimbank].UserId,[Wimbank].Amount,[Wimbank].DateOfTransaction,[Wimbank].Balance,[Wimbank].TransactionStatus,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [Wimbank]  
	LEFT JOIN [AspNetUsers] ON [Wimbank].UserId = [AspNetUsers].Id


	WHERE 1 = 1 
	AND (@searchString IS NULL OR [UserName] Like @searchString)
	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Wimbank] WITH(NOLOCK)

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY Username DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN


