
-- Update image file properties

CREATE PROCEDURE [dbo].[spImageFileUpdate]
	@id bigint,
	@url varchar(250),
	@extension varchar(250),
	@dateCreated datetime,
	@status int
AS
BEGIN
	UPDATE [dbo].[ImageFile] SET
	[Url] = @url,
	[ImageExtension] =@extension,
	[DateCreated]=@dateCreated,
	[Status]=@status

	WHERE [Id]=@id

END
