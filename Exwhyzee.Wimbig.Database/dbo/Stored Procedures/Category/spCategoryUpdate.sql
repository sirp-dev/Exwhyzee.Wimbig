-- Update Category entity

CREATE PROCEDURE [dbo].[spCategoryUpdate]
	@categoryId int = 0,
	@name nvarchar(200),
	@dateCreated datetime,
	@description nvarchar(Max),
	@sectionId int,
	@entityStatus int

	
AS
BEGIN
	UPDATE [dbo].[Category] SET
	[Name]= @name,
	[DateCreated]=@dateCreated,
	[Description] = @description,
	
	[SectionId] = @sectionId,
	[EntityStatus] = @entityStatus

	WHERE [CategoryId] = @categoryId
END