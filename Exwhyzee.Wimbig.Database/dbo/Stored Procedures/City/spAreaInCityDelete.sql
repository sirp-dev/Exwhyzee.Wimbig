-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spAreaInCityDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[AreaInCity] WHERE 
	[Id] = @Id
END
