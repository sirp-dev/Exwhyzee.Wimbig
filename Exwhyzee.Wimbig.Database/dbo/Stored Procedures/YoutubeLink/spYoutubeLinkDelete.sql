--Delete an image

CREATE PROCEDURE [dbo].[spYoutubeLinkDelete]
	@imageId bigInt = 0
AS
BEGIN
	Delete From [dbo].[YoutubeLinks] Where [Id] = @imageId
END