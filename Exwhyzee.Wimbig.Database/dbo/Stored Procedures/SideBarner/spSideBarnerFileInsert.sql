-- Create a new image entity

CREATE PROCEDURE [dbo].[spSideBarnerFileInsert]
	@url nvarchar(299),	
	@dateCreated dateTime,
	@status int,
	@isDefault bit,
	@targetLocation nvarchar(500)
AS
BEGIN

	INSERT INTO [dbo].[SideBarner] ([Url],[DateCreated],[Status],[IsDefault], [TargetLocation])

	output inserted.Id

	VALUES(@url,@dateCreated,@status,@isDefault,@targetLocation )
	                      
	END
