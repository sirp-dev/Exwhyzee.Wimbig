-- Get an image by identifier

CREATE PROCEDURE [dbo].[spBarnerImageFileGetById]
	@id bigint = 0

AS
BEGIN

	SELECT * From [dbo].[BarnerImage] Where [Id] = @id

END
