-- Get an image by identifier

CREATE PROCEDURE [dbo].[spImageFileGetById]
	@id bigint = 0

AS
BEGIN

	SELECT * From [dbo].[ImageFile] Where [Id] = @id

END
