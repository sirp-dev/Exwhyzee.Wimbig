CREATE PROCEDURE [dbo].[spWimbankGetById]
	@wimbankId bigint = 0
AS
Begin
	SELECT * from [dbo].[Wimbank] Where [Id] = @wimbankId
	
End