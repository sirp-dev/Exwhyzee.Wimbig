-- Get an image by identifier

CREATE PROCEDURE [dbo].[spSideBarnerFileGetById]
	@id bigint = 0

AS
BEGIN

	SELECT * From [dbo].[SideBarner] Where [Id] = @id

END
