-- Get an image by identifier

CREATE PROCEDURE [dbo].[spYoutubeLinkGetById]
	@id bigint = 0

AS
BEGIN

	SELECT * From [dbo].[YoutubeLinks] Where [Id] = @id

END
