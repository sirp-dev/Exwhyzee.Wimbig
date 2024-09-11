-- Update  entity

CREATE PROCEDURE [dbo].[spCityUpdate]
	@Id bigint,
	@name nvarchar(Max)
	
AS
BEGIN
	UPDATE [dbo].[City] SET
	[Name] = @name

	WHERE [Id] = @Id
END