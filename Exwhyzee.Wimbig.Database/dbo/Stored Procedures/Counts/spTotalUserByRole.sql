-- Get All Raffles

CREATE PROCEDURE [dbo].[spTotalUserByRole]


	@roleid nvarchar(max) = null,
	@startIndex int = 0,
@count int = 2147483647
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [AspNetUsers].Id, [AspNetUsers].UserName, [AspNetRoles].Name AS RoleName, [AspNetUsers].Email, [AspNetUsers].PhoneNumber, [AspNetUsers].DateRegistered,
	CONCAT(AspNetUsers.FirstName, + ' ' + AspNetUsers.LastName, + ' ' + AspNetUsers.OtherNames) AS FullName, [Wallets].Balance, [AspNetUsers].CurrentCity, [PayOutDetails].Percentage
	 INTO #TempTableFiltered FROM [AspNetUsers]  
	LEFT JOIN [AspNetUserRoles] ON [AspNetUsers].Id = [AspNetUserRoles].UserId
	LEFT JOIN [AspNetRoles] ON [AspNetUserRoles].RoleId = [AspNetRoles].Id
	LEFT JOIN [Wallets] ON [AspNetUsers].Id = [Wallets].UserId
	LEFT JOIN [PayOutDetails] ON [AspNetUsers].Id = [PayOutDetails].UserId


	WHERE 1 = 1 
	AND (@roleid IS NULL OR [AspNetUserRoles].RoleId = @roleid)
	
	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [AspNetUsers]

	--set filtered count
	SELECT @FilteredCount = ISNULL(COUNT(Id),0) FROM #TempTableFiltered

	--Select table result
	SELECT * FROM #TempTableFiltered
	ORDER BY RoleName DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN
