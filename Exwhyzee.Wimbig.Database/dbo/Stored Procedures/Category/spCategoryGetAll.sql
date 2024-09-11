-- Get Category

CREATE PROCEDURE [dbo].[spCategoryGetAll]
	@status int = 0,
	@dateStart datetime = null,
	@dateEnd datetime = null,
	
	@startIndex int = 0,
	@count int = 2147483647,
	@search nvarchar(250)= null

	
AS
BEGIN
	SELECT cat.[CategoryId], cat.[Name],cat.[Description],cat.[DateCreated], cat.[SectionId], 
	section.[Name] as SectionName, section.[Description] as SectionDescription, section.[DateCreated] as SectionDateCreated

	FROM [dbo].[Category] as cat, [dbo].[Section] as section WHERE 1=1
	ANd (cat.[EntityStatus] != 2 and section.[EntityStatus] != 2)
	And(cat.[SectionId] = section.[SectionId])
	AND(@dateStart is null OR cat.[DateCreated] >= @dateStart)
	AND (@dateEnd is null OR cat.[DateCreated] <= @dateEnd)
	AND(@search is null OR cat.[Name] like @search OR cat.[Description] like '%'+@search+'%')

	ORDER BY cat.[Name] ASC
	OFFSET @startIndex ROWS
	FETCH NEXT @count ROWS ONLY

END
