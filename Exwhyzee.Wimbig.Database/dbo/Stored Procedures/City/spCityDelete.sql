-- Delete a category by its Id

CREATE PROCEDURE [dbo].[spCityDelete]
	@Id int = 0
AS
BEGIN
	DELETE [dbo].[City] WHERE 
	[Id] = @Id
END
