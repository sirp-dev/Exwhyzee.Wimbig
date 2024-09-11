
CREATE PROCEDURE [dbo].[spTransferWimbankReadAll]
--@status int = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [WimbankTransfer].Id AS Id, [WimbankTransfer].UserId, [WimbankTransfer].ReceiverId, [WimbankTransfer].Amount, [WimbankTransfer].TransactionStatus, [WimbankTransfer].ReceiverPhone, [WimbankTransfer].DateOfTransaction AS TransactionDate,
	[AspNetUsers].UserName AS Username INTO #TempTableFiltered FROM [WimbankTransfer]  
	LEFT JOIN [AspNetUsers] ON [WimbankTransfer].UserId = [AspNetUsers].Id
	


	WHERE 1 = 1 
	AND (@searchString IS NULL OR [UserName] Like @searchString)
	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [Wimbank] WITH(NOLOCK)

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY TransactionDate DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN


