-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spCategoryGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[Category] WHERE [CategoryId] = @id AND [EntityStatus] != 2
END
