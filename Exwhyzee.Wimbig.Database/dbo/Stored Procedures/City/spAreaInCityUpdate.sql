-- Update  entity

CREATE PROCEDURE [dbo].[spAreaInCityUpdate]
	@Id bigint,
	@name nvarchar(Max),
	@cityId bigint

	
AS
BEGIN
	UPDATE [dbo].[AreaInCity] SET
	[Name] = @name,
	[CityId] = @cityId

	WHERE [Id] = @Id
END