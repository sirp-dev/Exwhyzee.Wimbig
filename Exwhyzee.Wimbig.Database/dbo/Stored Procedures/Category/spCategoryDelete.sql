-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spCategoryDelete]
	@categoryId int = 0
AS
BEGIN
	DELETE [dbo].[Category] WHERE 
	[CategoryId] = @categoryId
END
