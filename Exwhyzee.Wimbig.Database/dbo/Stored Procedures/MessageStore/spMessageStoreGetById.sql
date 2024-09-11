-- Read Section By Id

CREATE PROCEDURE [dbo].[spMessageStoreGetById]
	@id bigint = 0
AS
BEGIN
	SELECT * FROM [dbo].[MessageStore] WHERE [Id] = @id
END
