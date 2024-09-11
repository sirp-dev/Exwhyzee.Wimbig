-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spPayOutDetailsGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[PayOutDetails] WHERE [Id] = @id
END
