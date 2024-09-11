-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spCityGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[City] WHERE [Id] = @id
END
