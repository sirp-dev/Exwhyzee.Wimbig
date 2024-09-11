--Delete an image

CREATE PROCEDURE [dbo].[spSideBarnerFileDelete]
	@imageId bigInt = 0
AS
BEGIN
	Delete From [dbo].[SideBarner] Where [Id] = @imageId
END