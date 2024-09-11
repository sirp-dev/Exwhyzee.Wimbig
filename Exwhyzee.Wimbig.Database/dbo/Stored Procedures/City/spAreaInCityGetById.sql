-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spAreaInCityGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[AreaInCity] WHERE [Id] = @id
END
