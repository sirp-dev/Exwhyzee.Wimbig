-- Create a new image entity

CREATE PROCEDURE [dbo].[spYoutubeLinkInsert]
	@url nvarchar(299),	
	@title nvarchar(299),	
	@dateCreated dateTime
AS
BEGIN

	INSERT INTO [dbo].[YoutubeLinks] ([Url], [Title], [DateCreated])

	output inserted.Id

	VALUES(@url,@title,@dateCreated )
	                      
	END
