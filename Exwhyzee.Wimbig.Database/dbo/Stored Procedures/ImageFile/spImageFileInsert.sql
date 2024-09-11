-- Create a new image entity

CREATE PROCEDURE [dbo].[spImageFileInsert]
	@url nvarchar(299),	
	@dateCreated dateTime,
	@status int,
	@isDefault bit
AS
BEGIN

	INSERT INTO [dbo].[ImageFile] ([Url],[DateCreated],[Status],[IsDefault])

	output inserted.Id

	VALUES(@url,@dateCreated,@status,@isDefault )
	                      
	END
