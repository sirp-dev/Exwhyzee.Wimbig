CREATE PROCEDURE [dbo].[spUserAndRoleGetAll]


	@roleid nvarchar(max) = null,
	@startIndex int = 0,
@count int = 2147483647
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [AspNetUsers].Id, [AspNetUsers].UserName, [AspNetUsers].Email, [AspNetUsers].PhoneNumber, [AspNetUsers].DateRegistered, [AspNetUsers].AreaInCurrentCity, [AspNetUsers].RoleString,
	CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName, [Wallets].Balance, [AspNetUsers].CurrentCity, [PayOutDetails].Percentage
	 INTO #TempTableFiltered FROM [AspNetUsers]  
	LEFT JOIN [Wallets] ON [AspNetUsers].Id = [Wallets].UserId
	LEFT JOIN [PayOutDetails] ON [AspNetUsers].Id = [PayOutDetails].UserId


	WHERE 1 = 1 
	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [AspNetUsers]

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY UserName DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN
