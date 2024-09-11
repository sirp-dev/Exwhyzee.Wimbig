-- Get the first entity where CategooryId is eual provided id

CREATE PROCEDURE [dbo].[spAgentGetById]
	@id bigint = 0 
AS
BEGIN
	SELECT Top(1) * From [dbo].[Agent] WHERE [Id] = @id
END
