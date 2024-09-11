--Create Entity

CREATE PROCEDURE [dbo].[spAreaInCityInsert]
		
	@name nvarchar(Max),
	@cityId bigint

		
AS
BEGIN
	INSERT INTO [dbo].[AreaInCity] ([Name],[CityId])
	output inserted.Id

	VALUES (@name,@cityId)
END
