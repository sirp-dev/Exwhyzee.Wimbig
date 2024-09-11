-- Get all images

Create PROCEDURE [dbo].[spSideBarnerFileGetAll]	


@dateStart datetime = null,
@dateEnd datetime = null,
@startIndex int = 0,
@count int = 2147483647,
@searchString nvarchar(200) = null

	
AS

	DECLARE @FilteredCount INT = 0
	DECLARE @TotalCount INT = 0

	SELECT [SideBarner].Id AS Id, [SideBarner].DateCreated, [SideBarner].Url, [SideBarner].ImageExtension, [SideBarner].TargetLocation
	INTO #TempTableFiltered FROM [SideBarner]  
	
	WHERE 1 = 1 

	--set table total
	SELECT @TotalCount = ISNULL(COUNT(Id),0) FROM [SideBarner] 

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