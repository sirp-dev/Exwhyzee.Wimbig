CREATE PROCEDURE [dbo].[spAreaCityByCityName]

@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null,
@cityId bigint = null
	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [AreaInCity].Id AS Id, [AreaInCity].Name, City.Name AS CityName, City.Id AS CityId
 FROM [AreaInCity]  
	LEFT JOIN [City] ON [AreaInCity].Id = [City].Id
	WHERE [AreaInCity].CityId = @cityId
	

	AND (@searchString IS NULL OR [City].Name Like @searchString OR [AreaInCity].Name = @searchString)
	--set table total
	

	ORDER BY Id DESC
	OFFSET @startIndex ROWS 
	FETCH NEXT @count ROWS ONLY

	--SELECT Summary Result
	SELECT @FilteredCount AS FilteredCount, @TotalCount AS TotalCount

	RETURN