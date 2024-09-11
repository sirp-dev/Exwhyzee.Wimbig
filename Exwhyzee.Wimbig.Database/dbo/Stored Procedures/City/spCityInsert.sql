--Create Entity

CREATE PROCEDURE [dbo].[spCityInsert]
		
	@name nvarchar(Max)
		
AS
BEGIN
	INSERT INTO [dbo].[City] ([Name])
	output inserted.Id

	VALUES (@name)
END
